using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace iSpindelServiceLibrary
{
    public class MyEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            throw new NotImplementedException();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            throw new NotImplementedException();
        }
    }

    public class iSpindelServiceLibrary : IiSpindelServiceLibrary
    {
        //public static void Configure(ServiceConfiguration serviceConfiguration)
        //{
        //    ContractDescription contractDescription = new ContractDescription("iSpindelServiceLibrary", "iSpindelServiceLibrary");
        //    EndpointAddress endpointAddress = new EndpointAddress("localhost:iSpindelService");
        //    ServiceEndpoint serviceEndpoint = new ServiceEndpoint(contractDescription);
        //    serviceEndpoint.Address = endpointAddress;
        //    serviceEndpoint.Binding = new BasicHttpBinding();
        //    serviceEndpoint.EndpointBehaviors.Add(new MyEndpointBehaviour());
        //    serviceConfiguration.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
        //    serviceConfiguration.EnableProtocol(new BasicHttpBinding());
        //    serviceConfiguration.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
        //    serviceConfiguration.AddServiceEndpoint(serviceEndpoint);
        //}
        public static void Configure(ServiceConfiguration config)
        {
            ServiceEndpoint se = new ServiceEndpoint(new ContractDescription("IiSpindelServiceLibrary"), new BasicHttpBinding(), new EndpointAddress("http://localhost/iSpindelService"));
            se.Behaviors.Add(new MyEndpointBehavior());
            config.AddServiceEndpoint(se);

            config.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
            config.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });


            //ContractDescription contractDescription = new ContractDescription("IiSpindelServiceLibrary");
            ////EndpointAddress endpointAddress = new EndpointAddress("http://localhost/iSpindelService");
            //BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
            //basicHttpBinding.Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport };
            ////ServiceEndpoint serviceEndpoint = new ServiceEndpoint(contractDescription, new BasicHttpBinding(), endpointAddress);
            //config.AddServiceEndpoint(typeof(IiSpindelServiceLibrary), new BasicHttpBinding(), new Uri("http://localhost:35417/iSpindelService"));


        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
