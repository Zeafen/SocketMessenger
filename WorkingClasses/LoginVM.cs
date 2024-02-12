using Primary_Massager.Models;
using Primary_Massager.Status;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Printing;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Primary_Massager.WorkingClasses
{
    public class LoginVM
    {

        public UserModel User { get; set; } = new UserModel();
        public string Server_IP { get; set; }
        public int Port { get; set; }

        public string UserLogin {  get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public MessageType ReqType { get; set; }

        public LoginVM()
        {
            ReqType = MessageType.Login;
        }

        public Stream? SendRequest()
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(User.Server_IP, int.Parse(User.Port));
            }
            catch (Exception)
            {
                MessageBox.Show("Enabled to connect. Check if Hostname or/and port is correct.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
                return null;
            }
            byte[] request;
            //Serializing required data to byte array
            using (var ms = new MemoryStream())
            {
                switch (ReqType)
                {
                    case MessageType.Login:
                        using (var writer = new BinaryWriter(ms))
                        {
                            writer.Write((byte)ReqType);
                            writer.Write(UserLogin);
                            writer.Write(Password);
                        }
                        break;
                    case MessageType.Register:
                        using (var writer = new BinaryWriter(ms))
                        {
                            writer.Write((byte)ReqType);
                            writer.Write(Username);
                            writer.Write(UserLogin);
                            writer.Write(Password);
                        }
                        break;
                }
                request = ms.ToArray();
            }
            //sending request and checking if login and password are correct
            var stream = client.GetStream();
            stream.Write(request);
            var reader = new BinaryReader(stream);
            MainWindow mw;
            switch ((RequestStatus)reader.ReadByte())
            {
                case RequestStatus.Logged_In:
                    Application.Current.Resources.Add("User_Reader", reader);
                    Application.Current.Resources.Add("User_Writer", new BinaryWriter(stream));
                    return stream;
                case RequestStatus.Registered:
                    Application.Current.Resources.Add("User_Reader", reader);
                    Application.Current.Resources.Add("User_Writer", new BinaryWriter(stream));
                    return stream;
                case RequestStatus.User_DoesntExist:
                    MessageBox.Show("User with this login doesn't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    stream.Close();
                    return null;
                case RequestStatus.Canceled:
                    MessageBox.Show("Incorrect password. Please? check if you haven't made any mitrake");
                    stream.Close();
                    return null;
            }
            return null;
        }
    }
}
