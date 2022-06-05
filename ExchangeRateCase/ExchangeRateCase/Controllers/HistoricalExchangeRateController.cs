using System;
using ExchangeRateCase.Models;
using ExchangeRateCase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ExchangeRateCaseSolution.Helper;
using ExchangeRateCaseSolution.Models.HistoryExchangeRate;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateCaseSolution.Services.DateCollectionValidatorService;
using System.Collections.Generic;

namespace ExchangeRateCase.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricalExchangeRateController : ControllerBase
    {
        private readonly IHistoricalExchangeRateService _historicalExchangeRateService;
        private readonly IDateCollectionValidatorService _dateCollectionValidatorService;

        public HistoricalExchangeRateController(IHistoricalExchangeRateService historicalExchangeRateService, IDateCollectionValidatorService dateCollectionValidatorService)
        {
            _historicalExchangeRateService = historicalExchangeRateService ?? throw new ArgumentNullException(nameof(historicalExchangeRateService));
            _dateCollectionValidatorService = dateCollectionValidatorService ?? throw new ArgumentNullException(nameof(dateCollectionValidatorService));
        }

        [HttpPost]
        [Route("rates")]
        public async Task<IActionResult> MaxMinAvgRates(string baseCurrency, string targetCurrency, [FromBody] DateData listDate)
        {
            var listOfDates = listDate.Date[listDate.Date.Keys.First()];

            var validationResult = _dateCollectionValidatorService.ValidateDates(listOfDates);

            if (validationResult is not null)
            {
                ModelState.AddModelError(
                    Messages.ERROR_KEY,
                    validationResult
                    );
                return BadRequest(ModelState);
            }

            ICollection<ExchangeRate> list;

            try
            {
                list = await _historicalExchangeRateService.SendRequestRemoteServerAsync(baseCurrency, targetCurrency, listOfDates);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                      Messages.ERROR_KEY,
                      $"{Messages.RATE_CONVERT_ERROR_MESSAGE} {Messages.ERROR_MESSAGE}: {ex.Message}"
                  );
                return BadRequest(ModelState);
            }

            if (list.Count > 0)
            {
                var avgMinMax = new AvgMinMax();

                try
                {
                    avgMinMax = _historicalExchangeRateService.GetAvgMinMax(list);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(
                      Messages.ERROR_KEY,
                      $"{ Messages.CALCULATION_ERROR_MESSAGE} {Messages.ERROR_MESSAGE}: {ex.Message}"
                    );
                    return BadRequest(ModelState);
                }

                var result = _historicalExchangeRateService.GetFormattedResponse(avgMinMax);
                var response = ResponseData.Ok(result);
                return Ok(response);
            }
            else
            {
                ModelState.AddModelError(
                       Messages.ERROR_KEY,
                       Messages.CORRUPTED_DATA_MESSAGE
                   );
                return BadRequest(ModelState);
            }
        }
    }
}
