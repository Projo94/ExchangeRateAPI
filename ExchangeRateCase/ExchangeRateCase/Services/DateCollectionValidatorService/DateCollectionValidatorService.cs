using ExchangeRateCaseSolution.Helper;
using System;
using System.Collections.Generic;

namespace ExchangeRateCaseSolution.Services.DateCollectionValidatorService
{
    public class DateCollectionValidatorService : IDateCollectionValidatorService
    {
        public string ValidateDates(ICollection<DateTime> dates)
        {
            string result = null;

            if (dates.Count == 0)
            {
                result = Messages.DATE_LIST_ERROR_MESSAGE;
                return result;
            }

            foreach (var date in dates)
            {
                if (date > DateTime.Now)
                {
                    result = $"{Messages.DATE_AHEAD_ERROR_MESSAGE} {date.ToString("yyyy-MM-dd")}";
                    break;
                }
            }

            return result;
        }
    }
}
