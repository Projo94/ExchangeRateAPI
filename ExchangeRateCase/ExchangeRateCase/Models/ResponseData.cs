using ExchangeRateCaseSolution.Helper;
using System.Collections.Generic;


namespace ExchangeRateCase.Models
{
    public static class ResponseData
    {
        public static Dictionary<string, object> Ok(object data)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("data", data);
            dict.Add("result", Messages.RESULT_SUCCESS);

            return dict;
        }
    }
}
