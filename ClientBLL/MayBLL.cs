using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBLL
{
    public class MayBLL
    {
        public static void TatMay()
        {
            Process.Start("shutdown", "/s /f /t 0");
        }
    }
}
