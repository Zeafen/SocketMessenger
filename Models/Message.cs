using Primary_Massager.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Primary_Massager.Models
{
    public class Message : UIElement, INotifyPropertyChanged
    {
        public Message() { }
        public Message(string text)
        {
            IsOwned = true;
            Message_ID = ID_Counter++;
            _Text = text;
            SendDate = DateTime.Now;
            IsOwned = true;
        }
        public Message(string senderName, int message_ID, string text, DateTime sendDate)
        {
            IsOwned = false;
            SenderName=senderName;
            Message_ID=message_ID;
            _Text = text;
            SendDate = sendDate;
        }


        public static int ID_Counter = 0;
        public readonly int Message_ID;
        public string SenderName { get; private set; }

        public bool IsOwned { get; init; } = false;

        private bool _IsEditing { get; set; } = false;
        public bool IsEditing
        {
            get => _IsEditing;
            set
            {
                if (_IsEditing == value)
                    return;
                _IsEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        private string _Text {  get; set; }
        public string Text { get => _Text; set
            {
                if (_Text == value) return;
                _Text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public DateTime SendDate { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //this method creates byte array. It represents the whole message(body)
        public static byte[] CreateSendRequest(MessageType type_of_message, Message message)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write((byte)type_of_message);    
                    writer.Write(message.Message_ID);
                    writer.Write(message.Text);
                    writer.Write(message.SendDate.ToString());
                }
                return ms.ToArray();
            }
        }

        //Creates delete or Edit request, depends on type_of_message
        public static byte[] CreateEditDeleteRequest(MessageType type_of_message, int message_ID, string newText="")
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write((byte)type_of_message);
                    writer.Write(message_ID);
                    if (type_of_message == MessageType.Edit)
                        writer.Write(newText);
                }
                return ms.ToArray();
            }
        }

        //This method shall get the message from server's response.
        public static Message GetMessage(Stream responseStream)
        {
            Message message;
            using (var reader = new BinaryReader(responseStream))
            {
                message = new Message(reader.ReadString(), reader.ReadInt32(), reader.ReadString(), Convert.ToDateTime(reader.ReadString()));
            }
            return message;
        }
    }
}
