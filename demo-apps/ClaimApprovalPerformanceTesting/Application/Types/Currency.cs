using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HP.SOAQ.ServiceSimulation.Demo {

    [DataContract]
    public enum Currency
    {
        [EnumMember]
        USD,
        [EnumMember]
        GPB,
        [EnumMember]
        EUR,
        [EnumMember]
        CHF,
        [EnumMember]
        JPY
    }
}
