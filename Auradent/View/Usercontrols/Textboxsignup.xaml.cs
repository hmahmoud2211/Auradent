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
    /// Interaction logic for Textboxsignup.xaml
    /// </summary>
    public partial class Textboxsignup : UserControl
    {
        public string Textcontent
        {
            get { return (string)GetValue(TextcontentProperty); }
            set { SetValue(TextcontentProperty, value); }
        }

        public static readonly DependencyProperty TextcontentProperty =
            DependencyProperty.Register("Textcontent", typeof(string), typeof(Textboxsignup), new PropertyMetadata(string.Empty));

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
            DependencyProperty.Register("Placeholder", typeof(string), typeof(Textboxsignup), new PropertyMetadata(string.Empty));

        public Textboxsignup()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Txt_sign_up_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Txt_sign_up.Text))
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

