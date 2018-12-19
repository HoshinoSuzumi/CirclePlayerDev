using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS0618 // 类型或成员已过时
namespace MyPlayer
{
    class Utils
    {

        public Utils()
        {
        }

        public static String APPNAME = " Circle Player ";
        public static String VERSION = " 2.1.1a ";
        public static String BRANCH = " Release ";


        public static string getMediaPath()
        {
            string callback = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            if (callback != "")
                return callback;
            else
            {
                return "none";
            }
        }

    }
}
