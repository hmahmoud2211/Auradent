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
        private readonly Patient _patient;
        private readonly IdataHelper<Finance> _financeHelper;
        private ObservableCollection<FinanceViewModel> _transactions;
        public PlotModel GraphModel { get; private set; }

        public Patient_Finance(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
            var services = ((App)Application.Current).Services;
            _financeHelper = services.GetService<IdataHelper<Finance>>();
            
            DataContext = this;
            InitializeGraphModel();
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
            var finances = _financeHelper.GetAllData()
                .Where(f => f.PatientID == _patient.PatientID)
                .OrderByDescending(f => f.TransactionDate);

            foreach (var finance in finances)
            {
                _transactions.Add(new FinanceViewModel
                {
                    Month = finance.TransactionDate.ToString("MMM dd, yyyy"),
                    Sales = $"${finance.Amount:N2}",
                    Status = finance.Status,
                    Amount = finance.Amount,
                    PaymentMethod = finance.PaymentMethod
                });
            }

            TransactionsDataGrid.ItemsSource = _transactions;
            UpdatePaymentMethodStats();
            UpdateGraphData();
        }

        private void UpdateSummary()
        {
            if (_transactions != null && _transactions.Any())
            {
                decimal totalPaid = _transactions
                    .Where(t => t.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase))
                    .Sum(t => t.Amount);

                decimal totalCost = _transactions.Sum(t => t.Amount);
                decimal outstanding = totalCost - totalPaid;

                TotalPaidText.Text = $"${totalPaid:N2}";
                OutstandingText.Text = $"${outstanding:N2}";
                TotalCostText.Text = $"${totalCost:N2}";
            }
        }

        private void UpdatePaymentMethodStats()
        {
            if (_transactions != null && _transactions.Any())
            {
                var totalTransactions = _transactions.Count;
                var cashCount = _transactions.Count(t => t.PaymentMethod.Equals("Cash", StringComparison.OrdinalIgnoreCase));
                var cardCount = _transactions.Count(t => t.PaymentMethod.Equals("Card", StringComparison.OrdinalIgnoreCase));
                var insuranceCount = _transactions.Count(t => t.PaymentMethod.Equals("Insurance", StringComparison.OrdinalIgnoreCase));

                CashPaymentsText.Text = $"{(cashCount * 100.0 / totalTransactions):N0}%";
                CardPaymentsText.Text = $"{(cardCount * 100.0 / totalTransactions):N0}%";
                InsurancePaymentsText.Text = $"{(insuranceCount * 100.0 / totalTransactions):N0}%";
            }
        }

        private void InitializeGraphModel()
        {
            GraphModel = new PlotModel
            {
                PlotAreaBorderColor = OxyColors.Transparent,
                TextColor = OxyColor.FromRgb(228, 230, 235),
                DefaultFontSize = 12
            };

            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MMM yyyy",
                IntervalType = DateTimeIntervalType.Months,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None,
                MajorGridlineColor = OxyColor.FromRgb(43, 45, 49),
                TicklineColor = OxyColor.FromRgb(43, 45, 49),
                Title = "Date"
            };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None,
                MajorGridlineColor = OxyColor.FromRgb(43, 45, 49),
                TicklineColor = OxyColor.FromRgb(43, 45, 49),
                Title = "Amount ($)"
            };

            GraphModel.Axes.Add(dateAxis);
            GraphModel.Axes.Add(valueAxis);
        }

        private void UpdateGraphData()
        {
            var lineSeries = new LineSeries
            {
                Color = OxyColor.FromRgb(123, 92, 214),
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColor.FromRgb(123, 92, 214),
                MarkerFill = OxyColor.FromRgb(123, 92, 214)
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
            MessageBox.Show("Add transaction functionality will be implemented here.");
        }
    }

    public class FinanceViewModel
    {
        public string Month { get; set; }
        public string Sales { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}