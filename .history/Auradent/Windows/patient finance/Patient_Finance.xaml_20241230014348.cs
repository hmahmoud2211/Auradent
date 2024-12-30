using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Auradent.core;
using Auradent.Core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace Auradent.Windows.patient_finance
{
    /// <summary>
    /// Interaction logic for Patient_Finance.xaml
    /// </summary>
    public partial class Patient_Finance : AutoRefreshWindow
    {
        private readonly IdataHelper<Finance> _financeHelper;
        private readonly IdataHelper<Appointment> _appointmentHelper;
        private ObservableCollection<FinanceViewModel> _transactions;
        private Patient _currentPatient;
        private PlotModel _graphModel;

        public Patient_Finance()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _financeHelper = services.GetService<IdataHelper<Finance>>();
            _appointmentHelper = services.GetService<IdataHelper<Appointment>>();

            InitializeGraph();
            InitializeAutoRefresh(1); // 1 second refresh interval
        }

        protected override void RefreshData()
        {
            if (_currentPatient != null)
            {
                LoadFinanceData();
            }
        }

        private void LoadFinanceData()
        {
            try
            {
                // Get all appointments for the current patient
                var patientAppointments = _appointmentHelper.GetAllData()
                    .Where(a => a.PatientID_FK == _currentPatient?.PatientID)
                    .ToList();

                // Get finance records for these appointments
                var finances = _financeHelper.GetAllData()
                    .Where(f => patientAppointments.Any(a => a.Fainance_Fk == f.FinanceId))
                    .OrderByDescending(f => f.FinanceId)
                    .ToList();

                _transactions = new ObservableCollection<FinanceViewModel>(
                    finances.Select(f => new FinanceViewModel
                    {
                        Month = DateTime.Now.ToString("MMM dd, yyyy"),
                        Sales = $"${f.TotalAmount:N2}",
                        Status = f.TypeOfPayment
                    }));

                Dispatcher.Invoke(() =>
                {
                    TransactionsDataGrid.ItemsSource = _transactions;

                    // Calculate totals
                    decimal totalPaid = finances.Where(f => f.TypeOfPayment == "paid").Sum(f => f.TotalAmount);
                    decimal totalOutstanding = finances.Where(f => f.TypeOfPayment == "pending").Sum(f => f.TotalAmount);
                    decimal totalCost = finances.Sum(f => f.TotalAmount);

                    TotalPaidText.Text = $"${totalPaid:N2}";
                    OutstandingText.Text = $"${totalOutstanding:N2}";
                    TotalCostText.Text = $"${totalCost:N2}";

                    // Calculate payment methods distribution
                    var paymentMethods = finances.Where(f => f.TypeOfPayment == "paid")
                        .GroupBy(f => f.PaymentMethod)
                        .ToDictionary(g => g.Key, g => g.Sum(f => f.TotalAmount));

                    decimal totalPaidAmount = paymentMethods.Values.Sum();
                    if (totalPaidAmount > 0)
                    {
                        CashPaymentsText.Text = $"{(paymentMethods.GetValueOrDefault("cash", 0) / totalPaidAmount * 100):N0}%";
                        CardPaymentsText.Text = $"{(paymentMethods.GetValueOrDefault("card", 0) / totalPaidAmount * 100):N0}%";
                        InsurancePaymentsText.Text = $"{(paymentMethods.GetValueOrDefault("insurance", 0) / totalPaidAmount * 100):N0}%";
                    }

                    UpdateGraph(finances);
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Error loading finance data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        private void InitializeGraph()
        {
            _graphModel = new PlotModel
            {
                PlotAreaBorderColor = OxyColors.Transparent,
                TextColor = OxyColors.White,
                Background = OxyColors.Transparent
            };

            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MMM dd",
                AxislineColor = OxyColors.White,
                TextColor = OxyColors.White,
                TicklineColor = OxyColors.White
            };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Amount ($)",
                AxislineColor = OxyColors.White,
                TextColor = OxyColors.White,
                TicklineColor = OxyColors.White
            };

            _graphModel.Axes.Add(dateAxis);
            _graphModel.Axes.Add(valueAxis);

            this.DataContext = this;
        }

        private void UpdateGraph(System.Collections.Generic.List<Finance> finances)
        {
            _graphModel.Series.Clear();

            var lineSeries = new LineSeries
            {
                Color = OxyColor.FromRgb(123, 92, 214),
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColor.FromRgb(123, 92, 214),
                MarkerFill = OxyColor.FromRgb(123, 92, 214)
            };

            // Since Finance doesn't have a date field, we'll use the FinanceId to create a timeline
            int index = 0;
            foreach (var finance in finances.OrderBy(f => f.FinanceId))
            {
                lineSeries.Points.Add(new DataPoint(index++, (double)finance.TotalAmount));
            }

            _graphModel.Series.Add(lineSeries);
            _graphModel.InvalidatePlot(true);
        }

        public PlotModel GraphModel => _graphModel;

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

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Handle selection change if needed
        }

        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPatient == null)
            {
                MessageBox.Show("Please select a patient first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addTransactionWindow = new AddTransaction(_currentPatient, () => LoadFinanceData());
            addTransactionWindow.ShowDialog();
        }

        public void SetPatient(Patient patient)
        {
            _currentPatient = patient;
            if (patient != null)
            {
                PatientNameText.Text = patient.PatientName;
                PatientIdText.Text = patient.PatientID.ToString();
                LoadFinanceData();
            }
        }
    }

    public class FinanceViewModel
    {
        public string Month { get; set; }
        public string Sales { get; set; }
        public string Status { get; set; }
    }
}