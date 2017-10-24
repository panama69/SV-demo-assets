namespace HP.SOAQ.ServiceVirtualization.MessageModel.MetadataTypes {
    /// <summary>
    /// Metadata related to JMS.
    /// </summary>
    public abstract class JmsMetadata {

        public const string JmsHeadersNodeName = "JmsHeaders";

        public class HeaderNames {
            // mandatory headers
            public const string MessageId = "MessageID";
            public const string Destination = "Destination";
            public const string ReplyTo = "ReplyTo";
            public const string DeliveryMode = "DeliveryMode";
            public const string Redelivered= "Redelivered";
            public const string CorellationId = "CorrelationID";
            public const string MsgType = "MsgType";
            public const string Timestamp = "Timestamp";
            public const string Expiration = "Expiration";
            public const string Priority = "Priority";

            // properties
            public const string ContentType = "Content_Type";
            public const string SoapAction = "SoapAction";
            public const string MimeVersion = "Mime_Version";
        }

        public class DestinationTypeValues {
            public const string Queue = "Queue";
            public const string Topic = "Topic";
        }

        public class MessageFormatValues {
            public const string Text = "Text";
            public const string Byte = "Byte";
        }
    }
}
