using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Description;

namespace HP.SOAQ.ServiceSimulation.Demo {
    class ClaimProcessingService {
        const string SERVICE_NAME = "ClaimProcessingService";

        static void Main(string[] args) {
            // update the URL of MemberAccountsService according to commandline argument
            if (args.Length > 0) {
                Console.WriteLine("Using MemberAccountService at:\n" + args[0]);
                Config.getInstance().memberAcoountsServiceURL = args[0];
                if (args.Length > 1) {
                    Console.WriteLine("Using ApprovalService at:\n" + args[1]);
                    Config.getInstance().approvalServiceURL = args[1];
                    if (args.Length > 2) {
                        Console.WriteLine("Using ExchangeRateService at:\n" + args[2]);
                        Config.getInstance().exchangeRateServiceURL = args[2];
                    }
                }
            }

            // create service endpoint
            Uri baseAddress = new Uri("http://localhost:8102/ServiceSimulation/Demo/" + SERVICE_NAME);
            ServiceHost selfHost = new ServiceHost(typeof(ClaimProcessing), baseAddress);

            try {
                // add service endpoint
                selfHost.AddServiceEndpoint(typeof(IClaimProcessing), new BasicHttpBinding() { Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01" }, SERVICE_NAME);

                // enable WSDL
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);

                // start the service
                selfHost.Open();
                Console.WriteLine("The " + SERVICE_NAME + " is ready at:\n" + baseAddress);
                Console.WriteLine("Press <ENTER> to stop the service.");
                Console.ReadLine();

                // shutdown
                selfHost.Close();
            }
            catch (CommunicationException e) {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                selfHost.Abort();
            }
        }
    }
}
