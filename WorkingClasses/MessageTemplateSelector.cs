using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Primary_Massager.Models;

namespace Primary_Massager.WorkingClasses
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate MessageTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Message mes)
            {
                return MessageTemplate;
            }
            return DefaultTemplate;
        }
    }
}
