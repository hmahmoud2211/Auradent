using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auradent.core
{
    public class Finance
    {
        
        public int FinanceId { get; set; }
        public decimal TotalAmount { get; set; }
        // make attribute for payment method cash or visa
        public string? PaymentMethod { get; set; }

        public string? TypeOfPayment { get; set; }

        public Appointment Appointments { get; set; }
    }

    
    

    }
