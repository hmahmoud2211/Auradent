using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Windows
{
    public partial class PreviousVisits : Window
    {
        private readonly IdataHelper<Visit_Record> _visitHelper;
        private readonly Patient _currentPatient;
        private ObservableCollection<Visit_Record> _visits;

        public PreviousVisits(Patient patient)
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _visitHelper = services.GetService<IdataHelper<Visit_Record>>();
            _currentPatient = patient;

            PatientNameText.Text = $"- {patient.PatientName}";
            LoadVisits();
        }

        private void LoadVisits()
        {
            try
            {
                var visits = _visitHelper.GetAllData()
                    .Where(v => v.PatientID_FK == _currentPatient.PatientID)
                    .OrderByDescending(v => v.VisitDate)
                    .ToList();

                _visits = new ObservableCollection<Visit_Record>(visits);
                VisitsListView.ItemsSource = _visits;

                if (visits.Any())
                {
                    VisitsListView.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading visits: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VisitsListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (VisitsListView.SelectedItem is Visit_Record visit)
            {
                VisitDateText.Text = visit.VisitDate.ToString("MMMM dd, yyyy");
                SubjectiveText.Text = visit.Subjective;
                ObjectiveText.Text = visit.Objective;
                AssessmentText.Text = visit.Assessment;
                PlanText.Text = visit.Plan;
                NotesText.Text = visit.Notes;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ?
                WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}