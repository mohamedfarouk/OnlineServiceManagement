using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinServiceManger.Behaviour;

namespace WinServiceManger
{
    public partial class WinServiceMangerHost : ServiceBase
    {
        ServiceHost host = null;
        protected Thread m_thread = null;

        public WinServiceMangerHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            if (host != null)
            {
                host.Close();
            }

            host = new ServiceHost(typeof(WinServiceManger.Behaviour.WinServiceProvider));
            host.Open();

            m_thread = new Thread(new ThreadStart(MonitorServiceStatus));
            m_thread.Start();

            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            
        }

        protected override void OnStop()
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            if (host != null)
            {
                host.Close();
                host = null;
            }

            m_eventIsShuttingDown.Set();
            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        ManualResetEvent m_eventIsShuttingDown = new ManualResetEvent(false);

        public void MonitorServiceStatus()
        {
            ServiceLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);

            do
            {
              //Verify if status changed
                string serviceName = WinServiceProvider.HasServiceStatusChanged();
                if(serviceName != string.Empty)
                {
                    ServiceLogger.Logger.Info("Notifying all clients of change in service status");
                    WinServiceProvider.NotifyClients(serviceName);
                    ServiceLogger.Logger.Info("Resetting service status flags");
                    WinServiceProvider.ResetStatuses();
                }

            } while (!m_eventIsShuttingDown.WaitOne(1000));

            ServiceLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);

            m_eventIsShuttingDown.Set();

        }

         
    }
}
