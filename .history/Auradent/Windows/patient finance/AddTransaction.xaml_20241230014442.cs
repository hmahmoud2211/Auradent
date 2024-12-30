using System;
using System.Windows;
using System.Windows.Controls;
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
        private Action _onTransactionAdded;

        public AddTransaction(Patient patient, Action onTransactionAdded)
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _financeHelper = services.GetService<IdataHelper<Finance>>();
            _appointmentHelper = services.GetService<IdataHelper<Appointment>>();
            _currentPatient = patient;
            _onTransactionAdded = onTransactionAdded;

            // Set default values
            PaymentMethodComboBox.SelectedIndex = 0;
            PaymentStatusComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
                {
                    MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var paymentMethod = ((ComboBoxItem)PaymentMethodComboBox.SelectedItem).Content.ToString().ToLower();
                var paymentStatus = ((ComboBoxItem)PaymentStatusComboBox.SelectedItem).Content.ToString().ToLower();

                // Get new IDs
                int financeId = _financeHelper.GetAllData().Count > 0
                    ? _financeHelper.GetAllData().Max(f => f.FinanceId) + 1
                    : 1;

                int appointmentId = _appointmentHelper.GetAllData().Count > 0
                    ? _appointmentHelper.GetAllData().Max(a => a.AppointmentID) + 1
                    : 1;

                // Create new finance record
                var finance = new Finance
                {
                    FinanceId = financeId,
                    TotalAmount = amount,
                    PaymentMethod = paymentMethod,
                    TypeOfPayment = paymentStatus
                };

                // Create new appointment for this transaction
                var appointment = new Appointment
                {
                    AppointmentID = appointmentId,
                    PatientID_FK = _currentPatient.PatientID,
                    Fainance_Fk = financeId
                };

                // Save to database
                if (!_financeHelper.CheckIfIdExists(finance.FinanceId))
                {
                    int result = _financeHelper.Add(finance);
                    if (result == 0)
                    {
                        MessageBox.Show("Error saving finance record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (!_appointmentHelper.CheckIfIdExists(appointment.AppointmentID))
                {
                    int result = _appointmentHelper.Add(appointment);
                    if (result == 0)
                    {
                        MessageBox.Show("Error saving appointment record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    _onTransactionAdded?.Invoke();
                    Close();
                }
                else
                {
                    MessageBox.Show("Error: Appointment ID already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding transaction: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}