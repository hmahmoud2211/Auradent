using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontAwesome.WPF;
namespace Auradent.View.Usercontrols
{
    public partial class Item : UserControl
    {
        public Item()
        {
            InitializeComponent();
        }

        // Title Dependency Property
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(Item),
            new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Time Dependency Property
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            nameof(Time),
            typeof(string),
            typeof(Item),
            new PropertyMetadata(string.Empty));

        public string Time
        {
            get => (string)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        // Color Dependency Property
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color),
            typeof(string),
            typeof(Item),
            new PropertyMetadata("#000000"));

        public string Color
        {
            get => (string)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        // Icon Dependency Property
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(FontAwesomeIcon),
            typeof(Item),
            new PropertyMetadata(FontAwesomeIcon.None));

        public FontAwesomeIcon Icon
        {
            get => (FontAwesomeIcon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // IconBell Dependency Property
        public static readonly DependencyProperty IconBellProperty = DependencyProperty.Register(
            nameof(IconBell),
            typeof(FontAwesomeIcon),
            typeof(Item),
            new PropertyMetadata(FontAwesomeIcon.None));

        public FontAwesomeIcon IconBell
        {
            get => (FontAwesomeIcon)GetValue(IconBellProperty);
            set => SetValue(IconBellProperty, value);
        }

        // IsChecked Dependency Property
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            nameof(IsChecked),
            typeof(bool),
            typeof(Item),
            new PropertyMetadata(false, OnIsCheckedChanged));

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as Item;
            if (item != null)
            {
                bool isChecked = (bool)e.NewValue;
                item.Icon = isChecked ? FontAwesomeIcon.CheckCircle : FontAwesomeIcon.CircleThin;
                item.Color = isChecked ? "#4CAF50" : "#f1f1f1";
            }
        }

        // AppointmentID Dependency Property
        public static readonly DependencyProperty AppointmentIDProperty = DependencyProperty.Register(
            nameof(AppointmentID),
            typeof(int),
            typeof(Item),
            new PropertyMetadata(0));

        public int AppointmentID
        {
            get => (int)GetValue(AppointmentIDProperty);
            set => SetValue(AppointmentIDProperty, value);
        }

        // CheckChanged Event
        public static readonly RoutedEvent CheckChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(CheckChanged),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(Item));

        public event RoutedEventHandler CheckChanged
        {
            add { AddHandler(CheckChangedEvent, value); }
            remove { RemoveHandler(CheckChangedEvent, value); }
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            IsChecked = !IsChecked;
            RaiseEvent(new RoutedEventArgs(CheckChangedEvent, this));
        }
    }
}