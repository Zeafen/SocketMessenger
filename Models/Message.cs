using MessangerServer.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MessangerServer.Models
{
    [Serializable]
    public class Message
    {
        public Message() { }

        public Message(int message_Id,string text, DateTime sendDate)
        {
            Message_ID = message_Id;
            Text = text;
            SendDate = sendDate;
            IsEdited = false;
        }
        public string Text { get; set; }

        public readonly int Message_ID;

        public DateTime SendDate { get; private set; }

        public bool IsEdited { get; private set; }

        public static byte[] CreateSendRequest(string sender_Name, Message message)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write((byte)MessageType.Send);
                    writer.Write(sender_Name);
                    writer.Write(message.Message_ID);
                    writer.Write(message.Text);
                    writer.Write(message.SendDate.ToString());
                }
                return ms.ToArray();
            }
        }

        public static byte[] CreateEditRequest(string  userName, int messageID, string newText)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write((byte)MessageType.Edit);
                    writer.Write(userName);
                    writer.Write(messageID);
                    writer.Write(newText);
                }
                return ms.ToArray();
            }
        }

        public static byte[] CreateDeleteRequest(string userName, int messageID)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write((byte)MessageType.Delete);
                    writer.Write(userName);
                    writer.Write(messageID);
                }
                return ms.ToArray();
            }
        }

        public static Message GetMessage(BinaryReader reader)
        {
            Message message;
            message = new Message(reader.ReadInt32(), reader.ReadString(), Convert.ToDateTime(reader.ReadString()));
            return message;
        }
    }
}
