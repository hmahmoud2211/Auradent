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
using Auradent.core;
using Auradent.Data;
using MySql.Data.MySqlClient;

namespace Auradent.pages
{
    /// <summary>
    /// Interaction logic for Signuppage.xaml
    /// </summary>
    public partial class Signuppage : Page
    {
        private IdataHelper<DoctorandNurse> dataHelperEmployee;

        public Signuppage()
        {
            InitializeComponent();
            dataHelperEmployee = new DoctorandNurseEF();
        }

        private void Textboxsignup_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Textboxsignup_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Signup_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve user input
                int enteredUserid = int.Parse(ID_txt.Textcontent); // Use Text for TextBox
                string enteredUserNationalID = National_Id_txt.Textcontent; // Use Text for TextBox
                string enteredPassword = new_pass_txt.PasswordContent;


                var user = dataHelperEmployee.GetAllData().FirstOrDefault(u => u.ID == enteredUserid && u.Nationa_ID == enteredUserNationalID);
                
                if (user != null)
                {
                    // update the password with the new password
                    dataHelperEmployee.Update(new DoctorandNurse
                    {
                        ID = user.ID,
                        Username = user.Username,
                        Password = enteredPassword,
                        Role = user.Role,
                        Nationa_ID = user.Nationa_ID,
                        Appointments = user.Appointments,
                        Prescriptions = user.Prescriptions
                    });
                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow newmainWindow = new MainWindow
                    {
                        Title = "Finance",
                        WindowState = WindowState.Maximized
                    };

                    // Show the new window
                    newmainWindow.Show();
                    Window.GetWindow(this)?.Close();
                }
                else
                {
                    MessageBox.Show("Invalid ID or National ID", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);

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
