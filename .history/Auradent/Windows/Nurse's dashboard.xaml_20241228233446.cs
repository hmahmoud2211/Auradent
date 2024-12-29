using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for Nurse_s_dashboard.xaml
    /// </summary>
    public partial class Nurse_s_dashboard : Window
    {
        public Nurse_s_dashboard()
        {
            InitializeComponent();
            mainCalendar.DisplayDate = DateTime.Today;
            UpdateCurrentMonthYear();
        }

        private void UpdateCurrentMonthYear()
        {
            if (mainCalendar.DisplayDate != null)
            {
                currentMonthYear.Text = mainCalendar.DisplayDate.ToString("MM/yyyy");
                
                // Update month buttons
                foreach (var child in ((StackPanel)mainCalendar.Parent).Children)
                {
                    if (child is Button btn && btn.Tag != null && int.TryParse(btn.Tag.ToString(), out int btnMonth))
                    {
                        btn.Foreground = btnMonth == mainCalendar.DisplayDate.Month ? 
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F6F95")) : 
                            new SolidColorBrush(Colors.Black);
                    }
                }
            }
        }

        private void Month_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                if (int.TryParse(btn.Tag.ToString(), out int month))
                {
                    mainCalendar.DisplayDate = new DateTime(mainCalendar.DisplayDate.Year, month, 1);
                    UpdateCurrentMonthYear();
                }
            }
        }

        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            mainCalendar.DisplayDate = mainCalendar.DisplayDate.AddMonths(-1);
            UpdateCurrentMonthYear();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            mainCalendar.DisplayDate = mainCalendar.DisplayDate.AddMonths(1);
            UpdateCurrentMonthYear();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainCalendar.SelectedDate.HasValue)
            {
                UpdateCurrentMonthYear();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Item_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void close_btn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Back_btn(object sender, RoutedEventArgs e)
        {
            MainWindow secondWindow = new MainWindow();
            secondWindow.Show();
        }

        private void new_patient(object sender, RoutedEventArgs e)
        {
            NewPatient secondWindow = new NewPatient();
            secondWindow.Show();
        }

        private void Existing_btn(object sender, RoutedEventArgs e)
        {
            ExistPatient secondWindow = new ExistPatient();
            secondWindow.Show();
        }
    }
}