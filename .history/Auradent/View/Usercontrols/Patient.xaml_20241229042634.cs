using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Auradent.View.Usercontrols
{
    public partial class Patient : UserControl
    {
        public Patient()
        {
            InitializeComponent();
        }

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(String), typeof(Patient));



        public String ID
        {
            get { return (String)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public static readonly DependencyProperty IDProperty = DependencyProperty.Register("ID", typeof(String), typeof(Patient));




        public String Case
        {
            get { return (String)GetValue(CaseProperty); }
            set { SetValue(CaseProperty, value); }
        }

        public static readonly DependencyProperty CaseProperty = DependencyProperty.Register("Case", typeof(String), typeof(Patient));



        public String Date
        {
            get { return (String)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(String), typeof(Patient));

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}