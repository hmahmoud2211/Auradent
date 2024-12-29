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
    /// <summary>
    /// Interaction logic for Patient_Finance.xaml
    /// </summary>
    public partial class Patient_Finance : Window
    {
        private readonly Patient _patient;
        private readonly IdataHelper<Finance> _financeHelper;
        private readonly IdataHelper<Appointment> _appointmentHelper;
        private ObservableCollection<FinanceViewModel> _transactions;
        public PlotModel GraphModel { get; private set; }

        public Patient_Finance(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
            var services = ((App)Application.Current).Services;
            _financeHelper = services.GetService<IdataHelper<Finance>>();
            _appointmentHelper = services.GetService<IdataHelper<Appointment>>();

            DataContext = this;
            InitializeGraph();
            LoadPatientData();
            LoadTransactions();
            UpdateSummary();
        }

        private void LoadPatientData()
        {
            if (_patient != null)
            {
                PatientNameText.Text = _patient.PatientName;
                PatientIdText.Text = _patient.PatientID.ToString();
            }
        }

        private void LoadTransactions()
        {
            _transactions = new ObservableCollection<FinanceViewModel>();
            
            // First get all appointments for this patient
            var patientAppointments = _appointmentHelper.GetAllData()
                .Where(a => a.PatientID == _patient.PatientID)
                .Select(a => a.AppointmentID)
                .ToList();

            // Then get all finance records for these appointments
            var finances = _financeHelper.GetAllData()
                .Where(f => patientAppointments.Contains(f.AppointmentID))
                .OrderByDescending(f => f.TransactionDate);

            foreach (var finance in finances)
            {
                _transactions.Add(new FinanceViewModel
                {
                    Month = finance.TransactionDate.ToString("MMM dd, yyyy"),
                    Sales = finance.Amount.ToString("C"),
                    Status = finance.Status,
                    RawAmount = finance.Amount,
                    PaymentMethod = finance.PaymentMethod
                });
            }

            TransactionsDataGrid.ItemsSource = _transactions;
            UpdatePaymentMethodStats();
            UpdateGraphData();
        }

        private void UpdateSummary()
        {
            if (_transactions != null)
            {
                decimal totalPaid = _transactions
                    .Where(t => t.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase))
                    .Sum(t => t.RawAmount);

                decimal totalCost = _transactions.Sum(t => t.RawAmount);
                decimal outstanding = totalCost - totalPaid;

                TotalPaidText.Text = totalPaid.ToString("C");
                TotalCostText.Text = totalCost.ToString("C");
                OutstandingText.Text = outstanding.ToString("C");
            }
        }

        private void UpdatePaymentMethodStats()
        {
            if (_transactions != null && _transactions.Any())
            {
                var total = _transactions.Count;
                var cashCount = _transactions.Count(t => t.PaymentMethod.Equals("Cash", StringComparison.OrdinalIgnoreCase));
                var cardCount = _transactions.Count(t => t.PaymentMethod.Equals("Card", StringComparison.OrdinalIgnoreCase));
                var insuranceCount = _transactions.Count(t => t.PaymentMethod.Equals("Insurance", StringComparison.OrdinalIgnoreCase));

                CashPaymentsText.Text = $"{(cashCount * 100.0 / total):F0}%";
                CardPaymentsText.Text = $"{(cardCount * 100.0 / total):F0}%";
                InsurancePaymentsText.Text = $"{(insuranceCount * 100.0 / total):F0}%";
            }
        }

        private void InitializeGraph()
        {
            GraphModel = new PlotModel
            {
                PlotAreaBorderColor = OxyColors.Transparent,
                TextColor = OxyColor.Parse("#E4E6EB"),
                DefaultFontSize = 12
            };

            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MMM yyyy",
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.None,
                TicklineColor = OxyColor.Parse("#7B5CD6"),
                TextColor = OxyColor.Parse("#E4E6EB")
            };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.None,
                TicklineColor = OxyColor.Parse("#7B5CD6"),
                TextColor = OxyColor.Parse("#E4E6EB")
            };

            GraphModel.Axes.Add(dateAxis);
            GraphModel.Axes.Add(valueAxis);
        }

        private void UpdateGraphData()
        {
            var lineSeries = new LineSeries
            {
                Color = OxyColor.Parse("#7B5CD6"),
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColor.Parse("#7B5CD6"),
                MarkerFill = OxyColor.Parse("#7B5CD6")
            };

            foreach (var transaction in _transactions.OrderBy(t => DateTime.Parse(t.Month)))
            {
                var date = DateTime.Parse(transaction.Month);
                var amount = decimal.Parse(transaction.Sales.TrimStart('$'));
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), (double)amount));
            }

            GraphModel.Series.Clear();
            GraphModel.Series.Add(lineSeries);
            GraphModel.InvalidatePlot(true);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Add Transaction dialog
            MessageBox.Show("Add Transaction functionality will be implemented soon.", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Handle transaction selection if needed
        }
    }

    public class FinanceViewModel
    {
        public string Month { get; set; }
        public string Sales { get; set; }
        public string Status { get; set; }
        public decimal RawAmount { get; set; }
        public string PaymentMethod { get; set; }
    }
}