using ExchangeRateCase.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateCase.Services
{
    public interface IHistoricalExchangeRateService
    {
        Task<ICollection<ExchangeRate>> SendRequestRemoteServerAsync(string baseCurrency, string targetCurrency, ICollection<DateTime> listDate);

        AvgMinMax GetAvgMinMax(ICollection<ExchangeRate> list);

        ResponseMinMaxAvg GetFormattedResponse(AvgMinMax avgMinMax);
    }
}
