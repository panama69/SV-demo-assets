using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace HP.SOAQ.ServiceSimulation.Demo
{

    [ServiceBehavior(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class ExchangeRate : IExchangeRate
    {

        #region performance settings
        private static readonly int RESPONSE_TIME = 25;  
        #endregion

        // Exchange rates are update with following period:
        protected const int CURRENT_RATE_UPDATE_INTERVAL = 15; // [s]

        private readonly Random RNG = new Random();

        private readonly IDictionary<string, float> currentRates = new Dictionary<string, float>();

        private DateTime nextRateChange = DateTime.Now;

        public ExchangeRate()
        {
            currentRates["USD"] = 1.00f;
            currentRates["GPB"] = 0.63f;
            currentRates["EUR"] = 0.72f;
            currentRates["CHF"] = 0.98f;
            currentRates["JPY"] = 81.63f;
        }

        public float getCurrentRate(CurrencyPair currencies)
        {
            Console.Write("getCurrentRate(" + currencies.from + " -> " + currencies.to + " = ");

            updateCurrentRates();

            float fromRatio = currentRates[currencies.from.ToString()];
            float toRatio = currentRates[currencies.to.ToString()];

            Console.WriteLine(toRatio / fromRatio);

            Thread.Sleep(RESPONSE_TIME);
            return toRatio / fromRatio;
        }


        protected void updateCurrentRates()
        {
            if (DateTime.Compare(DateTime.Now, nextRateChange) > 0)
            {
                nextRateChange = nextRateChange.AddSeconds(CURRENT_RATE_UPDATE_INTERVAL);

                foreach (string key in new List<string>(currentRates.Keys))
                {
                    if (!Currency.USD.Equals(key))
                    {
                        currentRates[key] = currentRates[key] * (1000 + RNG.Next(21) - 10) / 1000;
                    }
                }
            }

        }

        public float getNoonRate(CurrencyPair currencies)
        {
            throw new NotImplementedException();
        }

        public float getHistoricalNoonRate(CurrencyPair currencies, DateTime date)
        {
            throw new NotImplementedException();
        }

    }

}


