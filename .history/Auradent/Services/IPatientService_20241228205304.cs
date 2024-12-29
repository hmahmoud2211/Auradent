using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auradent.Models;

namespace Auradent.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetTodaysPatients(DateTime date);
    }
} 