using System.Collections.Generic;

namespace ExchangeRateCase.Models
{
    public class ExchangeRate
    {
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
