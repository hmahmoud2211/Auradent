using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Auradent.Windows.patient_finance
{
    /// <summary>
    /// Interaction logic for Patient_Finance.xaml
    /// </summary>
    public partial class Patient_Finance : Window
    {
        public ObservableCollection<Sale> PaymentData { get; set; }
        public PlotModel GraphModel { get; set; }

        public Patient_Finance()
        {
            InitializeComponent();

            // Initialize sample data for the DataGrid
            PaymentData = new ObservableCollection<Sale>
            {
                new Sale { Date = "Jun", Payment = 87 },
                new Sale { Date = "Jul", Payment = 79 },
                new Sale { Date = "Aug", Payment = 95 },
                new Sale { Date = "Sep", Payment = 25 },
                new Sale { Date = "Oct", Payment = 65 },
            };

            // Set the DataContext to enable data binding
            this.DataContext = this;

            // Initialize the PlotModel
            GraphModel = new PlotModel { Title = "Patient Finance Overview" };

            // Add a sample LineSeries to the PlotModel
            var lineSeries = new LineSeries { Title = "Payment Trend", MarkerType = MarkerType.Circle };

            // Dynamically add points from PaymentData
            for (int i = 0; i < PaymentData.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, PaymentData[i].Payment));
            }

            // Add the series to the PlotModel
            GraphModel.Series.Add(lineSeries);
        }
            
        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Sale selectedSale)
            {
                MessageBox.Show($"Selected: {selectedSale.Date}, Payment: {selectedSale.Payment}");
            }
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            // Add a new row to the PaymentData collection
            PaymentData.Add(new Sale { Date = "Nov", Payment = 100 });

            // Update the graph
            UpdateGraph();
        }

        private void UpdateGraph()
        {
            // Clear the current points and rebuild from PaymentData
            var lineSeries = GraphModel.Series[0] as LineSeries;
            if (lineSeries != null)
            {
                lineSeries.Points.Clear();
                for (int i = 0; i < PaymentData.Count; i++)
                {
                    lineSeries.Points.Add(new DataPoint(i, PaymentData[i].Payment));
                }
                GraphModel.InvalidatePlot(true); // Refresh the graph
            }
        }
    }

    /// <summary>
    /// Model class for Payment data
    /// </summary>
    public class Sale
    {
        public required string Date { get; set; }  // Ensure Date is never null
        public int Payment { get; set; }
    }
}
