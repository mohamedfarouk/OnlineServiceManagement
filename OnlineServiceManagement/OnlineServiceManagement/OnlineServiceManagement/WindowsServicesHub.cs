using SignalR;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineServiceManagement
{
    [HubName("windowsServiceManager")]
    public class WindowsServicesHub : Hub, WinServiceManger.WinServiceProviderServiceCallback
    {
        private static dynamic _clientsCache = null;

        public void ServiceStatusChanged(string ServiceName)
        {
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            updateClientStatuses(ServiceName);
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        public void updateClientStatuses(string serviceName)
        {
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                //var context = GlobalHost.ConnectionManager.GetHubContext<WindowsServicesHub>();

                var context = SignalR.GlobalHost.ConnectionManager.GetHubContext<WindowsServicesHub>();
                context.Clients.notifyChanges(serviceName);

                //if (Clients == null)
                //{
                //    //Clients = HttpContext.Current.Cache["Clients"];
                //    Clients = _clientsCache;
                //}
                ////var clients = GetClients();
                //if (Clients != null)
                //{
                //    Clients.serviceStatusChanged(serviceName);
                //}
                //Clients.displayAlert();
            }
            catch (Exception ex)
            {
                string c = ex.Message;
                WebLogger.Logger.Error(ex);
            }
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        public void performedServiceStatusChange(string serviceName)
        {
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                WebLogger.Logger.Info("Service '{0}'", serviceName);
                //var clients = GetClients();
                HttpContext.Current.Cache["Clients"] = Clients;
                Clients.serviceStatusChanged(serviceName);
                //Clients.displayAlert();
            }
            catch (Exception ex)
            {
                string c = ex.Message;
                WebLogger.Logger.Error(ex);
            }
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        public void registerMe()
        {
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                //var clients = GetClients();
                //HttpContext.Current.Cache["Clients"] = Clients;
                _clientsCache = Clients;
                //Clients.displayAlert();
            }
            catch (Exception ex)
            {
                string c = ex.Message;
                WebLogger.Logger.Error(ex);
            }
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
}