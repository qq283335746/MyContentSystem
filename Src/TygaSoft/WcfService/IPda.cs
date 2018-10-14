using System;
using System.IO;
using System.ServiceModel;
using TygaSoft.WcfModel;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "http://TygaSoft.Services.PdaService")]
    public interface IPda
    {
        [OperationContract(Name = "GetHelloWord")]
        string GetHelloWord();

        [OperationContract(Name = "ValidateUser")]
        string ValidateUser(string username, string password);
    }
}
