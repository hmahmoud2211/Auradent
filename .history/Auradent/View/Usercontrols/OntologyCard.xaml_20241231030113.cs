using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Auradent.View.Usercontrols
{
    public partial class OntologyCard : UserControl
    {
        public OntologyCard()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(OntologyCard), new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            nameof(Id), typeof(string), typeof(OntologyCard), new PropertyMetadata(string.Empty));

        public string Id
        {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public static readonly DependencyProperty DefinitionProperty = DependencyProperty.Register(
            nameof(Definition), typeof(string), typeof(OntologyCard), new PropertyMetadata(string.Empty));

        public string Definition
        {
            get => (string)GetValue(DefinitionProperty);
            set => SetValue(DefinitionProperty, value);
        }

        public static readonly DependencyProperty SynonymsProperty = DependencyProperty.Register(
            nameof(Synonyms), typeof(List<string>), typeof(OntologyCard), new PropertyMetadata(null));

        public List<string> Synonyms
        {
            get => (List<string>)GetValue(SynonymsProperty);
            set => SetValue(SynonymsProperty, value);
        }

        public static readonly DependencyProperty LastUpdatedProperty = DependencyProperty.Register(
            nameof(LastUpdated), typeof(DateTime), typeof(OntologyCard), new PropertyMetadata(DateTime.Now));

        public DateTime LastUpdated
        {
            get => (DateTime)GetValue(LastUpdatedProperty);
            set => SetValue(LastUpdatedProperty, value);
        }

        public static readonly DependencyProperty DetailsUrlProperty = DependencyProperty.Register(
            nameof(DetailsUrl), typeof(string), typeof(OntologyCard), new PropertyMetadata(string.Empty));

        public string DetailsUrl
        {
            get => (string)GetValue(DetailsUrlProperty);
            set => SetValue(DetailsUrlProperty, value);
        }

        private void ViewDetails_Click(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(DetailsUrl))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = DetailsUrl,
                    UseShellExecute = true
                });
            }
        }
    }
} 