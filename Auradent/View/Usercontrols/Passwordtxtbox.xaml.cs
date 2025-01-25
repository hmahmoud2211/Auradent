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
        public event EventHandler<RoutedEventArgs> PasswordChanged;

        public string PasswordContent
        {
            get { return passwordBox.Password; }
            set { passwordBox.Password = value; }
        }

        public string Password
        {
            get { return passwordBox.Password; }
            set { passwordBox.Password = value; }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set 
            { 
                SetValue(PlaceholderProperty, value);
                tbplaceholder.Text = value;
            }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(Passwordtxtbox), new PropertyMetadata(string.Empty));

        public Passwordtxtbox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordBox != null && string.IsNullOrEmpty(passwordBox.Password))
            {
                tbplaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                tbplaceholder.Visibility = Visibility.Hidden;
            }

            PasswordChanged?.Invoke(this, e);
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
            if (PasswordTextBox.Visibility == Visibility.Visible)
            {
                PasswordTextBox.Text = passwordBox.Password;
            }
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Visibility == Visibility.Visible)
            {
                // Show password
                PasswordTextBox.Text = passwordBox.Password;
                passwordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                // Hide password
                passwordBox.Password = PasswordTextBox.Text;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                passwordBox.Visibility = Visibility.Visible;
            }
        }

        private void pass_txt_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Visibility == Visibility.Visible)
            {
                PasswordTextBox.Text = passwordBox.Password;
            }
        }
    }
}
