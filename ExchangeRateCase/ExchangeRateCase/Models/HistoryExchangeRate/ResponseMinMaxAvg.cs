namespace ExchangeRateCase.Models
{
    public class ResponseMinMaxAvg
    {
        public ResponseMinMaxAvg(string Min, string Max, string Avg)
        {
            this.Min = Min;
            this.Max = Max;
            this.Avg = Avg;
        }
        public string Avg { get; set; }

        public string Max { get; set; }

        public string Min { get; set; }
    }
}
