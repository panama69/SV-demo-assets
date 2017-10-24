using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HP.SOAQ.ServiceVirtualization.Common.Util;
using HP.SOAQ.ServiceVirtualization.MessageModel.MetadataTypes;
using TIBCO.EMS.ADMIN;

namespace TibcoCommon.Ems {
    class TibcoDestinationAdmin {

        public string ProviderUrl;
        public string Username;
        public string Password;
        public string DestinationName;
        public string DestinationType;
        public string ReplyToDestinationName;
        public string ReplyToDestinationType;
        public string AgentUsername;
        public string AgentPassword;

        public void PrepareDestinations() {
            Validation.NotNull(ProviderUrl);
            Validation.NotNull(Username);
            Validation.NotNull(Password);
            Validation.NotNull(DestinationName);
            Validation.NotNull(DestinationType);
            Validation.NotNull(ReplyToDestinationName);
            Validation.NotNull(ReplyToDestinationType);
            Validation.NotNull(AgentUsername);
            Validation.NotNull(AgentPassword);

            Admin tibcoAdmin = new Admin(ProviderUrl, AgentUsername, AgentPassword);
            DestinationInfo targetDestinationInfo = null;
            if (DestinationType.Equals(JmsMetadata.DestinationTypeValues.Topic)) {
                targetDestinationInfo = tibcoAdmin.GetTopic(DestinationName);
                if (targetDestinationInfo == null) {
                    try {
                        targetDestinationInfo = tibcoAdmin.CreateTopic(new TopicInfo(DestinationName));
                    } catch (AdminNameExistsException) {
                        // something (probably client) creates this destination faster
                        targetDestinationInfo = tibcoAdmin.GetTopic(DestinationName);
                    }
                }
            } else if (DestinationType.Equals(JmsMetadata.DestinationTypeValues.Queue)) {
                targetDestinationInfo = tibcoAdmin.GetQueue(DestinationName);
                if (targetDestinationInfo == null) {
                    try {
                        targetDestinationInfo = tibcoAdmin.CreateQueue(new QueueInfo(DestinationName));
                    } catch (AdminNameExistsException) {
                        // something (probably client) creates this destination faster
                        targetDestinationInfo = tibcoAdmin.GetTopic(DestinationName);
                    }
                }
            }

            DestinationInfo replyDestinationInfo = null;
            if (ReplyToDestinationType.Equals(JmsMetadata.DestinationTypeValues.Topic)) {
                replyDestinationInfo = tibcoAdmin.GetTopic(ReplyToDestinationName);
                if (replyDestinationInfo == null) {
                    try {
                        replyDestinationInfo = tibcoAdmin.CreateTopic(new TopicInfo(ReplyToDestinationName));
                    } catch (AdminNameExistsException) {
                        // something (probably client) creates this destination faster
                        replyDestinationInfo = tibcoAdmin.GetTopic(ReplyToDestinationName);
                    }
                }
            } else if (ReplyToDestinationType.Equals(JmsMetadata.DestinationTypeValues.Queue)) {
                replyDestinationInfo = tibcoAdmin.GetQueue(ReplyToDestinationName);
                if (replyDestinationInfo == null) {
                    try {
                        replyDestinationInfo = tibcoAdmin.CreateQueue(new QueueInfo(ReplyToDestinationName));
                    } catch (AdminNameExistsException) {
                        // something (probably client) creates this destination faster
                        replyDestinationInfo = tibcoAdmin.GetTopic(ReplyToDestinationName);
                    }
                }
            }

            UserInfo serviceUser = tibcoAdmin.GetUser(Username);
            if (serviceUser == null || (serviceUser.Equals(Password))) {
                serviceUser = tibcoAdmin.CreateUser(new UserInfo(Username));
            }
            serviceUser.Password = Password;
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
    }
}
