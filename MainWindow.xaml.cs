using MessangerServer;
using MessangerServer.Database_Models;
using MessangerServer.DBContext;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessangerServer.Models;
using MessangerServer.Status;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace MessangerServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ServerProcess ServerWork;
        public MainWindow()
        {
            ServerWork = new ServerProcess();
            InitializeComponent();
            this.DataContext = ServerWork;
        }

        private void Close_Server(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServerWork.Dispose();
        }

        private void Close_Server(object sender, RoutedEventArgs e)
        {
            ServerWork.Dispose();
            Application.Current.Shutdown();
        }

        private void BanPerson(object sender, RoutedEventArgs e)
        {
            ServerWork.BanPerson(ClientList.SelectedItem as User ?? null);
        }

        private void RemovePerson(object sender, RoutedEventArgs e)
        {
            if (ClientList.SelectedItem is User selectedUser)
                ServerWork.RemovePerson(selectedUser);
        } 
    }
}
