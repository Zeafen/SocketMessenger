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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Primary_Massager.Models
{
    /// <summary>
    /// Interaction logic for MessageControl.xaml
    /// </summary>
    public partial class MessageControl : UserControl
    {
        public MessageControl()
        {
            MessageText = "Some Text to fill the void in your mind...";
            SendDate = DateTime.Now;
            IsEdited = false;
            InitializeComponent();
        }
        public MessageControl(string messageText, DateTime sendDate)
        {
            MessageText = messageText;
            SendDate = sendDate;

            InitializeComponent();
        }
        public MessageControl( string Sender_Name, int message_Id, string messageText, DateTime sendDate)
        {
            Message_Id = message_Id;
            SenderName = Sender_Name;
            MessageText = messageText;
            SendDate = sendDate;
            InitializeComponent();
        }
        public static readonly DependencyProperty MessageTextProperty = DependencyProperty.Register(
            "MessageText", typeof(string), typeof(MessageControl));
        public static readonly DependencyProperty DateSendProperty = DependencyProperty.Register(
            "DateSend", typeof(DateTime), typeof(MessageControl));
        public static readonly DependencyProperty IsEditedProperty = DependencyProperty.Register(
            "IsEdited", typeof(bool), typeof(MessageControl));

        public string MessageText
        {
            set
            {
                SetValue(MessageTextProperty, value);
            }
            get => (string)GetValue(MessageTextProperty);
        }

        public DateTime SendDate
        {
            set
            {
                SetValue(DateSendProperty, value);
            }
            get => (DateTime)GetValue(DateSendProperty);
        }

        public bool IsEdited
        {
            set
            {
                SetValue(IsEditedProperty, value);
            }
            get => (bool)GetValue(IsEditedProperty);
        }

        public int Message_Id { get; init; }
        public string SenderName {get; init;}

        public static implicit operator MessageControl(Message message)
        {
            return new MessageControl(message.SenderName, message.Message_ID, message.Text, message.SendDate);
        }
    }
}
