using System;
using JmsAgentCommon.JmsAgent;
using TIBCO.EMS;

namespace HP.SOAQ.ServiceVirtualization.JmsTibcoAgent.NonIntrusive {
    /// <summary>
    /// Class for monitor system topic and receive monitoring message.
    /// Monitoring messages are used to retrieve original Jms messages sent to destination,
    /// we are going to virtualize. Such messages are analyzed, filtered and recorded. 
    /// Monitor message are also used to analyze original message, for example to create 
    /// request x response couples.
    /// </summary>
    public class EmsListener : IJmsListener/*, IExceptionListener, IMessageListener */{

        //private static readonly ILog Log = LogManager.GetLogger(typeof(EmsListener));

        // subscribing objects
        private Connection connection = null;
        private Session session = null;
        private MessageConsumer msgConsumer = null;
        private Destination destination = null;
        private bool subscribing;
        private bool configured;
        private IMessageListener messageListener = null;
        private IExceptionListener exceptionListener = null;

        // subscribing parameters
        private string serverUrl, userName, password, destinationName;
        private bool isTopic;

        

        private void Subscribe() {
            if (subscribing) {
                //L10nMessage message = JmsTibcoAgentMessages.NewEmsListenerAlreadySubscribing().Arg(destination.ToString());
                Console.Error.WriteLine("EmsListenerAlreadySubscribing");
                //throw message.AttachTo(new Exception(message));
            }
            if (!configured) {
                //L10nMessage message = JmsTibcoAgentMessages.NewNotConfiguredException();
                Console.Error.WriteLine("NotConfiguredException");
                //throw message.AttachTo(new ConfigurationException());
            }
            try {
                ConnectionFactory factory = new TIBCO.EMS.ConnectionFactory(serverUrl);

                // create the connection
                connection = factory.CreateConnection(userName, password);

                // create the session
                session = connection.CreateSession(false, Session.AUTO_ACKNOWLEDGE);

                // set the exception listener
                connection.ExceptionListener = exceptionListener;

                // create the destination
                if (isTopic)
                    destination = session.CreateTopic(destinationName);
                else
                    destination = session.CreateQueue(destinationName);

                /*if (Log.IsDebugEnabled) {
                    Log.Debug(JmsTibcoAgentMessages.NewSubcribingDestination().Arg(destinationName));    
                }*/

                // create the consumer
                msgConsumer = session.CreateConsumer(destination);

                // set the message listener
                msgConsumer.MessageListener = messageListener;
                
                // start the connection
                connection.Start();

                // set flag that it is subscribing
                subscribing = true;

                // Note: when message callback is used, the session
                // creates the dispatcher thread which is not a daemon
                // thread by default. Thus we can quit this method however
                // the application will keep running. It is possible to
                // specify that all session dispatchers are daemon threads.
            } catch (EMSException e) {
                Console.Error.WriteLine(e);
                //Log.Error(JmsTibcoAgentMessages.NewSubscribingException(), e);
                throw;
            }
        }

        private void UnSubscribe() {
            msgConsumer.Close();
            connection.Close();
            subscribing = false;
        }


        #region Implementation of IJmsListener

        public void Configure(string serverUrl, string userName, string password, string destinationName, bool isTopic) {
            if (subscribing) {
                /*L10nMessage message = JmsTibcoAgentMessages.NewConfigurationDuringSubscribingException();
                throw message.AttachTo(new ConfigurationException());*/
                Console.Error.WriteLine("ConfigurationDuringSubscribingException");
            }
            this.serverUrl = serverUrl;
            this.userName = userName;
            this.password = password;
            this.destinationName = destinationName;
            this.isTopic = isTopic;
            configured = true;
        }

        public void Configure(string serverUrl, string userName, string password, string destinationName, bool isTopic,
                              IMessageListener messageListener, IExceptionListener exceptionListener) {
            Configure(serverUrl, userName, password, destinationName, isTopic);
            this.messageListener = messageListener;
            this.exceptionListener = exceptionListener;
        }

        public void StartListening() {
            Subscribe();
        }

        public void StopListening() {
            UnSubscribe();
        }

        #endregion
    }
}