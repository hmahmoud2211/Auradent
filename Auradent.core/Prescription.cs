using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.core
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        // make multivalued attribute for the medicine
        public List<Medicine>? Medicines { get; set; }

        public String? Doses { get; set; }

        public DateTime PrescriptionDate { get; set; }
        // FK
        public int PatientID_FK { get; set; }
        public int DoctorandNursID_FK { get; set; }
        // Navigation Properties
        public Patient? Patient { get; set; }
        public DoctorandNurse? DoctorandNurse { get; set; }



    }
}
