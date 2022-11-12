using System;
using System.Collections.Generic;

namespace _2C2P.Assessment.DataLayer.Models;

public partial class ImportedDatum
{
    public string TxId { get; set; } = null!;

    public double Amount { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public DateTime TxDate { get; set; }

    public string Status { get; set; } = null!;

    public string FinalStatus { get; set; } = null!;

    public string SourceData { get; set; } = null!;
}
