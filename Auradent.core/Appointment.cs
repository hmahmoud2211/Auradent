using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.core
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string? AppointmentStatus { get; set; }
        // FK
        public int PatientID_FK { get; set; }
        public int DoctorandNurseID_FK { get; set; }
        public int Fainance_Fk { get; set; }
        // Navigation Properties
        public Patient? Patient { get; set; }
        public DoctorandNurse? DoctorandNurse { get; set; }
        public Finance? Finance { get; set; }


    }
}
