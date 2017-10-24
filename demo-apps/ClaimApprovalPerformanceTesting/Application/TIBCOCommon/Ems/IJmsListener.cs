using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JmsAgentCommon.JmsAgent {
    /// <summary>
    /// Listener represents component what listen on any destination, whatever it destination it is.
    /// It could be just one agent Jms destination, where client is reconfigured to send messages,
    /// or it could be set of system destinations for non-intrusive way. Both are declared by one
    /// destination name (for set of system destinations, wildcards can be used in name)
    /// For each listener new connection and session is created.
    /// </summary>
    public interface IJmsListener {

        /// <summary>
        /// Configure Jms listener. Jms listener has to be configured before it starts listening.
        /// It setups necessary parameters to subscribe Jms destination.
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="destinationName"></param>
        /// <param name="isTopic"></param>
        void Configure(string serverUrl, string userName, string password, string destinationName, bool isTopic);
        void StartListening();
        void StopListening();
    }
}