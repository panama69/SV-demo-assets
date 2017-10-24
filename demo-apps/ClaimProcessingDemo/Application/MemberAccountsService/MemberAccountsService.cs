using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceBehavior(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class MemberAccountsService {

        const string SERVICE_NAME = "MemberAccountsService";

        static void Main(string[] args) {
            // Decide type of security, None is default when param is missing
            HttpClientCredentialType type = HttpClientCredentialType.None;
            if (args.Length > 0) {
                if (!Enum.TryParse(args[0], true, out type)) {
                    Console.WriteLine("Wrong security type. Add \"none\" for no security, \"basic\" for basic, \"ntlm\" for NTLM");
                }
            }

            // create service endpoint
            Uri baseAddress = new Uri("http://"+ Dns.GetHostName() +":8101/ServiceSimulation/Demo/" + SERVICE_NAME);
            ServiceHost selfHost = new ServiceHost(typeof(MemberAccounts), baseAddress);

            try {
                if (type != HttpClientCredentialType.None) {
                    BasicHttpBinding binding = new BasicHttpBinding() { Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01" };
                    binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                    binding.Security.Transport.ClientCredentialType = type;
                    // add service endpoint
                    selfHost.AddServiceEndpoint(typeof(IMemberAccounts), binding, SERVICE_NAME);
                } else {
                    // add service endpoint
                    selfHost.AddServiceEndpoint(typeof(IMemberAccounts), new BasicHttpBinding() { Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01" }, SERVICE_NAME);
                }

                // enable WSDL
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);

                // start the service
                selfHost.Open();
                if (type == HttpClientCredentialType.None) {
                    Console.WriteLine("The " + SERVICE_NAME + " is ready at:\n" + baseAddress);
                } else {
                    Console.WriteLine("The " + SERVICE_NAME + " secured by " + type + " authentication is ready at:\n" + baseAddress);
                }
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
