using System.Windows;
using System.Windows.Input;

namespace Auradent.Windows
{
    public partial class PreviousVisits : Window
    {
        public string PatientName { get; set; }

        public PreviousVisits(string patientName)
        {
            InitializeComponent();
            PatientName = patientName;
            PatientNameText.Text = patientName;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void VisitsListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Handle selection change if needed
        }
    }

    public class VisitRecord
    {
        public System.DateTime VisitDate { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plan { get; set; }
    }
}
