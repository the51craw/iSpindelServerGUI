﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.9035
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iSpindelServiceLibrary
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompositeType", Namespace="http://schemas.datacontract.org/2004/07/iSpindelServiceLibrary")]
    public partial class CompositeType : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private bool BoolValueField;
        
        private string StringValueField;
        
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
        public bool BoolValue
        {
            get
            {
                return this.BoolValueField;
            }
            set
            {
                this.BoolValueField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue
        {
            get
            {
                return this.StringValueField;
            }
            set
            {
                this.StringValueField = value;
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IiSpindelServiceLibrary")]
public interface IiSpindelServiceLibrary
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IiSpindelServiceLibrary/GetData", ReplyAction="http://tempuri.org/IiSpindelServiceLibrary/GetDataResponse")]
    string GetData(int value);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IiSpindelServiceLibrary/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IiSpindelServiceLibrary/GetDataUsingDataContractResponse")]
    iSpindelServiceLibrary.CompositeType GetDataUsingDataContract(iSpindelServiceLibrary.CompositeType composite);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IiSpindelServiceLibraryChannel : IiSpindelServiceLibrary, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class IiSpindelServiceLibraryClient : System.ServiceModel.ClientBase<IiSpindelServiceLibrary>, IiSpindelServiceLibrary
{
    
    public IiSpindelServiceLibraryClient()
    {
    }
    
    public IiSpindelServiceLibraryClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public IiSpindelServiceLibraryClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public IiSpindelServiceLibraryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public IiSpindelServiceLibraryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string GetData(int value)
    {
        return base.Channel.GetData(value);
    }
    
    public iSpindelServiceLibrary.CompositeType GetDataUsingDataContract(iSpindelServiceLibrary.CompositeType composite)
    {
        return base.Channel.GetDataUsingDataContract(composite);
    }
}
