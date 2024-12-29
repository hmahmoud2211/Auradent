using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using Auradent.Models;
using Auradent.Services;
using Auradent.Commands;

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
        // Get today's date
        var today = DateTime.Today;
        
        // Get all appointments for today, ordered by time descending
        var patients = await _patientService.GetTodaysPatients(today);
        
        // Take the last 3 patients (most recent)
        TodaysPatients = new ObservableCollection<Patient>(patients.Take(3));
    }

    private void ViewPatientDetails(Patient patient)
    {
        // Implement your patient details viewing logic here
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 