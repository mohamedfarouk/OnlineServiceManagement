using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineServiceManagement
{
    public class WebLogger
    {
        public static Logger Logger
        {
            get
            {
                return LogManager.GetLogger("AOneWebAdmin");
            }
        }
    }
}