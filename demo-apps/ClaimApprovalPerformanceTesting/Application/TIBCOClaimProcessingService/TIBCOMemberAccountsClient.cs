using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using HP.SOAQ.ServiceVirtualization.Common.Util;
using HP.SOAQ.ServiceVirtualization.JmsTibcoAgent;
using HP.SOAQ.ServiceVirtualization.JmsTibcoAgent.NonIntrusive;
using HP.SOAQ.ServiceVirtualization.MessageModel.MetadataTypes;
using TIBCO.EMS;
using TIBCO.EMS.ADMIN;
using HP.SOAQ.ServiceSimulation.Demo.Properties;


namespace HP.SOAQ.ServiceSimulation.Demo {
    class TIBCOMemberAccountsClient {

        private EmsListener emsListener;
        string providerUrl, username, password, destinationName, destinationType, replyToDestinationName, replyToDestinationType, agentUsername, agentPassword;
        private ResponseHandler responseHandler;
        const int OneLoopTime = 100;

        EmsProducer emsProducer = new EmsProducer();
        private Session session;

        public TIBCOMemberAccountsClient()
        {
            // setup according settings
            Setup();

            session = emsProducer.Connect(providerUrl, username, password);
        }

        ~TIBCOMemberAccountsClient()
        {
            emsProducer.Disconnect();
            emsListener.StopListening();
        }


        public IXMLMemberAccountsResponse call(IXMLMemberAccountsRequest request) {
            MemoryStream reqStream = new MemoryStream();
            DataContractSerializer ser = new DataContractSerializer(typeof(IXMLMemberAccountsRequest));
            ser.WriteObject(reqStream, request);

            ListenOnReplyToDestination();
            SendRequestToService(reqStream.ToArray());
            byte[] responseBytes = WaitForResponse(Settings.Default.ClientTimeout);

            ser = new DataContractSerializer(typeof(IXMLMemberAccountsResponse));
            IXMLMemberAccountsResponse response = (IXMLMemberAccountsResponse)ser.ReadObject(new MemoryStream(responseBytes));
            return response;
        }            

        /// <summary>
        /// Wait for response. If timeout is reached, it throws TimeoutException.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public byte[] WaitForResponse(int timeout) {
            try {
                int timeoutTime = timeout;
                
                while (responseHandler.Response == null) {
                    Thread.Sleep(OneLoopTime);
                    timeoutTime = timeoutTime - OneLoopTime;
                    if (timeoutTime <= 0) {
                        throw new TimeoutException("Response not received in " + (timeout / 1000) + " seconds");
                    }
                }
                Message response = responseHandler.Response;

                if (response is TextMessage) {
                    throw new NotImplementedException();
                } else if (response is BytesMessage) {
                    BytesMessage bytesMessage = (BytesMessage)response;
                    byte [] responseMsgBytes = new byte[bytesMessage.BodyLength];
                    bytesMessage.ReadBytes(responseMsgBytes);
                    return responseMsgBytes;
                } else {
                    throw new Exception("Message other than BytesMessage received: " + response);
                }
            } finally {
                // clear response for other request
                responseHandler.Response = null;
            }
        }

        /// <summary>
        /// Start to listen on replyTo destination for responses.
        /// </summary>
        public void ListenOnReplyToDestination() {
            lock(this)
            {
                if (responseHandler == null)
                {
                    responseHandler = new ResponseHandler();

                    // configure listener to listen on reply to destination
                    emsListener = new EmsListener();
                    emsListener.Configure(providerUrl, username, password, replyToDestinationName,
                                          replyToDestinationType == JmsMetadata.DestinationTypeValues.Topic,
                                          responseHandler, null);
                    emsListener.StartListening();
                }
            }
        }

        /// <summary>
        /// Setup approval client according parameters in application settings.
        /// </summary>
        private void Setup() {
            Validation.NotNull(Settings.Default.ProviderUrl);
            Validation.NotNull(Settings.Default.ServiceUsername);
            Validation.NotNull(Settings.Default.ServicePassword);
            Validation.NotNull(Settings.Default.DestinationName);
            Validation.NotNull(Settings.Default.DestinatioType);
            Validation.NotNull(Settings.Default.ReplyToDestinationName);
            Validation.NotNull(Settings.Default.ReplyToDestinationType);
            Validation.NotNull(Settings.Default.AgentUsername);
            Validation.NotNull(Settings.Default.AgentPassword);

            providerUrl = Settings.Default.ProviderUrl;
            username = Settings.Default.ServiceUsername;
            password = Settings.Default.ServicePassword;
            destinationName = Settings.Default.DestinationName;
            destinationType = Settings.Default.DestinatioType;
            replyToDestinationName = Settings.Default.ReplyToDestinationName;
            replyToDestinationType = Settings.Default.ReplyToDestinationType;
            agentUsername = Settings.Default.AgentUsername;
            agentPassword = Settings.Default.AgentPassword;

            /*Console.WriteLine(
                    String.Format("Used Args: [serviceServerUrl={0}], [username={1}] and [password={2}], [destinationName={3}], [destinationType={4}], [replyToDestinationName={5}], [replyToDestinationType={6}], [agentUsername={7}] and [agentPassword={8}].",
                                  providerUrl, username, password, destinationName, destinationType, replyToDestinationName, replyToDestinationType, agentUsername, agentPassword));*/
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

            ACLEntry subscribeOnDestinationACL = new ACLEntry(replyDestinationInfo,
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

            ACLEntry publishOnDestinationACL = new ACLEntry(targetDestinationInfo,
                                                            serviceUser,
                                                            sendAndPublishPermissions);
            // grand permissions for replyTo destination
            tibcoAdmin.Grant(publishOnDestinationACL);
        }

        /// <summary>
        /// Create and send request message.
        /// </summary>
        public void SendRequestToService(byte[] messageBytes) {
            Message message = CreateMessageFromBytes(messageBytes, session);
            emsProducer.Send(destinationName, destinationType, message);
        }

        /// <summary>
        /// Create bytes message from given message bytes and setup reply to destination.
        /// </summary>
        /// <param name="messageBytes"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private Message CreateMessageFromBytes(byte [] messageBytes, Session session) {
            BytesMessage message = new BytesMessage(session);
            message.WriteBytes(messageBytes);
            if (replyToDestinationType.Equals(JmsMetadata.DestinationTypeValues.Topic)) {
                message.ReplyTo = new Topic(replyToDestinationName);
            } else if (replyToDestinationType.Equals(JmsMetadata.DestinationTypeValues.Queue)) {
                message.ReplyTo = new Queue(replyToDestinationName);
            }
            return message;
        }

        private class ResponseHandler : IMessageListener {
            public Message Response = null;

            public void OnMessage(Message message) {
                Response = message;
                //Console.WriteLine("Response received: " + message);
            }
        }
    }


    
}
