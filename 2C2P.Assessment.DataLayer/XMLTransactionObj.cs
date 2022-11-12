using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2C2P.Assessment.DataLayer
{
    public class XMLTransactionObj
    {
        public string? id { get; set; }
        public string? TransactionDate { get; set; }
        public string? Status { get; set; }
        public XMLPaymentDetailsObj? PaymentDetails { get; set; }
    }

    public class XMLPaymentDetailsObj
    {
        public string? Amount { get; set; }
        public string? CurrencyCode { get; set; }
    }

}
