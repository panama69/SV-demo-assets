using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HP.SOAQ.ServiceSimulation.Demo {
    public class Config {

        private static readonly Config Instance = new Config();

        private Config() { }

        public static Config GetInstance() {
            return Instance;
        }
        
        public string ServiceUrl { get; set; }
        public HttpClientCredentialType Type { get; set; }
    }
}
