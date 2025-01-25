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
using FontAwesome.WPF;
using Microsoft.Extensions.DependencyInjection;
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
            var services = ((App)Application.Current).Services;
            if (services == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            dataHelperEmployee = services.GetService<IdataHelper<DoctorandNurse>>() ?? throw new InvalidOperationException("Data helper service is not available.");
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
                int enteredUserid = int.Parse(ID_txt.Textcontent); // Use Textcontent
                string enteredUserNationalID = National_Id_txt.Textcontent; // Use Textcontent
                string enteredPassword = new_pass_txt.Password;

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
                        Title = "Login Page",
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
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            MainWindow newmainWindow = new MainWindow
            {
                Title = "Finance",
                WindowState = WindowState.Maximized
            };

            // Show the new window
            newmainWindow.Show();
            Window.GetWindow(this)?.Close();
        }

        private void new_pass_txt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string password = new_pass_txt.Password;
            PasswordCriteriaPanel.Visibility = Visibility.Visible;

            // Check length
            bool lengthValid = password.Length >= 8;
            UpdateCriteriaStatus(lengthValid, LengthIcon, LengthText, "At least 8 characters");

            // Check uppercase
            bool hasUpper = password.Any(char.IsUpper);
            UpdateCriteriaStatus(hasUpper, UpperIcon, UpperText, "One uppercase letter");

            // Check lowercase
            bool hasLower = password.Any(char.IsLower);
            UpdateCriteriaStatus(hasLower, LowerIcon, LowerText, "One lowercase letter");

            // Check numbers
            bool hasNumber = password.Any(char.IsDigit);
            UpdateCriteriaStatus(hasNumber, NumberIcon, NumberText, "One number");

            // Check special characters
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));
            UpdateCriteriaStatus(hasSpecial, SpecialIcon, SpecialText, "One special character (!@#$%^&*)");

            // Enable/disable signup button based on all criteria being met
            Signup_btn.IsEnabled = lengthValid && hasUpper && hasLower && hasNumber && hasSpecial;
        }

        private void UpdateCriteriaStatus(bool isValid, ImageAwesome icon, TextBlock text, string criteriaText)
        {
            icon.Icon = isValid ? FontAwesomeIcon.Check : FontAwesomeIcon.Times;
            icon.Foreground = new SolidColorBrush(isValid ? Colors.LightGreen : Colors.Red);
            text.Foreground = new SolidColorBrush(isValid ? Colors.LightGreen : Colors.Red);
            text.Text = criteriaText;
        }
    }
}