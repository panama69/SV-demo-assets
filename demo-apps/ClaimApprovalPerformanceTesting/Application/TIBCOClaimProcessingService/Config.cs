using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.SOAQ.ServiceSimulation.Demo {
    public class Config {

        private static Config instance = new Config();

        private Config() { }

        public static Config getInstance() {
            return instance;
        }


        public string memberAcoountsServiceURL { get; set; }
        public string approvalServiceURL { get; set; }
        public string exchangeRateServiceURL { get; set; }

    }
}
