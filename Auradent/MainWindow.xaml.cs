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
        private IdataHelper<Employee> dataHelperEmployee;

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
            _=dataHelperEmployee.Add(login_data);
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
            var login_data = new Employee
            {
                Username = "hazem",
                Password = "123",
                Role = "Doctor",
                Nationa_ID = ""
            };
            // insert the login data in the database
            dataHelperEmployee.InsertData(login_data);
            string enteredUsername = Usr_name.Textcontent;
            string enteredPassword = Pass_txt_box.PasswordContent;

            // read from the database and check if the username = enteredUsername and password = enteredPassword
            // if true, then open the Dr_Dashboard_page 
            // else, show an error message
            if (enteredUsername == login_data.Username && enteredPassword == login_data.Password)
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
                MessageBox.Show("Invalid username or password");

            }
        }
        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Signuppage();
        }
    }
}
