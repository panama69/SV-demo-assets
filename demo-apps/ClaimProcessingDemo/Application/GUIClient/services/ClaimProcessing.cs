﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3603
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HP.SOAQ.ServiceSimulation.Demo
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Claim", Namespace="http://schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo")]
    public partial class Claim : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private float adjustedAmountField;
        
        private string approvalStatusField;
        
        private bool approvedField;
        
        private float claimedAmountField;
        
        private System.DateTime dateField;
        
        private string descriptionField;
        
        private string firstNameField;
        
        private string lastNameField;
        
        private HP.SOAQ.ServiceSimulation.Demo.MemberId memberIdField;
        
        private bool reimbursedField;
        
        private string socialSecurityNumberField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float adjustedAmount
        {
            get
            {
                return this.adjustedAmountField;
            }
            set
            {
                this.adjustedAmountField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string approvalStatus
        {
            get
            {
                return this.approvalStatusField;
            }
            set
            {
                this.approvalStatusField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool approved
        {
            get
            {
                return this.approvedField;
            }
            set
            {
                this.approvedField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float claimedAmount
        {
            get
            {
                return this.claimedAmountField;
            }
            set
            {
                this.claimedAmountField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string firstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string lastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public HP.SOAQ.ServiceSimulation.Demo.MemberId memberId
        {
            get
            {
                return this.memberIdField;
            }
            set
            {
                this.memberIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool reimbursed
        {
            get
            {
                return this.reimbursedField;
            }
            set
            {
                this.reimbursedField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string socialSecurityNumber
        {
            get
            {
                return this.socialSecurityNumberField;
            }
            set
            {
                this.socialSecurityNumberField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MemberId", Namespace="http://schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo")]
    public partial class MemberId : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long idField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClaimId", Namespace="http://schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo")]
    public partial class ClaimId : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long idField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MemberNotFoundFault", Namespace="http://schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo")]
    public partial class MemberNotFoundFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ReasonField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Reason
        {
            get
            {
                return this.ReasonField;
            }
            set
            {
                this.ReasonField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClaimNotFoundFault", Namespace="http://schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo")]
    public partial class ClaimNotFoundFault : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ReasonField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Reason
        {
            get
            {
                return this.ReasonField;
            }
            set
            {
                this.ReasonField = value;
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConfigurationName="IClaimProcessing", SessionMode=System.ServiceModel.SessionMode.NotAllowed)]
public interface IClaimProcessing
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/enterClaim", ReplyAction="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/enterClaimResp" +
        "onse")]
    [System.ServiceModel.FaultContractAttribute(typeof(HP.SOAQ.ServiceSimulation.Demo.MemberNotFoundFault), Action="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/enterClaimMemb" +
        "erNotFoundFaultFault", Name="MemberNotFoundFault", Namespace="http://schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo")]
    HP.SOAQ.ServiceSimulation.Demo.ClaimId enterClaim(HP.SOAQ.ServiceSimulation.Demo.Claim claim);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/listClaims", ReplyAction="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/listClaimsResp" +
        "onse")]
    HP.SOAQ.ServiceSimulation.Demo.ClaimId[] listClaims(HP.SOAQ.ServiceSimulation.Demo.MemberId memberId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/getClaim", ReplyAction="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/getClaimRespon" +
        "se")]
    [System.ServiceModel.FaultContractAttribute(typeof(HP.SOAQ.ServiceSimulation.Demo.ClaimNotFoundFault), Action="http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/getClaimClaimN" +
        "otFoundFaultFault", Name="ClaimNotFoundFault", Namespace="http://schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo")]
    HP.SOAQ.ServiceSimulation.Demo.Claim getClaim(HP.SOAQ.ServiceSimulation.Demo.ClaimId claimId);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IClaimProcessingChannel : IClaimProcessing, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class ClaimProcessingClient : System.ServiceModel.ClientBase<IClaimProcessing>, IClaimProcessing
{
    
    public ClaimProcessingClient()
    {
    }
    
    public ClaimProcessingClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public ClaimProcessingClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public ClaimProcessingClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public ClaimProcessingClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public HP.SOAQ.ServiceSimulation.Demo.ClaimId enterClaim(HP.SOAQ.ServiceSimulation.Demo.Claim claim)
    {
        return base.Channel.enterClaim(claim);
    }
    
    public HP.SOAQ.ServiceSimulation.Demo.ClaimId[] listClaims(HP.SOAQ.ServiceSimulation.Demo.MemberId memberId)
    {
        return base.Channel.listClaims(memberId);
    }
    
    public HP.SOAQ.ServiceSimulation.Demo.Claim getClaim(HP.SOAQ.ServiceSimulation.Demo.ClaimId claimId)
    {
        return base.Channel.getClaim(claimId);
    }
}
