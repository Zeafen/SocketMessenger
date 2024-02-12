using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Primary_Massager.Models
{
    public class UserModel : INotifyPropertyChanged
    {

        public UserModel() { }

        private string _name { get; set; } = string.Empty;
        public string Name { get => _name; set
            {
                if (value == null || value == _name) return;
                _name = value;  
                OnPropertyChanged();
            }
        }
        private string _login { get; set; } = string.Empty;
        public string Login
        {
            get => _login; set
            {
                if (value == null || value == _login) return;
                _login = value;
                OnPropertyChanged();
            }
        }
        private string _password { get; set; } = string.Empty;
        public string Password
        {
            get => _password; set
            {
                if (value == null || value == _password) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _serverIP { get; set; } = string.Empty;
        public string Server_IP { get => _serverIP; set
            {
                if (value == null || value == _serverIP) return;
                _serverIP = value;
                OnPropertyChanged();
            } 
        }

        private string _port { get; set; } = string.Empty;
        public string Port { get => _port; set
            {
                if (value == _port) return;
                _port = value;
                OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
