using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using Auradent.Models;
using Auradent.Services;
using Auradent.Commands;

namespace Auradent.ViewModels
{
    public class NewDoctorDashboardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Patient> _todaysPatients;
        private readonly IPatientService _patientService;

        public ObservableCollection<Patient> TodaysPatients
        {
            get => _todaysPatients;
            set
            {
                _todaysPatients = value;
                OnPropertyChanged(nameof(TodaysPatients));
            }
        }

        public ICommand ViewPatientDetailsCommand { get; }

        public NewDoctorDashboardViewModel(IPatientService patientService)
        {
            _patientService = patientService;
            ViewPatientDetailsCommand = new RelayCommand<Patient>(ViewPatientDetails);
            LoadTodaysPatients();
        }

        private async void LoadTodaysPatients()
        {
            try
            {
                var today = DateTime.Today;
                var patients = await _patientService.GetTodaysPatients(today);
                TodaysPatients = new ObservableCollection<Patient>(patients.Take(3));
            }
            catch (Exception ex)
            {
                // Handle or log the error appropriately
                System.Diagnostics.Debug.WriteLine($"Error loading patients: {ex.Message}");
            }
        }

        private void ViewPatientDetails(Patient patient)
        {
            if (patient != null)
            {
                var integRatedPatient = new IntegRatedPatient
                {
                    Title = "Upcoming Patient",
                    WindowState = System.Windows.WindowState.Maximized
                };
                integRatedPatient.Show();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 