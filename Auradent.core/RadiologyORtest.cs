using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.core
{
    public class RadiologyORtest
    {
        public int RadiologyORtestID { get; set; }
        public String? Report { get; set; }
        public byte[]? Labtests { get; set; }
        public byte[]? Scans { get; set; }
        // FK
        public int MedicalRecordID { get; set; }
        // Navigation Properties
        public ICollection<Medical_Record> Medical_Records { get; set; }
    }
}
