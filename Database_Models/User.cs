using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MessangerServer.Database_Models
{
    public class User
    {
        public User() { }
        public User(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
            User_Key = Guid.NewGuid().ToString();
        }
        public User(string name, string login, string password, Stream user_Stream) : this(name, login, password)
        {        
            mesReader = new BinaryReader(user_Stream);
            mesWriter = new BinaryWriter(user_Stream);
        }

        public int Id { get; set; }

        [NotMapped]
        public string User_Key { init; get; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }

        [Required(AllowEmptyStrings  = false)]
        public string Password { get; set; }

        [NotMapped]
        public BinaryReader mesReader { get; set; }
        [NotMapped]
        public BinaryWriter mesWriter { get; set; }

        public void SetNewStream(Stream stream)
        {
            if (mesReader?.BaseStream == stream)
            {
                return;
            }
            mesReader = new BinaryReader(stream);
            mesWriter = new BinaryWriter(stream);
        }
    }
}
