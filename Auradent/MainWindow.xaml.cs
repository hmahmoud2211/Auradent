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
using Microsoft.Extensions.DependencyInjection;

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
            var services = ((App)Application.Current).Services;
            if (services == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            dataHelperEmployee = services.GetService<IdataHelper<DoctorandNurse>>() ?? throw new InvalidOperationException("Data helper service is not available.");
            DoctorandNurse login_data = new DoctorandNurse
            {
                ID = 1,
                Username = "doctor",
                Password = "123",
                Role = "doctor",
                Nationa_ID = "30309290113038"
            };
            if (!dataHelperEmployee.CheckIfIdExists(1))
                dataHelperEmployee.Add(login_data);
            DoctorandNurse login_data_nurse = new DoctorandNurse
            {
                ID = 2,
                Username = "nurse",
                Password = "123",
                Role = "nurse",
                Nationa_ID = "30309290113037"
            };
            if (!dataHelperEmployee.CheckIfIdExists(2))
                dataHelperEmployee.Add(login_data_nurse);
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

            if (user != null && enteredUsername == "doctor")
            {

                newdoctordashboard newdoctordashboard = new newdoctordashboard
                {
                    Title = "Doctor Dashboard",
                    WindowState = WindowState.Maximized
                };
                newdoctordashboard.Show();
                this.Close();
            }
            else
            {
                if (user != null && enteredUsername == "nurse")
                {
                    Nurse_s_dashboard nurse_S_Dashboard = new Nurse_s_dashboard
                    {
                        Title = "Nurse's Dashboard",
                        WindowState = WindowState.Maximized
                    };
                    nurse_S_Dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Signuppage();
        }
    }
}