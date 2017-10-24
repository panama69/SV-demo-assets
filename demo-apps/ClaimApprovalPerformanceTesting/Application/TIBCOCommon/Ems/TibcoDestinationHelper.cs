using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HP.SOAQ.ServiceVirtualization.Common.Util;
using HP.SOAQ.ServiceVirtualization.MessageModel.MetadataTypes;

namespace HP.SOAQ.ServiceVirtualization.JmsTibcoAgent.Common {
    public static class TibcoDestinationHelper {
        
        /// <summary>
        /// Decode unified destination name such as "Topic[topic.input]" to destination name "topic.input" and destination type "Topic".
        /// </summary>
        /// <param name="encodedDestination"></param>
        /// <param name="destinationName"></param>
        /// <param name="destinationType"></param>
        public static void DecodeDestinationNameAndType(string encodedDestination, out string destinationName, out string destinationType) {
            char[] splitBy = new[] { '[', ']' };
            string[] destinationParts = encodedDestination.Split(splitBy, 3);
            Validation.IsTrue(destinationParts.Count() == 3, "Invalid encoded destination name: " + encodedDestination);
            destinationType = destinationParts[0];
            destinationName = destinationParts[1];
            Validation.NotNullOrEmpty(destinationName, "Jms destination name can not be empty.");
            Validation.NotNullOrEmpty(destinationType, "Jms destination type can not be empty");
        }

        /// <summary>
        /// Encode given destination name and type to unified destination name.
        /// Example topic name "topic.input" encode to "Topic[topic.input]"
        /// </summary>
        /// <param name="destinationName"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static String EncodeDestinationNameAndType(string destinationName, string destinationType) {
            Validation.NotNullOrEmpty(destinationName, "Jms destination name can not be empty.");
            Validation.NotNullOrEmpty(destinationType, "Jms destination type can not be empty");
            // try to format topic unified destination name
            if (destinationType.ToLowerInvariant().Equals(JmsMetadata.DestinationTypeValues.Topic.ToLowerInvariant())) {
                return JmsMetadata.DestinationTypeValues.Topic + "[" + destinationName + "]";
            }

            // try to format topic unified queue name
            if (destinationType.ToLowerInvariant().Equals(JmsMetadata.DestinationTypeValues.Queue.ToLowerInvariant())) {
                return JmsMetadata.DestinationTypeValues.Queue + "[" + destinationName + "]";
            } 

            throw new ApplicationException("Invalid destination type:" + destinationType);
        }
    }
}
