using MessangerServer.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MessangerServer.Models
{
    public class MessageRepository
    {
        private Dictionary<string, List<Message>> UsersMessages;
        private Dictionary<string, List<Message>> DisconectedUsers;
        public MessageRepository(Dictionary<string, List<Message>> userMessages)
        {
            UsersMessages = userMessages;
        }
        public MessageRepository()
        {
            UsersMessages  =new Dictionary<string, List<Message>>();
        }

        public bool UserStorageExists(string user_Key) => UsersMessages.ContainsKey(user_Key);
        /// <summary>
        /// Deletes user's storage
        /// </summary>
        /// <param name="User_Key"></param>
        /// <returns>false if storage doesn't exist; true if operation succeed</returns>
        public bool ClearUserStorage(string User_Key)
        {
            if (!UsersMessages.ContainsKey(User_Key))
                return false;
            UsersMessages.Remove(User_Key);
            return true;
        }
        /// <summary>
        /// Adds message to the user's repository
        /// </summary>
        /// <param name="User_Key"></param>
        /// <param name="message"></param>
        /// <returns>true if message was successfully added; false if repository doesn't exist</returns>
        public bool AddMessage(string User_Key, Message message) 
        {
            if (!UsersMessages.ContainsKey(User_Key))
                return false;
            UsersMessages[User_Key].Add(message);
            return true;
        }
        /// <summary>
        /// creates message repository for user
        /// </summary>
        /// <param name="User_Key"></param>
        /// <returns>true if operation succeed; false if not</returns>
        public bool InitializeUserStorage(string User_Key)
        {
            if (UsersMessages.ContainsKey(User_Key))
                return false;
            UsersMessages.Add(User_Key, new List<Message>());
            return true;
        }
        public void UpdateMessage(string user_Key,int message_ID, string newValue)
        {
            if (!UsersMessages.ContainsKey(user_Key))
                return;
            try
            {
                UsersMessages[user_Key].FirstOrDefault(m => m.Message_ID == message_ID).Text = newValue;
            }
            catch (Exception)
            {
                return;
            }
        }
        public void DeleteMessage(string user_Key, int Message_ID)
        {
            UsersMessages[user_Key].Remove(UsersMessages[user_Key].FirstOrDefault(x => x.Message_ID == Message_ID));
        }
    }
}
