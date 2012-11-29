using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace OnlineServiceManagement
{
    public class Global
    {
        private static WinServiceManger.WinServiceProviderServiceClient _client;

        public static WinServiceManger.WinServiceProviderServiceClient WinServiceMangerClient
        {
            get 
            {
                return _client;
            }
        }

        public static void Initialise()
        {
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            _client = new WinServiceManger.WinServiceProviderServiceClient(new InstanceContext(new WindowsServicesHub()));
            //_client = new WinServiceManger.WinServiceProviderServiceClient(new InstanceContext());
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        public static void DeInitialise()
        {
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            _client.Close();
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
}