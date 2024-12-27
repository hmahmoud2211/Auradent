using System.Windows;
using System.Windows.Input;

namespace Auradent.Windows
{

    public partial class NewPatient : Window
    {
        public NewPatient()
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

        private void close_btn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}