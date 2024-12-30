using System;

namespace Auradent.core
{
    public class Visit_Record
    {
        public int VisitId { get; set; }
        public int PatientID_FK { get; set; }
        public DateTime VisitDate { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plan { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; } // Completed, Ongoing, etc.

        public Patient Patient { get; set; }
    }
} 