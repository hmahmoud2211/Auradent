using Auradent.pages;
using Auradent.Windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MySql.Data.MySqlClient;
using Auradent.Data;
using Auradent.core;

namespace Auradent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IdataHelper<DoctorandNurse> dataHelperEmployee;

        public MainWindow()
        {
            InitializeComponent();
            dataHelperEmployee = new DoctorandNurseEF();
            DoctorandNurse login_data = new DoctorandNurse
            {
                ID = 1,
                Username = "hazem",
                Password = "123",
                Role = "doctor",
                Nationa_ID = "30309290113038"
            };
            if (!dataHelperEmployee.CheckIfIdExists(1))
                dataHelperEmployee.Add(login_data);
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
            //make a new instance of the login data and fill it with the vertual data not from the user


            // insert the login data in the database
            // check if the the username exist in the database


            string enteredUsername = Usr_name.Textcontent;
            string enteredPassword = Pass_txt_box.PasswordContent;
            var user = dataHelperEmployee.GetAllData().FirstOrDefault(u => u.Username == enteredUsername && u.Password == enteredPassword);

            if (user != null)
            {

                Window pageWindow = new Window
                {
                    Title = "Page Window",
                    WindowState = WindowState.Maximized
                };
                Frame frame = new Frame();
                frame.Navigate(new Uri("\\pages\\Dr_Dashboard_page.xaml", UriKind.Relative));
                pageWindow.Content = frame;
                pageWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Signuppage();
        }
    }
}