using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Auradent.View.Usercontrols
{
    /// <summary>
    /// Interaction logic for Teammates.xaml
    /// </summary>
    public partial class Teammates : UserControl
    {
        public Teammates()
        {
            InitializeComponent();
        }


        public string TeammatesTitle
        {
            get => (string)GetValue(TeammatesTitleProperty);
            set => SetValue(TeammatesTitleProperty, value);
        }
        public static readonly DependencyProperty TeammatesTitleProperty =
    DependencyProperty.Register("TeammatesTitle", typeof(string), typeof(Teammates));




        public string Num
        {
            get { return (string)GetValue(NumProperty); }
            set { SetValue(NumProperty, value); }
        }
        public static readonly DependencyProperty NumProperty = DependencyProperty.Register("Num", typeof(string), typeof(Teammates));


        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(Teammates));








    }
}