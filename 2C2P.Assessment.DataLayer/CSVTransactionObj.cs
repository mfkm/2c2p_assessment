using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2C2P.Assessment.DataLayer
{
    public class CSVTransactionObj
    {
        [Index(0)]
        public string TxId { get; set; }
        [Index(1)]
        public string Amount { get; set; }
        [Index(2)]
        public string CurrencyCode { get; set; }
        [Index(3)]
        public string TxDate { get; set; }
        [Index(4)]
        public string Status { get; set; }
    }
}
