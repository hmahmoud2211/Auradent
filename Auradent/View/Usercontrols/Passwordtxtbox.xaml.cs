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

namespace Auradent.View.Usercontrols
{
    /// <summary>
    /// Interaction logic for Passwordtxtbox.xaml
    /// </summary>
    public partial class Passwordtxtbox : UserControl
    {
        public Passwordtxtbox()
        {
            InitializeComponent();
        }
        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }
            set { 
                placeholder = value;
                tbplaceholder.Text = placeholder;
            }
        }


        private void Txt_sign_up_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null && string.IsNullOrEmpty(passwordBox.Password))
            {
                tbplaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                tbplaceholder.Visibility = Visibility.Hidden;
            }
        }
    }

}
