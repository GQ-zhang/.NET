using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace UtilityLibrary.Extend
{
    public class LogHelper
    {
        public static Logger Instance =LogManager.GetCurrentClassLogger();
    }
}
