using System;

namespace Auradent.core
{
    public class Finance
    {
        public int FinanceID { get; set; }
        public int PatientID { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }
} 