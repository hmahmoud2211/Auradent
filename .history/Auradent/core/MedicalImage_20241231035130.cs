using System;

namespace Auradent.core
{
    public class MedicalImage
    {
        public int ImageId { get; set; }
        public int PatientId { get; set; }
        public byte[] ImageData { get; set; }
        public string OcrText { get; set; }
        public DateTime UploadDate { get; set; }
        public string ImageType { get; set; }
    }
}