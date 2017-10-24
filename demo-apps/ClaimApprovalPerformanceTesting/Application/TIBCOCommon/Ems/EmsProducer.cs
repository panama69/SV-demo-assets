using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HP.SOAQ.ServiceVirtualization.JmsTibcoAgent.Common;
using HP.SOAQ.ServiceVirtualization.MessageModel.MetadataTypes;

using TIBCO.EMS;

namespace HP.SOAQ.ServiceVirtualization.JmsTibcoAgent {
    public class EmsProducer {
        //private static readonly ILog Log = LogManager.GetLogger(typeof(EmsProducer));

        private bool isConnected;
        private Connection connection;
        private Session session;

        /// <summary>
        /// Connect to EMS server under given user account.
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public Session Connect(string serverUrl, string userName, string password) {
            try {
                ConnectionFactory factory = new TIBCO.EMS.ConnectionFactory(serverUrl);
                connection = factory.CreateConnection(userName, password);
                session = connection.CreateSession(false, Session.AUTO_ACKNOWLEDGE);
                isConnected = true;
                return session;
            } catch (EMSException e) {
                Console.Error.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Disconnect publisher connected to EMS server.
        /// </summary>
        public void Disconnect() {
            if (isConnected) {
                isConnected = false;
                try {
                    session.Close();
                    connection.Close();
                } catch (EMSException e) {
                    Console.Error.WriteLine(e);
                    throw;
                }
            } else {
                throw new ApplicationException("EMS Publisher not connected.");
            }
        }

        public bool IsConnected() {
            return isConnected;
        }

        /// <summary>
        /// Send JMS message to given destination.
        /// </summary>
        /// <param name="encodedJmsDestination">Jms destination encoding destination name and destination type. Example: "Topic[topic.input]"</param>
        /// <param name="jmsMessageToPublish"></param>
        public void Send(string encodedJmsDestination, Message jmsMessageToPublish) {
            string jmsDestinationName, jmsdestinationType;
            TibcoDestinationHelper.DecodeDestinationNameAndType(encodedJmsDestination, out jmsDestinationName, out jmsdestinationType);
            Send(jmsDestinationName, jmsdestinationType, jmsMessageToPublish);
        }

        /// <summary>
        /// Send JMS message to given destination.
        /// </summary>
        /// <param name="jmsDestinationName"></param>
        /// <param name="jmsdestinationType"></param>
        /// <param name="jmsMessageToPublish"></param>
        public void Send(string jmsDestinationName, string jmsdestinationType, Message jmsMessageToPublish) {
            if (!isConnected) {
                // TODO localize
                throw new ApplicationException("EmsPublisher not connected, connect before publishing.");
            }
            try {

                
                // Using CreateTopic/CreateQueue to enable publishing into dynamic destinations. 
                // If there is no such destination, it creates one.
                // TODO consider getting destination not creation, if destination doesn't exist it could throw exception
                Destination destination;
                if (jmsdestinationType.Equals(JmsMetadata.DestinationTypeValues.Topic)) {
                    destination = session.CreateTopic(jmsDestinationName);
                } else if (jmsdestinationType.Equals(JmsMetadata.DestinationTypeValues.Queue)) {
                    destination = session.CreateQueue(jmsDestinationName);
                } else {
                    // TODO localize
                    throw new ApplicationException("Uknown destination type.");
                }
                MessageProducer producer = session.CreateProducer(null);
                producer.Send(destination, jmsMessageToPublish);
            } catch (EMSException e) {
                Console.Error.WriteLine(e);
                throw;
            }
        }
    }
}
