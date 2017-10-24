using System;
using System.IO;
using System.Runtime.Serialization;

using HP.SOAQ.ServiceVirtualization.Common.Util;
using HP.SOAQ.ServiceVirtualization.JmsTibcoAgent;
using HP.SOAQ.ServiceVirtualization.JmsTibcoAgent.NonIntrusive;
using HP.SOAQ.ServiceVirtualization.MessageModel.MetadataTypes;
using TIBCO.EMS;
using TIBCO.EMS.ADMIN;

namespace HP.SOAQ.ServiceSimulation.Demo{
    class TIBCOMemberAccountsService {

        string providerUrl, username, password, destinationName, destinationType, replyToDestinationName, replyToDestinationType, agentUsername, agentPassword;
        private ServiceRequestHandler serviceRequestHandler;
        private EmsListener emsListener;

        private static readonly XMLMemberAccounts xmlMemberAccounts = new XMLMemberAccounts();

        static void Main(string[] args) {

            TIBCOMemberAccountsService service = new TIBCOMemberAccountsService();
            service.Setup();
            service.Start();

            Console.WriteLine("Press <ENTER> to stop the service.");
            Console.ReadLine();
            service.Stop();
            Environment.Exit(0);
        }


        /// <summary>
        /// Setup service according parameters in application settings.
        /// </summary>
        private void Setup() {
            Validation.NotNull(Properties.Settings.Default.ProviderUrl);
            Validation.NotNull(Properties.Settings.Default.ServiceUsername);
            Validation.NotNull(Properties.Settings.Default.ServicePassword);
            Validation.NotNull(Properties.Settings.Default.DestinationName);
            Validation.NotNull(Properties.Settings.Default.DestinatioType);
            Validation.NotNull(Properties.Settings.Default.ReplyToDestinationName);
            Validation.NotNull(Properties.Settings.Default.ReplyToDestinationType);
            Validation.NotNull(Properties.Settings.Default.AgentUsername);
            Validation.NotNull(Properties.Settings.Default.AgentPassword);

            providerUrl = Properties.Settings.Default.ProviderUrl;
            username = Properties.Settings.Default.ServiceUsername;
            password = Properties.Settings.Default.ServicePassword;
            destinationName = Properties.Settings.Default.DestinationName;
            destinationType = Properties.Settings.Default.DestinatioType;
            replyToDestinationName = Properties.Settings.Default.ReplyToDestinationName;
            replyToDestinationType = Properties.Settings.Default.ReplyToDestinationType;
            agentUsername = Properties.Settings.Default.AgentUsername;
            agentPassword = Properties.Settings.Default.AgentPassword;
            
            // prepare destination
            PrepareDestinations();
        }

        /// <summary>
        /// Creates EMS target and replyTo destinations if not exist. 
        /// Creates client and service user account in EMS if not exist. 
        /// Setup permissions.
        /// </summary>
        private void PrepareDestinations() {
            Admin tibcoAdmin = new Admin(providerUrl, agentUsername, agentPassword);
            DestinationInfo targetDestinationInfo = null;
            if (destinationType.Equals(JmsMetadata.DestinationTypeValues.Topic)) {
                targetDestinationInfo = tibcoAdmin.GetTopic(destinationName);
                if (targetDestinationInfo == null) {
                    try {
                        targetDestinationInfo = tibcoAdmin.CreateTopic(new TopicInfo(destinationName));
                    } catch (AdminNameExistsException) {
                        // something (probably client) creates this destination faster
                        targetDestinationInfo = tibcoAdmin.GetTopic(destinationName);
                    }
                }
            } else if (destinationType.Equals(JmsMetadata.DestinationTypeValues.Queue)) {
                targetDestinationInfo = tibcoAdmin.GetQueue(destinationName);
                if (targetDestinationInfo == null) {
                    try {
                        targetDestinationInfo = tibcoAdmin.CreateQueue(new QueueInfo(destinationName));
                    } catch (AdminNameExistsException) {
                        // something (probably client) creates this destination faster
                        targetDestinationInfo = tibcoAdmin.GetTopic(destinationName);
                    }
                }
            }

            DestinationInfo replyDestinationInfo = null;
            if (replyToDestinationType.Equals(JmsMetadata.DestinationTypeValues.Topic)) {
                replyDestinationInfo = tibcoAdmin.GetTopic(replyToDestinationName);
                if (replyDestinationInfo == null) {
                    try {
                        replyDestinationInfo = tibcoAdmin.CreateTopic(new TopicInfo(replyToDestinationName));
                    } catch (AdminNameExistsException) {
                        // something (probably client) creates this destination faster
                        replyDestinationInfo = tibcoAdmin.GetTopic(replyToDestinationName);
                    }
                }
            } else if (replyToDestinationType.Equals(JmsMetadata.DestinationTypeValues.Queue)) {
                replyDestinationInfo = tibcoAdmin.GetQueue(replyToDestinationName);
                if (replyDestinationInfo == null) {
                    try {
                        replyDestinationInfo = tibcoAdmin.CreateQueue(new QueueInfo(replyToDestinationName));
                    } catch (AdminNameExistsException) {
                        // something (probably client) creates this destination faster
                        replyDestinationInfo = tibcoAdmin.GetTopic(replyToDestinationName);
                    }
                }
            }

            UserInfo serviceUser = tibcoAdmin.GetUser(username);
            if (serviceUser == null) {
                serviceUser = tibcoAdmin.CreateUser(new UserInfo(username));
            }
            serviceUser.Password = password;
            tibcoAdmin.UpdateUser(serviceUser);

            // create permissions
            Permissions receiveAndSubscribePermissions = new Permissions();
            receiveAndSubscribePermissions.SetPermission(Permissions.VIEW, true);
            // revoke receive for a gueue
            receiveAndSubscribePermissions.SetPermission(Permissions.RECEIVE, true);
            // revoke subscribe for a topic
            receiveAndSubscribePermissions.SetPermission(Permissions.SUBSCRIBE, true);

            ACLEntry subscribeOnDestinationACL = new ACLEntry(targetDestinationInfo,
                                                              serviceUser,
                                                              receiveAndSubscribePermissions);
            // grand permissions for target destination
            tibcoAdmin.Grant(subscribeOnDestinationACL);

            // create permissions
            Permissions sendAndPublishPermissions = new Permissions();
            sendAndPublishPermissions.SetPermission(Permissions.VIEW, true);
            // revoke send for a gueue
            sendAndPublishPermissions.SetPermission(Permissions.SEND, true);
            // revoke send for a topic
            sendAndPublishPermissions.SetPermission(Permissions.PUBLISH, true);

            ACLEntry publishOnDestinationACL = new ACLEntry(replyDestinationInfo,
                                                            serviceUser,
                                                            sendAndPublishPermissions);
            // grand permissions for replyTo destination
            tibcoAdmin.Grant(publishOnDestinationACL);
        }

