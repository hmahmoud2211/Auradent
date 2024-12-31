using System;

namespace Auradent.core
{
    public class LabTest
    {
        public int LabTestID { get; set; }
        public int PatientID { get; set; }
        public string ImagePath { get; set; }
        public string OcrText { get; set; }
        public DateTime UploadDate { get; set; }
        public string TestType { get; set; } // "Lab" or "Radiology"
    }
}