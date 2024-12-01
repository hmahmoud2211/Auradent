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
using MySql.Data.MySqlClient;

namespace Auradent.pages
{
    /// <summary>
    /// Interaction logic for Signuppage.xaml
    /// </summary>
    public partial class Signuppage : Page
    {
        private readonly string connectionString = "Server=localhost;Database=Auradent;Uid=root;Pwd=Hazem@2003;";
        public Signuppage()
        {
            InitializeComponent();
        }

        private void Textboxsignup_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Textboxsignup_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
           
            {
                try
                {
                    // Parse user input
                    int enteredUserid = int.Parse(ID_txt.Textcontent); // Use Text for TextBox
                    long enteredUserNationalID = long.Parse(National_Id_txt.Textcontent); // Use Text for TextBox
                    string enteredPassword = new_pass_txt.PasswordContent; // Use Password for PasswordBox

                    // Connect to the database
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if the user exists
                        string query = "SELECT COUNT(*) FROM Security WHERE Doctor_ID = @Userid AND National_ID = @UsernationalID";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Userid", enteredUserid);
                        command.Parameters.AddWithValue("@UsernationalID", enteredUserNationalID);

                        int userExists = Convert.ToInt32(command.ExecuteScalar());

                        if (userExists > 0)
                        {
                            // Update the user's password
                            string query_2 = "UPDATE Security SET Password = @newPassword WHERE National_ID = @UsernationalID";
                            MySqlCommand command2 = new MySqlCommand(query_2, connection);
                            command2.Parameters.AddWithValue("@newPassword", enteredPassword);
                            command2.Parameters.AddWithValue("@UsernationalID", enteredUserNationalID);

                            int update = command2.ExecuteNonQuery();

                            if (update > 0)
                            {
                                MessageBox.Show("Password has been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                                // Navigate to MainWindow
                                MainWindow newMainWindow = new MainWindow
                                {
                                    Title = "New Window",
                                    WindowState = WindowState.Maximized // Open maximized
                                };
                                newMainWindow.Show();

                                // Close the current window
                                Window.GetWindow(this)?.Close();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update the password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid User ID or National ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter valid numeric values for User ID and National ID.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
    }
}