        /// <summary>
        /// Starts listening for request on service destination.
        /// </summary>
        private void Start() {
            // configure listener to listen on reply to destination
            emsListener = new EmsListener();
            serviceRequestHandler = new ServiceRequestHandler(providerUrl, username, password);

            emsListener.Configure(providerUrl, username, password, destinationName,
                                  destinationType == JmsMetadata.DestinationTypeValues.Topic, serviceRequestHandler,
                                  null);
            emsListener.StartListening();
            Console.WriteLine(String.Format("Member Accounts Service listening at: {0} \n in EMS server: {1}", destinationName, providerUrl));
            
        }

        private void Stop() {
            emsListener.StopListening();
        }

        class ServiceRequestHandler : IMessageListener {
            private readonly EmsProducer emsProducer;
            private readonly Session session;

            public ServiceRequestHandler(string serverUrl, string username, string password) {
                emsProducer = new EmsProducer();
                session = emsProducer.Connect(serverUrl, username, password);
            }

            public void OnMessage(Message message) {
                //Console.WriteLine("Msg received: " + message);
                if (message is TextMessage) {
                    TextMessage originalTextJmsMessage = (TextMessage) message;
                    string bodyText = originalTextJmsMessage.Text;
                    throw new NotImplementedException();
                }
                if (message is BytesMessage) {
                    BytesMessage originalBytesJmsMessage = (BytesMessage) message;
                    byte[] bodyBytes = new byte[originalBytesJmsMessage.BodyLength];
                    int result = originalBytesJmsMessage.ReadBytes(bodyBytes);
                    /*Console.WriteLine("Request:");
                    PrintOutBytes(bodyBytes);*/
                    byte [] responseBytes = ProcessRequestAndGetResponse(bodyBytes);
                    /*Console.WriteLine("Response:");
                    PrintOutBytes(responseBytes);*/

                    Message responseBytesMessage = CreateResponseByteMessage(responseBytes, session);
                    // set corellation id
                    responseBytesMessage.CorrelationID = originalBytesJmsMessage.MessageID;
                    emsProducer.Send(Properties.Settings.Default.ReplyToDestinationName, Properties.Settings.Default.ReplyToDestinationType, responseBytesMessage);
                } 
            }

            private byte[] ProcessRequestAndGetResponse(byte[] requestBytes) {
                DataContractSerializer ser = new DataContractSerializer(typeof(IXMLMemberAccountsRequest));
                IXMLMemberAccountsRequest request = (IXMLMemberAccountsRequest)ser.ReadObject(new MemoryStream(requestBytes));

                IXMLMemberAccountsResponse response = xmlMemberAccounts.process(request);

                MemoryStream outStream = new MemoryStream();
                ser = new DataContractSerializer(typeof(IXMLMemberAccountsResponse));
                ser.WriteObject(outStream, response);

                return outStream.ToArray();
            }

            private static void PrintOutBytes(byte [] bytes) {
                using (StreamReader reader = new StreamReader(new MemoryStream(bytes))) {
                    string contents = reader.ReadToEnd();
                    Console.Write(contents);
                }
            }

            private static Message CreateResponseByteMessage(byte [] messageBytes, Session session) {
                BytesMessage bytesMessage = new BytesMessage(session);
                bytesMessage.WriteBytes(messageBytes);
                return bytesMessage;
            }
        }
       
    }
}
