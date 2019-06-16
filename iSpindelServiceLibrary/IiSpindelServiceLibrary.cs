using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace iSpindelServiceLibrary
{
    //[ServiceContract]
    //public interface ISampleService
    //{
    //    [OperationContract]
    //    string GreetMe(Person person);
    //}
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IiSpindelServiceLibrary
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }

    //[DataContract]
    //public class Person
    //{
    //    [DataMember]
    //    public string Name { get; set; }
    //}

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "iSpindelServiceLibrary.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    //public class iSpindelServiceLibrary : IiSpindelServiceLibrary
    //{
    //    public string GetValue(Int32 value)
    //    {
    //        return value.ToString();
    //    }
    //}
}
