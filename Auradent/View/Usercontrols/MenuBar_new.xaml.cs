using Auradent.Windows;
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

namespace Auradent.View.Usercontrols
{
    /// <summary>
    /// Interaction logic for MenuBar_new.xaml
    /// </summary>
    public partial class MenuBar_new : UserControl
    {
        public MenuBar_new()
        {
            InitializeComponent();
        }

        private void Log_out_btn(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow
            {
                WindowState = WindowState.Maximized,
                Title = "Auradent"
            };
            mainWindow.Show();
            Window.GetWindow(this)?.Close();
        }

        private void Calander_btn(object sender, RoutedEventArgs e)
        {
            Nurse_s_dashboard nurse_S_Dashboard = new Nurse_s_dashboard
            {
                WindowState = WindowState.Normal,
                Title = "Calnder"
            };
            nurse_S_Dashboard.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
