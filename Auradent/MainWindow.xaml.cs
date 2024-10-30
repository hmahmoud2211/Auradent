using Auradent.pages;
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

namespace Auradent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Textboxsignup_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Textboxsignup_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

       

        

        private void Textboxsignup_Loaded_2(object sender, RoutedEventArgs e)
        {

        }

        private void Login_page_Click(object sender, RoutedEventArgs e)
        {
            int pass = 123;
            string name = "hazem";
            if (int.TryParse(Pass_txt_box.PasswordContent, out int passowrd))
            {
                if (passowrd == pass && name == Usr_name.Textcontent)
                {
                    MessageBox.Show("Verified user", "Welcome❤️", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Content = new Dashboard();
                }
                else
                {
                    MessageBox.Show("Declined user", "Get out!", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid format", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
               
            }
        }

        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Signuppage();
        }
    }
    

}