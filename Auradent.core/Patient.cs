using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.core
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string chronic_diseases { get; set; }
        public string PatientAddress { get; set; }
        public int MedicalRecordID { get; set; }

        [ForeignKey("MedicalRecordID")]
        // Navigation Properties
        public virtual Medical_Record Medical_Record { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }



    }
}
