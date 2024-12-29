using System;
using System.Windows;
using System.Windows.Controls;
using Auradent.core;

namespace Auradent.View.Usercontrols
{
    public partial class Patient : UserControl
    {
        public event EventHandler<core.Patient> ViewReportClicked;

        public Patient()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Patient), new PropertyMetadata(string.Empty));

        public string ID
        {
            get { return (string)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(string), typeof(Patient), new PropertyMetadata(string.Empty));

        public string Case
        {
            get { return (string)GetValue(CaseProperty); }
            set { SetValue(CaseProperty, value); }
        }

        public static readonly DependencyProperty CaseProperty =
            DependencyProperty.Register("Case", typeof(string), typeof(Patient), new PropertyMetadata(string.Empty));

        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(Patient), new PropertyMetadata(string.Empty));

        private void ViewReport_Click(object sender, RoutedEventArgs e)
        {
            if (this.Tag is core.Patient patient)
            {
                ViewReportClicked?.Invoke(this, patient);
            }
        }
    }
}