using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Web.App.Models
{
    public class PaymentDetails
    {
        public string OrderId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentType { get; set; }
    }
}