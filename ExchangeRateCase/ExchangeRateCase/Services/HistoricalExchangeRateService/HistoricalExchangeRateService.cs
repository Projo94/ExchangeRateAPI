using ExchangeRateCase.Models;
using ExchangeRateCaseSolution.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateCase.Services
{
    public class HistoricalExchangeRateService : IHistoricalExchangeRateService
    {
        static object _syncroot = new();
        private readonly string _exchangeRateUrl;

        public HistoricalExchangeRateService(IConfiguration configuration)
        {
            _exchangeRateUrl = configuration["API:ExchangeRateAPI"];
        }

        public AvgMinMax GetAvgMinMax(ICollection<ExchangeRate> list)
        {
            var dictionary = new Dictionary<decimal, DateTime>(list.Count);

            foreach (var item in list)
            {
                var rateDecimal = item.Rates.First().Value;
                dictionary.TryAdd(rateDecimal, Convert.ToDateTime(item.Date));
            }

            var avgMinMax = new AvgMinMax();

            if (dictionary.Keys.Count > 0)
            {
                avgMinMax.Avg = dictionary.Average(k => k.Key);
                avgMinMax.Min = dictionary.Keys.Min();
                avgMinMax.DateMin = dictionary[avgMinMax.Min].ToString("yyyy-MM-dd");
                avgMinMax.Max = dictionary.Keys.Max();
                avgMinMax.DateMax = dictionary[avgMinMax.Max].ToString("yyyy-MM-dd");
            }

            return avgMinMax;
        }

        public ResponseMinMaxAvg GetFormattedResponse(AvgMinMax avgMinMax)
        {
            var min = $"{Messages.MIN_RATE} {avgMinMax.Min} {Messages.ON} {avgMinMax.DateMin}";
            var max = $"{Messages.MAX_RATE} {avgMinMax.Max} {Messages.ON} {avgMinMax.DateMax}";
            var avg = $"{Messages.AVG_RATE} {avgMinMax.Avg}";

            var responseMinMaxAvg = new ResponseMinMaxAvg(min, max, avg);

            return responseMinMaxAvg;
        }

        public async Task<ICollection<ExchangeRate>> SendRequestRemoteServerAsync(string baseCurrency, string targetCurrency, ICollection<DateTime> listDate)
        {
            var list = new List<ExchangeRate>();
            var client = new HttpClient();

            var urls = new List<string>();
            var currencyParametersUrl = $"base={ baseCurrency }&symbols={ targetCurrency }";

            foreach (var date in listDate)
            {
                var dateString = date.ToString("yyyy-MM-dd");
                var url = $"{_exchangeRateUrl}{ dateString }?{ currencyParametersUrl }";
                urls.Add(url);
            }

            var requests = urls.Select
                (
                    url => client.GetAsync(url)
                ).ToList();

            await Task.WhenAll(requests);

            var responses = requests.Select
                (
                    task => task.Result
                );

            foreach (var r in responses)
            {
                var responseString = await r.Content.ReadAsStringAsync();
                lock (_syncroot)
                {
                    var responseDeserilized = JsonConvert.DeserializeObject<ExchangeRate>(responseString);

                    if (responseDeserilized is not null)
                    {
                        if (responseDeserilized.Date is null || responseDeserilized.Rates is null)
                            return new List<ExchangeRate>();
                    }
                    else
                    {
                        return new List<ExchangeRate>();
                    }

                    list.Add(responseDeserilized);
                }
            }

            return list;
        }
    }
}
