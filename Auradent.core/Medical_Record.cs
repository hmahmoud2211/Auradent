using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.core
{
    public class Medical_Record
    {
        public int RecordId { get; set; }
        public string? Subjective { get; set; }
        public string? objective { get; set; }
        public string? Report { get; set; }
        public string? Assessment { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? Notes { get; set; }
        public DateTime RecordDate { get; set; }
        // Navigation Properties
        public Patient Patient { get; set; }
        public RadiologyORtest RadiologyORtest { get; set; }

    }
}
