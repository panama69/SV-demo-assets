using System;

using System.ServiceModel;
using System.ServiceModel.Description;

namespace HP.SOAQ.ServiceSimulation.Demo {
    class ClaimProcessingService {
        const string SERVICE_NAME = "ClaimProcessingService";

        static void Main(string[] args) {
            // update the URL of MemberAccountsService according to commandline argument
            // Aguments: {service url} {security type} or
            // {security type} only
            if (args.Length > 0) {
                HttpClientCredentialType type;
                // Ugly hack (I have no time) = try to parse first param as authentication type, if it is not, take as address, NEEDS TO BE REFACTORED
                if (!Enum.TryParse(args[0], true, out type)) {
                    // It is URL address
                    Console.WriteLine("Using MemberAccountService at:\n" + args[0]);
                    Config.GetInstance().ServiceUrl = args[0];
                }
                // Choose security type if there is even second parameter
                if (args.Length > 1 && !Enum.TryParse(args[1], true, out type)) {
                    Console.WriteLine("Wrong security type. Add \"none\" for no security, \"basic\" for basic, \"ntlm\" for NTLM");
                }
                Config.GetInstance().Type = type;
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
            } catch (CommunicationException e) {
                Console.WriteLine("Exception occurred: {0}", e.Message);
                selfHost.Abort();
            }
        }
    }
}
