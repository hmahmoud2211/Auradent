using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace Auradent.Windows.patient_finance
{
    public partial class Patient_Finance : Window
    {
        private readonly IdataHelper<Finance> _financeHelper;
        private readonly IdataHelper<Patient> _patientHelper;
        private ObservableCollection<FinanceViewModel> _transactions;
        private Patient _currentPatient;
        private PlotModel _graphModel;

        public Patient_Finance()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _financeHelper = services.GetService<IdataHelper<Finance>>();
            _patientHelper = services.GetService<IdataHelper<Patient>>();
            
            InitializeGraph();
            LoadFinanceData();
        }

        private void LoadFinanceData()
        {
            try
            {
                var finances = _financeHelper.GetAllData()
                    .Where(f => f.PatientID_FK == _currentPatient?.PatientID)
                    .OrderByDescending(f => f.TransactionDate)
                    .ToList();

                _transactions = new ObservableCollection<FinanceViewModel>(
                    finances.Select(f => new FinanceViewModel
                    {
                        Month = f.TransactionDate.ToString("MMM dd, yyyy"),
                        Sales = $"${f.Amount:N2}",
                        Status = f.PaymentStatus
                    }));

                TransactionsDataGrid.ItemsSource = _transactions;

                // Calculate totals
                decimal totalPaid = finances.Where(f => f.PaymentStatus == "Paid").Sum(f => f.Amount);
                decimal totalOutstanding = finances.Where(f => f.PaymentStatus == "Pending").Sum(f => f.Amount);
                decimal totalCost = finances.Sum(f => f.Amount);

                TotalPaidText.Text = $"${totalPaid:N2}";
                OutstandingText.Text = $"${totalOutstanding:N2}";
                TotalCostText.Text = $"${totalCost:N2}";

                // Calculate payment methods distribution
                var paymentMethods = finances.Where(f => f.PaymentStatus == "Paid")
                    .GroupBy(f => f.PaymentMethod)
                    .ToDictionary(g => g.Key, g => g.Sum(f => f.Amount));

                decimal totalPaidAmount = paymentMethods.Values.Sum();
                if (totalPaidAmount > 0)
                {
                    CashPaymentsText.Text = $"{(paymentMethods.GetValueOrDefault("Cash", 0) / totalPaidAmount * 100):N0}%";
                    CardPaymentsText.Text = $"{(paymentMethods.GetValueOrDefault("Card", 0) / totalPaidAmount * 100):N0}%";
                    InsurancePaymentsText.Text = $"{(paymentMethods.GetValueOrDefault("Insurance", 0) / totalPaidAmount * 100):N0}%";
                }

                UpdateGraph(finances);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading finance data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            foreach (var finance in finances.OrderBy(f => f.TransactionDate))
            {
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(finance.TransactionDate), (double)finance.Amount));
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
            // TODO: Implement add transaction functionality
            MessageBox.Show("Add transaction functionality will be implemented here.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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