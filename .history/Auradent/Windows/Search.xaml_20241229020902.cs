using Microsoft.Ajax.Utilities;
using System;
using System.Collections.ObjectModel;
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
    public partial class Search : Window
    {
        public Search()
        {
            InitializeComponent();
            var converter = new BrushConverter();
            ObservableCollection<Member> members = new ObservableCollection<Member>();

            members.Add(new Member { Number = "1", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098AD"), Name = "John Doe", Position = "14", Email = "john.doe@gmail.com", Phone = "415-954-1475" });
            members.Add(new Member { Number = "2", Character = "R", BgColor = (Brush)converter.ConvertFromString("#1E88E5"), Name = "Reza Alavin", Position = "13", Email = "reza110@hotmail.com", Phone = "254-451-7893" });
            members.Add(new Member { Number = "3", Character = "D", BgColor = (Brush)converter.ConvertFromString("#FF8F00"), Name = "Dennis Castillo", Position = "15", Email = "deny.cast@gmail.com", Phone = "125-520-0141" });
            members.Add(new Member { Number = "4", Character = "G", BgColor = (Brush)converter.ConvertFromString("#FF5252"), Name = "Gabriel Cox", Position = "50", Email = "coxcox@gmail.com", Phone = "808-635-1221" });
            members.Add(new Member { Number = "5", Character = "L", BgColor = (Brush)converter.ConvertFromString("#0CA678"), Name = "Lena Jones", Position = "30", Email = "lena.offi@hotmail.com", Phone = "320-658-9174" });
            members.Add(new Member { Number = "6", Character = "B", BgColor = (Brush)converter.ConvertFromString("#6741D9"), Name = "Benjamin Caliword", Position = "12", Email = "beni12@hotmail.com", Phone = "114-203-6258" });
            members.Add(new Member { Number = "7", Character = "S", BgColor = (Brush)converter.ConvertFromString("#FF6D00"), Name = "Sophia Muris", Position = "10", Email = "sophi.muri@gmail.com", Phone = "852-233-6854" });
            members.Add(new Member { Number = "8", Character = "A", BgColor = (Brush)converter.ConvertFromString("#FF5252"), Name = "Ali Pormand", Position = "7", Email = "alipor@yahoo.com", Phone = "968-378-4849" });
            members.Add(new Member { Number = "9", Character = "F", BgColor = (Brush)converter.ConvertFromString("#1E88E5"), Name = "Frank Underwood", Position = "16", Email = "frank@yahoo.com", Phone = "301-584-6966" });
            members.Add(new Member { Number = "10", Character = "S", BgColor = (Brush)converter.ConvertFromString("#0CA678"), Name = "Saeed Dasman", Position = "5", Email = "saeed.dasi@hotmail.com", Phone = "817-320-5052" });

            members.Add(new Member { Number = "11", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098AD"), Name = "John Doe", Position = "1", Email = "john.doe@gmail.com", Phone = "415-954-1475" });
            members.Add(new Member { Number = "12", Character = "R", BgColor = (Brush)converter.ConvertFromString("#1E88E5"), Name = "Reza Alavi", Position = "2", Email = "reza110@hotmail.com", Phone = "254-451-7893" });
            members.Add(new Member { Number = "13", Character = "D", BgColor = (Brush)converter.ConvertFromString("#FF8F00"), Name = "Dennis Castillo", Position = "3", Email = "deny.cast@gmail.com", Phone = "125-520-0141" });
            members.Add(new Member { Number = "14", Character = "G", BgColor = (Brush)converter.ConvertFromString("#FF5252"), Name = "Gabriel Cox", Position = "33", Email = "coxcox@gmail.com", Phone = "808-635-1221" });
            members.Add(new Member { Number = "15", Character = "L", BgColor = (Brush)converter.ConvertFromString("#0CA678"), Name = "Lena Jones", Position = "55", Email = "lena.offi@hotmail.com", Phone = "320-658-9174" });
            members.Add(new Member { Number = "16", Character = "B", BgColor = (Brush)converter.ConvertFromString("#6741D9"), Name = "Benjamin Caliword", Position = "45", Email = "beni12@hotmail.com", Phone = "114-203-6258" });
            members.Add(new Member { Number = "17", Character = "S", BgColor = (Brush)converter.ConvertFromString("#FF6D00"), Name = "Sophia Muris", Position = "6", Email = "sophi.muri@gmail.com", Phone = "852-233-6854" });
            members.Add(new Member { Number = "18", Character = "A", BgColor = (Brush)converter.ConvertFromString("#FF5252"), Name = "Ali Pormand", Position = "8", Email = "alipor@yahoo.com", Phone = "968-378-4849" });
            members.Add(new Member { Number = "19", Character = "F", BgColor = (Brush)converter.ConvertFromString("#1E88E5"), Name = "Frank Underwood", Position = "9", Email = "frank@yahoo.com", Phone = "301-584-6966" });
            members.Add(new Member { Number = "20", Character = "S", BgColor = (Brush)converter.ConvertFromString("#0CA678"), Name = "Saeed Dasman", Position = "11", Email = "saeed.dasi@hotmail.com", Phone = "817-320-5052" });

            members.Add(new Member { Number = "21", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098AD"), Name = "John Doe", Position = "765", Email = "john.doe@gmail.com", Phone = "415-954-1475" });
            members.Add(new Member { Number = "22", Character = "R", BgColor = (Brush)converter.ConvertFromString("#1E88E5"), Name = "Reza Alavi", Position = "60", Email = "reza110@hotmail.com", Phone = "254-451-7893" });
            members.Add(new Member { Number = "23", Character = "D", BgColor = (Brush)converter.ConvertFromString("#FF8F00"), Name = "Dennis Castillo", Position = "188", Email = "deny.cast@gmail.com", Phone = "125-520-0141" });
            members.Add(new Member { Number = "24", Character = "G", BgColor = (Brush)converter.ConvertFromString("#FF5252"), Name = "Gabriel Cox", Position = "230", Email = "coxcox@gmail.com", Phone = "808-635-1221" });
            members.Add(new Member { Number = "25", Character = "L", BgColor = (Brush)converter.ConvertFromString("#0CA678"), Name = "Lena Jones", Position = "45", Email = "lena.offi@hotmail.com", Phone = "320-658-9174" });
            members.Add(new Member { Number = "26", Character = "B", BgColor = (Brush)converter.ConvertFromString("#6741D9"), Name = "Benjamin Caliword", Position = "456", Email = "beni12@hotmail.com", Phone = "114-203-6258" });
            members.Add(new Member { Number = "27", Character = "S", BgColor = (Brush)converter.ConvertFromString("#FF6D00"), Name = "Sophia Muris", Position = "343", Email = "sophi.muri@gmail.com", Phone = "852-233-6854" });
            members.Add(new Member { Number = "28", Character = "A", BgColor = (Brush)converter.ConvertFromString("#FF5252"), Name = "Ali Pormand", Position = "222", Email = "alipor@yahoo.com", Phone = "968-378-4849" });
            members.Add(new Member { Number = "29", Character = "F", BgColor = (Brush)converter.ConvertFromString("#1E88E5"), Name = "Frank Underwood", Position = "111", Email = "frank@yahoo.com", Phone = "301-584-6966" });
            members.Add(new Member { Number = "30", Character = "S", BgColor = (Brush)converter.ConvertFromString("#0CA678"), Name = "Saeed Dasman", Position = "100", Email = "saeed.dasi@hotmail.com", Phone = "817-320-5052" });

            membersDataGrid.ItemsSource = members;
        }
        private void PatientSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the selection logic here
            ComboBox comboBox = sender as ComboBox;

            if (comboBox != null && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedContent = selectedItem.Content.ToString();
                MessageBox.Show($"Selected: {selectedContent}", "Selection Changed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool isMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;
                    isMaximized = false;
                }
                else
                {

                    this.WindowState = WindowState.Maximized;
                    isMaximized = true;
                }
            }




        }

        private void home_btn(object sender, RoutedEventArgs e)
        {
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
    }
}