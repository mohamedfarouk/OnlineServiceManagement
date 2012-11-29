using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServiceManger
{
    public class ServiceLogger
    {
        public static Logger Logger
        {
            get
            {
                return LogManager.GetLogger("WinServiceManger");
            }
        }
    }
}
