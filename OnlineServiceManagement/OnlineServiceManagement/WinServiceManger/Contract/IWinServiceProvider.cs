using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceProcess;

namespace WinServiceManger.Contract
{

    [ServiceContract]
    public interface IWinServiceNotificationHandler
    {
        [OperationContract(IsOneWay = true)]
        void ServiceStatusChanged(String ServiceName);
    }

     [ServiceContract(
        Name = "WinServiceProviderService",
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IWinServiceNotificationHandler))]
    public interface IWinServiceProvider
    {
        [OperationContract]
        string Ping();

        [OperationContract]
        ServiceControllerStatus GetServiceStatus(String Identification, String ServiceName);

        [OperationContract]
        ServiceControllerStatus StartService(String Identification, String ServiceName);

        [OperationContract]
        ServiceControllerStatus StopService(String Identification, String ServiceName);

        [OperationContract]
        ServiceControllerStatus RestartService(String Identification, String ServiceName);
    }
}
