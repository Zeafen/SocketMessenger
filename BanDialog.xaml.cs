using MessangerServer.Database_Models;
using MessangerServer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for BanDialog.xaml
    /// </summary>
    public partial class BanDialog : Window
    {
        User user_To_Ban;
        UserContext Context;
        public BanDialog(User guilty, UserContext ctx)
        {
            user_To_Ban = guilty;
            Context = ctx;
            InitializeComponent();
        }

        private void Send_Ban(object sender, RoutedEventArgs e)
        {
            if (BanDate.SelectedDate == null || BanDate.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Please, select the date", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
                Context.BanList.Add(new BanList(user_To_Ban, ReasonInput.Text, BanDate.SelectedDate??DateTime.Today));
            DialogResult = true;
        }
    }
}
