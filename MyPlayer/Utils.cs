using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlayer
{
    class Utils
    {

        public Utils()
        {
        }

        public static String APPNAME = " Circle Player ";
        public static String VERSION = " 2.1.0a ";
        public static String BRANCH = " Release ";


        public static string getMediaPath()
        {
            String callback = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            if (callback != "")
                return callback;
            else
            {
                return "none";
            }
        }

    }
}
