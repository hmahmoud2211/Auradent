using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for SOAP.xaml
    /// </summary>
    public partial class SOAP : Window
    {

        public class PlaceholderVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string text = value as string;
                return string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public SOAP()
        {
            InitializeComponent();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = WindowState.Normal;
            }
        }



        private void AssessmentNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AssessmentNotes.Text.Length == 0)
            {
                AssessmentPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                AssessmentPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        // Event handler for PlanNotes TextBox
        private void PlanNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PlanNotes.Text.Length == 0)
            {
                PlanPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                PlanPlaceholder.Visibility = Visibility.Collapsed;
            }
        }


        private void ComplaintComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComplaintComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                // Show the free-text input only when "Other" is selected
                if (selectedItem.Content.ToString() == "Other")
                {
                    OtherComplaintTextBox.Visibility = Visibility.Visible;
                }
                else
                {
                    OtherComplaintTextBox.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void InitialDiagnoseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if the selected item is "Other"
            if (InitialDiagnoseComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content.ToString() == "Other")
            {
                InitialDiagnoseTextBox.Visibility = Visibility.Visible; // Show free-text input
            }
            else
            {
                InitialDiagnoseTextBox.Visibility = Visibility.Collapsed; // Hide free-text input
            }
        }



    }
}