using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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

namespace MessangerServer
{
    /// <summary>
    /// Interaction logic for InitializeEP.xaml
    /// </summary>
    public partial class InitializeEP : Window
    {
        public InitializeEP()
        {
            InitializeComponent();
            HostSelection.ItemsSource = Dns.GetHostAddresses(Dns.GetHostName());
        }

        private void Start_Server(object sender, RoutedEventArgs e)
        {
            var address = HostSelection.SelectedItem as IPAddress;
            if (address == null)
            {
                MessageBox.Show("Please, select the host address.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(PortInput.Text, out var port))
            {
                MessageBox.Show("Incorrect port", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Application.Current.Resources.Add("EndPoint", new IPEndPoint(address, port));
            MainWindow Server = new MainWindow();
            Server.Show();
            this.Close();
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
