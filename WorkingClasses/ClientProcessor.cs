using Primary_Massager.Comands;
using Primary_Massager.Models;
using Primary_Massager.Status;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Primary_Massager.WorkingClasses
{
    public class ClientProcessor : IDisposable
    {
        private EditCommand _editCommand = null;
        public EditCommand EditCommand => _editCommand ?? (_editCommand = new EditCommand());

        private RelayCommand<Message> _deleteCommand = null;
        public RelayCommand<Message> DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand<Message>(SendDelete, CanSendDelete));


        public ObservableCollection<Message> incomingMessages {  get; private set; }
        CancellationTokenSource cts;
        Task responsesProccessing;
        BinaryWriter mesWriter;
        BinaryReader mesReader;

        public ClientProcessor() : this(Application.Current.Resources["User_Reader"] as BinaryReader, Application.Current.Resources["User_Writer"] as BinaryWriter) { }
        public ClientProcessor(BinaryReader reader, BinaryWriter writer)
        {
            incomingMessages = new ObservableCollection<Message>();
            mesReader = reader;
            mesWriter = writer;
            cts = new CancellationTokenSource();
            responsesProccessing = Task.Run(() => GetResponse(mesReader, cts));
        }

        public void SendRequest(string messageText)
        {
            try
            {
                Message sendMessage = new Message(messageText);
                mesWriter.Write(Message.CreateSendRequest(MessageType.Send, sendMessage));
                incomingMessages.Add(sendMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GetResponse(BinaryReader reader, CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        Message message = null;
                        string sender;
                        int mesId;
                        switch ((MessageType)mesReader.ReadByte())
                        {
                            case MessageType.Send:
                                message = new Message(reader.ReadString(), reader.ReadInt32(), reader.ReadString(), Convert.ToDateTime(reader.ReadString()));
                                incomingMessages.Add(message);
                                break;
                            case MessageType.Edit:
                                sender = reader.ReadString();
                                mesId = reader.ReadInt32();
                                string newText = reader.ReadString();
                                Message editedMes = incomingMessages.FirstOrDefault(m => m.Message_ID == mesId && m.SenderName == sender);
                                if (message != null)
                                    message.Text = newText;
                                break;
                            case MessageType.Delete:
                                sender = reader.ReadString();
                                mesId = reader.ReadInt32();
                                Message deletedMes = incomingMessages.FirstOrDefault(m => m.Message_ID == mesId && m.SenderName == sender);
                                if (message != null)
                                    incomingMessages.Remove(deletedMes);
                                break;
                            case MessageType.Ban:
                                MessageBox.Show(reader.ReadString(), "Fool", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }
            }
        }


        public void SendEdit(Message selectedMessage, string newText)
        {
            try
            {
                mesWriter.Write(Message.CreateEditDeleteRequest(MessageType.Edit, selectedMessage.Message_ID, newText));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SendDelete(Message selectedMessage)
        {
            mesWriter.Write(Message.CreateEditDeleteRequest(MessageType.Delete, selectedMessage.Message_ID));
            incomingMessages.Remove(selectedMessage);
        }

        private bool CanSendDelete(Message message) => message != null && message is Message;
        public void Dispose()
        {

            mesReader.Close();
            mesWriter.Close();
            cts.Cancel();
            responsesProccessing.Wait();
        }
    }
}
