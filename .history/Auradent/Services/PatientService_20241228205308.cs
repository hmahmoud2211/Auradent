using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auradent.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Services
{
    public class PatientService : IPatientService
    {
        private readonly IdataHelper<Patient> _dataHelper;

        public PatientService()
        {
            var services = ((App)System.Windows.Application.Current).Services;
            _dataHelper = services.GetService<IdataHelper<Patient>>() ?? 
                throw new InvalidOperationException("Data helper service is not available.");
        }

        public async Task<IEnumerable<Patient>> GetTodaysPatients(DateTime date)
        {
            // Assuming your Patient model has AppointmentTime property
            return await Task.Run(() =>
            {
                return _dataHelper.GetAllData()
                    .Where(p => p.AppointmentTime.Date == date.Date)
                    .OrderByDescending(p => p.AppointmentTime)
                    .ToList();
            });
        }
    }
} 