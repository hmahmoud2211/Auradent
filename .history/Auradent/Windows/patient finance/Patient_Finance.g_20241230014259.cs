namespace Auradent.Windows.patient_finance
{
    public partial class Patient_Finance
    {
        private System.Windows.Controls.DataGrid TransactionsDataGrid;
        private System.Windows.Controls.TextBlock TotalPaidText;
        private System.Windows.Controls.TextBlock OutstandingText;
        private System.Windows.Controls.TextBlock TotalCostText;
        private System.Windows.Controls.TextBlock CashPaymentsText;
        private System.Windows.Controls.TextBlock CardPaymentsText;
        private System.Windows.Controls.TextBlock InsurancePaymentsText;
        private System.Windows.Controls.TextBlock PatientNameText;
        private System.Windows.Controls.TextBlock PatientIdText;

        private void InitializeComponent()
        {
            if (_contentLoaded) return;
            _contentLoaded = true;

            System.Uri resourceLocater = new System.Uri("/Auradent;component/Windows/patient%20finance/Patient_Finance.xaml", System.UriKind.Relative);
            System.Windows.Application.LoadComponent(this, resourceLocater);

            TransactionsDataGrid = ((System.Windows.Controls.DataGrid)(this.FindName("TransactionsDataGrid")));
            TotalPaidText = ((System.Windows.Controls.TextBlock)(this.FindName("TotalPaidText")));
            OutstandingText = ((System.Windows.Controls.TextBlock)(this.FindName("OutstandingText")));
            TotalCostText = ((System.Windows.Controls.TextBlock)(this.FindName("TotalCostText")));
            CashPaymentsText = ((System.Windows.Controls.TextBlock)(this.FindName("CashPaymentsText")));
            CardPaymentsText = ((System.Windows.Controls.TextBlock)(this.FindName("CardPaymentsText")));
            InsurancePaymentsText = ((System.Windows.Controls.TextBlock)(this.FindName("InsurancePaymentsText")));
            PatientNameText = ((System.Windows.Controls.TextBlock)(this.FindName("PatientNameText")));
            PatientIdText = ((System.Windows.Controls.TextBlock)(this.FindName("PatientIdText")));
        }

        private bool _contentLoaded;
    }
}