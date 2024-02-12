using Primary_Massager.Models;
using Primary_Massager.Status;
using Primary_Massager.WorkingClasses;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Primary_Massager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ClientProcessor ClientWork { get; set; }
        public MainWindow()
        {
            ClientWork = new ClientProcessor();
            InitializeComponent();
        }
        private void SendRequest(object sender, RoutedEventArgs e)
        {
            ClientWork.SendRequest(InputText.Text);
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlgResult = MessageBox.Show("Are you sure you want to exit our nice messager?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(dlgResult == MessageBoxResult.Yes)
            {
                ClientWork.Dispose();
                e.Cancel = true;
            }
            e.Cancel = false;
        }


        private void MessageTextField_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ClientWork.SendEdit(MessageContainer.SelectedItem as Message, (sender as TextBox).Text);
                (MessageContainer.SelectedItem as Message).IsEditing = false;
            }
        }

        private void Exit_Account(object sender, RoutedEventArgs e)
        {
            ClientWork.Dispose();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}