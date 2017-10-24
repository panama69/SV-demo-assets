﻿using System;

using System.ServiceModel;
using System.ServiceModel.Description;

namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceBehavior(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class ApprovalService {

        const string SERVICE_NAME = "ApprovalService";

        static void Main(string[] args) {
            // Decide type of security, None is default when param is missing
            HttpClientCredentialType type = HttpClientCredentialType.None;
            if (args.Length > 0) {
                if (!Enum.TryParse(args[0], true, out type)) {
                    Console.WriteLine("Wrong security type. Add \"none\" for no security, \"basic\" for basic, \"ntlm\" for NTLM");
                }
            }

            // create service endpoint
            Uri baseAddress = new Uri("http://localhost:8103/ServiceSimulation/Demo/" + SERVICE_NAME);
            ServiceHost selfHost = new ServiceHost(typeof(Approval), baseAddress);

            try {
                BasicHttpBinding binding = new BasicHttpBinding() { Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01" };
                binding.MaxReceivedMessageSize = binding.MaxBufferSize = 4000000;
                binding.ReaderQuotas.MaxArrayLength = 4000000;
                binding.MessageEncoding = WSMessageEncoding.Mtom;
                if (type != HttpClientCredentialType.None)
                {
                    binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                    binding.Security.Transport.ClientCredentialType = type;
                }
                // add service endpoint
                selfHost.AddServiceEndpoint(typeof(IApproval), binding, SERVICE_NAME);

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