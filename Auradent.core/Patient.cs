using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.core
{
    public class Patient
    {
        public int PatientID { get; set; }
        public String? PatientName { get; set; }
        public String? PatientAddress { get; set; }
        public String? PatientPhone { get; set; }
        public String? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String? chronic_diseases { get; set; }
        // FK
        public int MedicalRecordID { get; set; }
        // Navigation Properties
        public Medical_Record Medical_Record { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }



    }
}
