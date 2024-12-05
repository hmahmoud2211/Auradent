using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontAwesome.WPF;
namespace Auradent.UserControls
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
            typeof(SolidColorBrush),
            typeof(Item),
            new PropertyMetadata(Brushes.White));

        public SolidColorBrush Color
        {
            get => (SolidColorBrush)GetValue(ColorProperty);
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
    }
}