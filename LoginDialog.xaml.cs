using Primary_Massager.Status;
using Primary_Massager.WorkingClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Primary_Massager
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public LoginVM LoginVM { get; set; }
        public LoginDialog()
        {
            LoginVM = new LoginVM();
            InitializeComponent();
        }

        private void SendRequest(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            if (LoginVM.SendRequest() != null)
            {
                this.Close();
                var mainVm = new MainWindow();
                mainVm.Show();
                return;
            }
            this.IsEnabled = true;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlgResult = MessageBox.Show("Are you sure you want to exit", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Error);
            switch (dlgResult)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    e.Cancel = true;
                    break;
            }
        }

        private void Exit_Login(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnRegistrationMode_Changed(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked ?? false)
                LoginField.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            else
                LoginField.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
        }
    }
}
