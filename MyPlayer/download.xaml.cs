using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net;
using System.IO;

namespace MyPlayer
{
    /// <summary>
    /// download.xaml 的交互逻辑
    /// </summary>
    public partial class download : Window
    {
        public static string DOWN_URL { set; get; }
        public static string MUSIC_NAME { set; get; }

        public download()
        {
            InitializeComponent();
            VersionInfo.Content = Utils.APPNAME + Utils.VERSION + "Branch:" + Utils.BRANCH;
        }

        public void Download()
        {
            if (!Utils.getMediaPath().Equals("none"))
            {
                tip.Content = "启动下载引擎...";
                mDownload(Utils.getMediaPath(), DOWN_URL);
            }
            else
            {
                MessageBox.Show("未设置音乐存放目录");
                this.Close();
            }
        }

        public void mDownload(String path, String source)
        {
            WebRequest request = WebRequest.Create(source);
            WebResponse respone;
            try
            {
                respone = request.GetResponse();
            }
            catch (WebException) 
            {
                MessageBox.Show("连接服务器失败，请检查网络连接并重试");
                this.Close();
                return;
            }
            pgbar.Maximum = respone.ContentLength;
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Stream netStream = respone.GetResponseStream();
                Stream fileStream = new FileStream(path + @"\" + MUSIC_NAME, FileMode.Create);
                byte[] read = new byte[1024];
                long progressBarValue = 0;
                int realReadLen = netStream.Read(read, 0, read.Length);
                while (realReadLen > 0)
                {
                    fileStream.Write(read, 0, realReadLen);
                    progressBarValue += realReadLen;
                    pgbar.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);
                    realReadLen = netStream.Read(read, 0, read.Length);
                }
                netStream.Close();
                fileStream.Close();

            }, null);

        }

        public delegate void ProgressBarSetter(double value);
        public void SetProgressBar(double value)
        {
            tip.Content = "正在下载" + MUSIC_NAME + "..." + ((value / pgbar.Maximum) * 100).ToString("0.00") + "%";
            pgbar.Value = value;
            if ((value / pgbar.Maximum) * 100 == 100)
            {
                tip.Content = "启动播放器...";
                Thread.Sleep(500);
                //System.Windows.Forms.Application.Restart();

                //System.Reflection.Assembly.GetEntryAssembly();
                //string startpath = System.IO.Directory.GetCurrentDirectory();
                //System.Diagnostics.Process.Start(startpath + "\\MyPlayer.exe");
                MainWindow win = new MainWindow();
                win.Show();
                win.Play(MUSIC_NAME);

                //Application.Current.Shutdown();
                this.Close();
            }
        }


        //public string getFileNameByUrl(string url)
        //{
        //    // Format for http://music.163.com/song/media/outer/url?id=36990266.mp3
        //    string callback = url.Split('?')[1].Split('=')[1];
        //    return callback;
        //}
        


        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }
        public void CloseWindow(object sender, RoutedEventArgs args)
        {
            this.Close();
        }
    }
}
