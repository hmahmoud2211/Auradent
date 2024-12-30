using System;
using System.Windows;
using System.Windows.Input;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Windows.patient_finance
{
    public partial class AddTransaction : Window
    {
        private readonly IdataHelper<Finance> _financeHelper;
        private readonly IdataHelper<Appointment> _appointmentHelper;
        private readonly Patient _currentPatient;

        public AddTransaction(Patient patient)
        {
            InitializeComponent();
            _currentPatient = patient;
            var services = ((App)Application.Current).Services;
            _financeHelper = services.GetService<IdataHelper<Finance>>();
            _appointmentHelper = services.GetService<IdataHelper<Appointment>>();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                // Create new finance record
                var finance = new Finance
                {
                    TotalAmount = decimal.Parse(AmountTextBox.Text),
                    PaymentMethod = ((System.Windows.Controls.ComboBoxItem)PaymentMethodComboBox.SelectedItem).Content.ToString().ToLower(),
                    TypeOfPayment = ((System.Windows.Controls.ComboBoxItem)PaymentStatusComboBox.SelectedItem).Content.ToString().ToLower()
                };

                // Save finance record
                if (_financeHelper.Add(finance) == 1)
                {
                    // Create new appointment record to link finance with patient
                    var appointment = new Appointment
                    {
                        PatientID_FK = _currentPatient.PatientID,
                        Fainance_Fk = finance.FinanceId
                    };

                    _appointmentHelper.Add(appointment);

                    MessageBox.Show("Transaction added successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to add transaction.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding transaction: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(AmountTextBox.Text))
            {
                MessageBox.Show("Please enter an amount.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid amount.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (PaymentMethodComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (PaymentStatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment status.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
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
            this.DialogResult = false;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
