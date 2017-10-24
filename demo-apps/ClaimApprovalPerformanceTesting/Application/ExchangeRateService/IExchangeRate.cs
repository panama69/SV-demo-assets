using System;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceContract(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", SessionMode = SessionMode.NotAllowed)]
    public interface IExchangeRate {

        [OperationContract]
        float getNoonRate(CurrencyPair currencies);

        [OperationContract]
        float getHistoricalNoonRate(CurrencyPair currencies, DateTime date);

        [OperationContract]
        float getCurrentRate(CurrencyPair currencies);

    }

    [DataContract]
    public class CurrencyPair {
        [DataMember]
        public Currency from { get; set; }
        [DataMember]
        public Currency to { get; set; }
    }


}
