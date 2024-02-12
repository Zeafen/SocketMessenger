using MessangerServer.Database_Models;
using MessangerServer.DBContext;
using MessangerServer.Models;
using MessangerServer.Status;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Data;

namespace MessangerServer
{
    public class ServerProcess : IDisposable
    {
        private TaskFactory ClientsProccessing;
        private MessageRepository messageContainer;
        public ObservableCollection<User> Clients { get; set; }
        private UserContext context;
        private TcpListener Server;
        private CancellationTokenSource cts;

        public ServerProcess()
        {
            Clients = new ObservableCollection<User>();
            ClientsProccessing = new TaskFactory();
            messageContainer = new MessageRepository();
            Server = new TcpListener(Application.Current.Resources["EndPoint"] as IPEndPoint);
            Server.Start();
            context = new UserContext();
            cts = new CancellationTokenSource();
            object lockClients = new object();
            BindingOperations.EnableCollectionSynchronization(Clients, lockClients);
            ClientsProccessing.StartNew(RecieveMessage, cts.Token);
        }

        private async void RecieveMessage()
        {

            while (true)
            {
                if (!cts.IsCancellationRequested)
                {
                    try
                    {
                        var client = Server.AcceptTcpClient();
                        var ClientStream = client.GetStream();
                        var reader = new BinaryReader(ClientStream);
                        switch ((MessageType)reader.ReadByte())
                        {
                            case MessageType.Register:
                                User newAccount = new User(reader.ReadString(), reader.ReadString(), reader.ReadString(), ClientStream);
                                if (Clients.FirstOrDefault(u => u.Login == newAccount.Login) != null)
                                {
                                    newAccount.mesWriter.Write((byte)RequestStatus.User_AlredyExists);
                                    break;
                                }
                                context.Users.Add(newAccount);
                                context.SaveChanges();
                                messageContainer.InitializeUserStorage(newAccount.User_Key);
                                Clients.Add(newAccount);
                                ClientsProccessing.StartNew(() => ProccessClient(newAccount), cts.Token);
                                using (var str = new MemoryStream())
                                {
                                    using (var wr = new BinaryWriter(str))
                                        wr.Write((byte)RequestStatus.Logged_In);
                                    newAccount.mesWriter.Write(str.ToArray());
                                }
                                break;
                            case MessageType.Login:
                                string Login = reader.ReadString();
                                string Password = reader.ReadString();
                                User account = context.Users.FirstOrDefault(c => c.Login == Login && c.Password == Password);
                                if (account != null)
                                {
                                    account.SetNewStream(ClientStream);
                                    using (var str = new MemoryStream())
                                    {
                                        using (var wr = new BinaryWriter(str))
                                            wr.Write((byte)RequestStatus.Logged_In);
                                        account.mesWriter.Write(str.ToArray());
                                    }
                                    messageContainer.InitializeUserStorage(account.User_Key);
                                    Clients.Add(account);
                                    ClientsProccessing.StartNew(() => ProccessClient(account));
                                }
                                else
                                    ClientStream.WriteByte((byte)RequestStatus.User_DoesntExist);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }
        }

        private async void ProccessClient(User user)
        {
            while (true)
            {
                if (!cts.IsCancellationRequested)
                {

                    try
                    {
                        int mesID;
                        Message mes;
                        switch ((MessageType)user.mesReader.ReadByte())
                        {
                            case MessageType.Send:
                                mes = Message.GetMessage(user.mesReader);
                                if (context.IsUserBanned(user))
                                {
                                    user.mesWriter.Write((byte)MessageType.Ban);
                                    user.mesWriter.Write(context.BanList.First(b => b.User_ID == user.Id).Ban_Reason);
                                    break;
                                }
                                await Task.Run(async () =>
                                {
                                    foreach (var client in Clients)
                                    {
                                        if (client.User_Key != user.User_Key)
                                            if (client.mesWriter != null)
                                                client.mesWriter.Write(Message.CreateSendRequest(user.Name, mes));
                                    }
                                    messageContainer.AddMessage(user.User_Key, mes);
                                });
                                break;
                            case MessageType.Delete:
                                mesID = user.mesReader.ReadInt32();
                                if (context.IsUserBanned(user))
                                {
                                    user.mesWriter.Write((byte)MessageType.Ban);
                                    user.mesWriter.Write(context.BanList.First(b => b.User_ID == user.Id).Ban_Reason);
                                    break;
                                }
                                await Task.Run(async () =>
                                {
                                    foreach (var client in Clients)
                                    {
                                        if (client.User_Key != user.User_Key)
                                            if (client.mesWriter != null)
                                            {
                                                client.mesWriter.Write(Message.CreateDeleteRequest(user.Name, mesID));
                                            }
                                    }
                                    messageContainer.DeleteMessage(user.User_Key, mesID);
                                });
                                break;
                            case MessageType.Edit:
                                mesID = user.mesReader.ReadInt32();
                                string newText = user.mesReader.ReadString();
                                if (context.IsUserBanned(user))
                                {
                                    user.mesWriter.Write((byte)MessageType.Ban);
                                    user.mesWriter.Write("You\'re banned. Reason: " + context.BanList.First(b => b.User_ID == user.Id).Ban_Reason);
                                    break;
                                }
                                await Task.Run(async () =>
                                {
                                    foreach (var client in Clients)
                                    {
                                        if (client.User_Key != user.User_Key)
                                            if (client.mesWriter != null)
                                            {
                                                client.mesWriter.Write(Message.CreateEditRequest(user.Name, mesID, newText));
                                            }
                                    }
                                    messageContainer.UpdateMessage(user.User_Key, mesID, newText);
                                });
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        messageContainer.ClearUserStorage(user.User_Key);
                        Clients.Remove(Clients.FirstOrDefault(x => x.Id == user.Id));
                        user.mesReader.Close();
                        user.mesWriter.Close();
                        break;
                    }
                }
            }
        }

        public void BanPerson(User selectedUser)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("You can\'t ban nobody", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var dlg = new BanDialog(selectedUser, context);
            dlg.ShowDialog();
        }

        public void RemovePerson(User selectedUser)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("You can\'t ban nobody", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            messageContainer.ClearUserStorage(selectedUser.User_Key);
            Clients.Remove(Clients.FirstOrDefault(u => u.Login == selectedUser.Login));
            selectedUser?.mesReader.Close();
            selectedUser?.mesWriter.Close();
        }

        public void Dispose()
        {
            cts.Cancel();
            Server.Stop();
        }
    }
}
