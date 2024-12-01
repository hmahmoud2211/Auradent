using Auradent.pages;
using Auradent.Windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MySql.Data.MySqlClient;

namespace Auradent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Connection string to the MySQL database
        private readonly string connectionString = "Server=localhost;Database=Auradent;Uid=root;Pwd=Hazem@2003;";

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
            string enteredUsername = Usr_name.Textcontent;
            string enteredPassword = Pass_txt_box.PasswordContent;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Query to check user credentials
                    string query = "SELECT * FROM Security WHERE User_name = @UserName AND Password = @Password";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserName", enteredUsername);
                    command.Parameters.AddWithValue("@Password", enteredPassword);

                    int userExists = Convert.ToInt32(command.ExecuteScalar());

                    if (userExists > 0)
                    {
                        // Navigate to the dashboard if the login is successful
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
                        MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Signuppage();
        }
    }
}
