using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineServiceManagement.Models;
using System.ServiceModel;

namespace OnlineServiceManagement.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            Session["Identification"] = User.Identity.Name;
            WindowsServicesModel wsm = new WindowsServicesModel(Session["Identification"].ToString());
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return View(wsm);
        }

        [HttpPost]
        public ActionResult Index(WindowsServicesModel Model)
        {
            return View(Model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetWindowsServices()
        {
            JsonResult retValue = null;
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                if (Session["Identification"] != null)
                {
                    WindowsServicesModel wsm = new WindowsServicesModel(Session["Identification"].ToString());
                    retValue = new JsonResult()
                    {
                        Data = wsm
                    };
                }
            }
            catch (Exception ex)
            {
                WebLogger.Logger.ErrorException("Error ", ex);
            }
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retValue;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult StartService(string ServiceName)
        {
            JsonResult retValue = null;
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                if (Session["Identification"] != null)
                {
                    WinServiceManger.ServiceControllerStatus status = Global.WinServiceMangerClient.StartService(Session["Identification"].ToString(), ServiceName);
                    //if (status == WinServiceManger.ServiceControllerStatus.Running)
                    //{
                    WindowsServicesModel wsm = new WindowsServicesModel(Session["Identification"].ToString());
                    retValue = new JsonResult()
                    {
                        Data = wsm
                    };
                    //}
                }
            }
            catch (Exception ex)
            {
                WebLogger.Logger.ErrorException("Error ", ex);
            }
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retValue;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult StopService(string ServiceName)
        {
            JsonResult retValue = null;
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                WinServiceManger.ServiceControllerStatus status = Global.WinServiceMangerClient.StopService(Session["Identification"].ToString(), ServiceName);
                WindowsServicesModel wsm = new WindowsServicesModel(Session["Identification"].ToString());
                retValue = new JsonResult()
                {
                    Data = wsm
                };
            }
            catch (Exception ex)
            {
                WebLogger.Logger.ErrorException("Error ", ex);
            }
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retValue;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RestartService(string ServiceName)
        {
            JsonResult retValue = null;
            WebLogger.Logger.Info("Started " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                WinServiceManger.ServiceControllerStatus status = Global.WinServiceMangerClient.RestartService(Session["Identification"].ToString(), ServiceName);
                //if (status == WinServiceManger.ServiceControllerStatus.Running)
                //{
                WindowsServicesModel wsm = new WindowsServicesModel(Session["Identification"].ToString());
                retValue = new JsonResult()
                {
                    Data = wsm
                };
                //}
            }
            catch (Exception ex)
            {
                WebLogger.Logger.ErrorException("Error ", ex);
            }
            WebLogger.Logger.Info("Completed " + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return retValue;
        }

        //[HttpPost]
        //[ActionName("Post")]
        //[AcceptSubmitButton(Name = "btnStart", Value = "Start")]
        //public ActionResult Start(WindowsServicesModel Model)
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ActionName("Post")]
        //[AcceptSubmitButton(Name = "btnStop", Value = "Stop")]
        //public ActionResult Stop(WindowsServicesModel Model)
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ActionName("Post")]
        //[AcceptSubmitButton(Name = "btnRestart", Value = "Restart")]
        //public ActionResult Restart(WindowsServicesModel Model)
        //{
        //    return View();
        //}
    }
}
