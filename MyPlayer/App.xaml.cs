using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyPlayer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public void OnAppStartup(object sender, StartupEventArgs e)
        {
            /* 
             * ccp://@down@http://music.163.com/song/media/outer/url?id=29947420.mp3@Fade.mp3
             * mArgs[0]: Protocol
             * mArgs[1]: Flag
             * mArgs[2]: DOWN_URL
             * mArgs[3]: MUSIC_NAME
             * 
             */
            string[] naive = e.Args;
            //MessageBox.Show((naive == e.Args).ToString());
            if (naive.Length != 0 && naive != null)
            {
                string[] mArgs;
                mArgs = naive[0].Split('@');
                download win = new download();
                //MessageBox.Show(mArgs.ToArray()[0] + "  &&  " + e.Args.ToArray()[1]);
                download.DOWN_URL = mArgs.ToArray()[2];
                download.MUSIC_NAME = System.Web.HttpUtility.UrlDecode(mArgs.ToArray()[3]);
                //StartupUri = new Uri("download.xaml", UriKind.Relative);
                win.Show();
                win.Download();
            }
            else 
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
            }
        }
    }
}
