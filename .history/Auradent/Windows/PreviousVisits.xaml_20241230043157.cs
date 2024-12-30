using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Auradent.core;
using Auradent.Data;

namespace Auradent.Windows
{
    public partial class PreviousVisits : Window
    {
        private ObservableCollection<VisitRecord> _visits;
        private readonly string _patientName;
        private readonly IdataHelper<Medical_Record> _medicalRecordHelper;

        public PreviousVisits(string patientName)
        {
            InitializeComponent();
            _patientName = patientName;
            PatientNameText.Text = $"- {patientName}";

            var services = ((App)Application.Current).Services;
            _medicalRecordHelper = services.GetService<IdataHelper<Medical_Record>>();

            InitializeVisits();
            LoadVisitHistory();
        }

        private void InitializeVisits()
        {
            _visits = new ObservableCollection<VisitRecord>();
            VisitsListView.ItemsSource = _visits;
        }

        private void LoadVisitHistory()
        {
            var records = _medicalRecordHelper.GetAllData()
                .Where(r => !string.IsNullOrEmpty(r.Subjective) ||
                           !string.IsNullOrEmpty(r.objective) ||
                           !string.IsNullOrEmpty(r.Assessment) ||
                           !string.IsNullOrEmpty(r.TreatmentPlan))
                .OrderByDescending(r => r.RecordDate)
                .ToList();

            foreach (var record in records)
            {
                var visit = new VisitRecord
                {
                    Date = record.RecordDate.ToString("MMM dd, yyyy HH:mm"),
                    Type = "SOAP Record",
                    Subjective = record.Subjective,
                    Objective = record.objective,
                    Assessment = record.Assessment,
                    Plan = record.TreatmentPlan
                };
                _visits.Add(visit);
            }

            if (_visits.Any())
            {
                VisitsListView.SelectedItem = _visits.First();
            }
        }

        public void AddVisit(string subjective, string objective, string assessment, string plan)
        {
            var visit = new VisitRecord
            {
                Date = DateTime.Now.ToString("MMM dd, yyyy HH:mm"),
                Type = "SOAP Record",
                Subjective = subjective,
                Objective = objective,
                Assessment = assessment,
                Plan = plan
            };

            _visits.Insert(0, visit);
            VisitsListView.SelectedItem = visit;
        }

        private void VisitsListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (VisitsListView.SelectedItem is VisitRecord visit)
            {
                VisitDateText.Text = visit.Date;
                VisitTypeText.Text = visit.Type;
                SubjectiveText.Text = visit.Subjective;
                ObjectiveText.Text = visit.Objective;
                AssessmentText.Text = visit.Assessment;
                PlanText.Text = visit.Plan;
            }
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
    }

    public class VisitRecord
    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plan { get; set; }
    }
}
