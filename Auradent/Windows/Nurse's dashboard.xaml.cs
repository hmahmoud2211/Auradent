using System.Windows;
using System.Windows.Input;

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
    }
}