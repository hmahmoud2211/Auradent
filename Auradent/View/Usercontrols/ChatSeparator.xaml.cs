using System.Windows;
using System.Windows.Controls;

namespace Auradent.View.Usercontrols
{

    public partial class ChatSeparator : UserControl
    {
        public ChatSeparator()
        {
            InitializeComponent();
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ChatSeparator));
    }
}