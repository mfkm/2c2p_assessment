using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2C2P.Assessment.DataLayer
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class DataResult : Result
    {
        public List<SavedData> Data { get; set; }
    }
}
