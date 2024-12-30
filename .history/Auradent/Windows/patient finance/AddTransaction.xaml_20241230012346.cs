using System;
using System.Windows;
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

                // Create new appointment for this transaction
                var appointment = new Appointment
                {
                    AppointmentID = _appointmentHelper.GetAllData().Count > 0
                        ? _appointmentHelper.GetAllData().Max(a => a.AppointmentID) + 1
                        : 1,
                    PatientID_FK = _currentPatient.PatientID,
                    Fainance_Fk = _financeHelper.GetAllData().Count > 0
                        ? _financeHelper.GetAllData().Max(f => f.FinanceId) + 1
                        : 1
                };

                // Create new finance record
                var finance = new Finance
                {
                    FinanceId = appointment.Fainance_Fk,
                    TotalAmount = amount,
                    PaymentMethod = paymentMethod,
                    TypeOfPayment = paymentStatus
                };

                // Save to database
                if (!_financeHelper.CheckIfIdExists(finance.FinanceId))
                {
                    _financeHelper.Add(finance);
                }

                if (!_appointmentHelper.CheckIfIdExists(appointment.AppointmentID))
                {
                    _appointmentHelper.Add(appointment);
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