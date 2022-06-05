using System;
using System.Collections.Generic;

namespace ExchangeRateCaseSolution.Services.DateCollectionValidatorService
{
    public interface IDateCollectionValidatorService
    {
        public string ValidateDates(ICollection<DateTime> dates);

    }
}
