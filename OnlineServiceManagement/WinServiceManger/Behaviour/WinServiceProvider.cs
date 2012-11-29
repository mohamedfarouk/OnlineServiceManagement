using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WinServiceManger.Contract;

namespace WinServiceManger.Behaviour
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall)]
    public class WinServiceProvider : IWinServiceProvider
    {
        private static List<IWinServiceNotificationHandler> _callbackList = new List<IWinServiceNotificationHandler>();
        private static Dictionary<string, ServiceControllerStatus> _services = new Dictionary<string, ServiceControllerStatus>();
        public static readonly object _locker_ = new object();

        public string Ping()
        {
            return "I can ping";
        }

        private void RegisterClient(String Identification)
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name + "Identifiction as '" + Identification + "'");
            // Subscribe the guest to the beer inventory
            IWinServiceNotificationHandler client = OperationContext.Current.GetCallbackChannel<IWinServiceNotificationHandler>();
            if (!_callbackList.Contains(client))
            {
                _callbackList.Add(client);
            }
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void RegisterServiceNames(String ServiceName, ServiceControllerStatus Status)
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name + "for Service '" + ServiceName + "'");

            if (!_services.ContainsKey(ServiceName))
            {
                _services.Add(ServiceName, Status);
            }
            else
            {
                _services[ServiceName] = Status;
            }

            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        public static void ResetStatuses()
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                lock (_locker_)
                {
                    string[] serviceNames = _services.Keys.ToArray();
                    foreach (string serviceName in serviceNames)
                    {
                        ServiceController sc = ServiceController.GetServices().Where(s => s.ServiceName == serviceName).FirstOrDefault();
                        _services[serviceName] = sc.Status;
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Logger.Error(ex);
            }
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        public static void NotifyClients(String ServiceName)
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            ServiceLogger.Logger.Info("Found {0} callback clients", _callbackList.Count);
            try
            {
                _callbackList.ForEach(
                    delegate(IWinServiceNotificationHandler callback)
                    { callback.ServiceStatusChanged(ServiceName); });

                ResetStatuses();
            }
            catch (Exception ex)
            {
                ServiceLogger.Logger.ErrorException("Exception", ex);
            }
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        public static string HasServiceStatusChanged()
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            string retStatusChanged = string.Empty;
            try
            {
                lock (_locker_)
                {
                    string[] serviceNames = _services.Keys.ToArray();
                    foreach (string serviceName in serviceNames)
                    {
                        ServiceController sc = ServiceController.GetServices().Where(s => s.ServiceName == serviceName).FirstOrDefault();
                        if (sc != null)
                        {
                            if (_services[serviceName] != sc.Status)
                            {
                                ServiceLogger.Logger.Info("Status changed for '{0}'", serviceName);
                                retStatusChanged = serviceName;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Logger.Error(ex);
            }
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retStatusChanged;
        }

        public ServiceControllerStatus GetServiceStatus(String Identification, String ServiceName)
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name + "Identifiction as '" + Identification + "'");
            RegisterClient(Identification);
            ServiceControllerStatus retStatus = ServiceControllerStatus.StopPending;
            ServiceController sc = ServiceController.GetServices().Where(s => s.ServiceName == ServiceName).FirstOrDefault();
            if (sc != null)
            {
                retStatus = sc.Status;
            }
            RegisterServiceNames(ServiceName, sc.Status);
            //NotifyClients(ServiceName);
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retStatus;
        }

        public ServiceControllerStatus StartService(String Identification, String ServiceName)
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name + "Identifiction as '" + Identification + "'");
            RegisterClient(Identification);
            ServiceControllerStatus retStatus = ServiceControllerStatus.StopPending;
            ServiceController sc = ServiceController.GetServices().Where(s => s.ServiceName == ServiceName).FirstOrDefault();
            if (sc != null)
            {
                sc.Start();
                retStatus = sc.Status;
                //while (retStatus != ServiceControllerStatus.Running)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //}
            }
            RegisterServiceNames(ServiceName, sc.Status);
            //NotifyClients(ServiceName);
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retStatus;
        }

        public ServiceControllerStatus StopService(String Identification, String ServiceName)
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name + "Identifiction as '" + Identification + "'");
            RegisterClient(Identification);
            ServiceControllerStatus retStatus = ServiceControllerStatus.StopPending;
            ServiceController sc = ServiceController.GetServices().Where(s => s.ServiceName == ServiceName).FirstOrDefault();
            if (sc != null)
            {
                sc.Stop();
                retStatus = sc.Status;
                //while (retStatus != ServiceControllerStatus.Stopped)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //}
            }
            RegisterServiceNames(ServiceName, sc.Status);
            //NotifyClients(ServiceName);
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retStatus;
        }

        public ServiceControllerStatus RestartService(String Identification, String ServiceName)
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name + "Identifiction as '" + Identification + "'");
            RegisterClient(Identification);
            ServiceControllerStatus retStatus = ServiceControllerStatus.StopPending;
            ServiceController sc = ServiceController.GetServices().Where(s => s.ServiceName == ServiceName).FirstOrDefault();
            if (sc != null)
            {
                retStatus = StopService(Identification, ServiceName);
                retStatus = StartService(Identification, ServiceName);
            }
            RegisterServiceNames(ServiceName, sc.Status);
            //NotifyClients(ServiceName);
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retStatus;
        }
    }
}
