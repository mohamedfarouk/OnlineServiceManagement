using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineServiceManagement.Models
{
    public class WinService
    {
        public string Name { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string CSSClass { get; set; }
        public string Status { get; set; }
    }

    public class WindowsServicesModel
    {
        public List<WinService> Services { get; set; }
        public WindowsServicesModel(String Identification)
        {
            LoadServices(Identification);
        }

        private void LoadServices(String Identification)
        {
            try
            {
                Services = new List<WinService>()
                {
                    new WinService
                    {
                        Name = "ASP.NET State Service",
                        ServiceName = "aspnet_state",
                        Description="Provides support for out-of-process session states for ASP.NET. If this service is stopped, out-of-process requests will not be processed. If this service is disabled, any services that explicitly depend on it will fail to start.",
                        CSSClass = "one",
                        Status= ""
                    },
                    new WinService
                    {
                        Name = "Background Intelligent Transfer Service",
                        ServiceName = "BITS",
                        Description="Transfers files in the background using idle network bandwidth. If the service is disabled, then any applications that depend on BITS, such as Windows Update or MSN Explorer, will be unable to automatically download programs and other information.",
                        CSSClass = "two",
                        Status= ""
                    },
                    new WinService
                    {
                        Name = "Bluetooth Support Service",
                        ServiceName = "bthserv",
                        Description="The Bluetooth service supports discovery and association of remote Bluetooth devices.  Stopping or disabling this service may cause already installed Bluetooth devices to fail to operate properly and prevent new devices from being discovered or associated.",
                        CSSClass = "three",
                        Status= ""
                    }
                };

                foreach (WinService winService in Services)
                {
                    WinServiceManger.ServiceControllerStatus status = Global.WinServiceMangerClient.GetServiceStatus(Identification, winService.ServiceName);
                    switch (status)
                    {
                        case WinServiceManger.ServiceControllerStatus.Running:
                            winService.Status = "Running";
                            break;
                        case WinServiceManger.ServiceControllerStatus.ContinuePending:
                            winService.Status = "Continue Pending";
                            break;
                        case WinServiceManger.ServiceControllerStatus.Paused:
                            winService.Status = "Paused";
                            break;
                        case WinServiceManger.ServiceControllerStatus.PausePending:
                            winService.Status = "Pause Pending";
                            break;
                        case WinServiceManger.ServiceControllerStatus.StartPending:
                            winService.Status = "Start Pending";
                            break;
                        case WinServiceManger.ServiceControllerStatus.Stopped:
                            winService.Status = "Stopped";
                            break;
                        default:
                            winService.Status = "Unknown";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WebLogger.Logger.ErrorException("WindowsServicesModel.LoadServices Exception : ", ex);
            }
        }
    }
}