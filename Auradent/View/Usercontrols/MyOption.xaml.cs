using System.Windows;
using System.Windows.Controls;

namespace Auradent.View.Usercontrols
{
    public partial class MyOption : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MyOption), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(MyOption), new PropertyMetadata(false));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public MyOption()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            IsSelected = true;
        }
    }
}
