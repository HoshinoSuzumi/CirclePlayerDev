using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Forms = System.Windows.Forms;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

#pragma warning disable CS0618 // 类型或成员已过时
namespace MyPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isPlaying = false;
        public bool isSnack = true;
        MediaPlayer player = new MediaPlayer();
        DispatcherTimer timer = new DispatcherTimer();

        List<Datas> datas = new List<Datas>();

        System.DateTime currentTime = System.DateTime.Now; 

        private WindowState ws;
        private WindowState wsl;
        NotifyIcon notifyIcon;


        public MainWindow()
        {
            InitializeComponent();
            start();
            title_info.Content = Utils.VERSION;
            VersionInfo.Content = Utils.APPNAME + Utils.VERSION + "Branch:" + Utils.BRANCH;
            vol_bar.Maximum = 1;
            player.Volume = 1;
            timer.Tick += new EventHandler(time_Tick);
            timer.Interval = TimeSpan.FromSeconds(1.0);
            var files = Directory.GetFiles(getMediaPath(), "*.mp3");

            foreach (var file in files)
            {
                Datas buf = new Datas();
                buf.FileName = new FileInfo(file).Name;
                buf.LastDate = new FileInfo(file).LastWriteTime.ToString("F");

                datas.Add(buf);
            }
            mlistView.ItemsSource = datas;
        }

        private void start()
        {
            this.notifyIcon = new NotifyIcon();
            //this.notifyIcon.BalloonTipText = "欢迎使用 Circle Player";
            int hours = currentTime.Hour;
            if (hours >= 19)
                this.notifyIcon.BalloonTipText = "晚上好";
            else if (hours >= 13)
                this.notifyIcon.BalloonTipText = "下午好";
            else if (hours >= 11)
                this.notifyIcon.BalloonTipText = "中午好";
            else if (hours >= 9)
                this.notifyIcon.BalloonTipText = "上午好";
            else if (hours >= 6)
                this.notifyIcon.BalloonTipText = "早上好";
            else if (hours >= 0)
                this.notifyIcon.BalloonTipText = "夜深了";
            else
                this.notifyIcon.BalloonTipText = "欢迎使用 Circle Player";
            this.notifyIcon.Text = "圆心播放器" + Utils.VERSION;//最小化到托盘时，鼠标悬停时显示的文本
            this.notifyIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory+@"\resource\icon.ico");//程序图标*
            this.notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
            this.notifyIcon.ShowBalloonTip(1000);
        }

        private void openLink(String url)
        {
            //调用系统默认的浏览器 
            System.Diagnostics.Process.Start(url);
        }

        public string getConfig(String key)
        {
            String callback = System.Configuration.ConfigurationSettings.AppSettings[key];
            if (callback != "")
                return callback;
            else
            {
                showTips(3,"Circle Player 警告","配置字段不存在");
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public string getMediaPath()
        {
            String callback = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            if (callback != "")
                return callback;
            else
            {
                mContentControl.Content = new SettingMediaPathWin();
                showTips(1, "Circle Player 提示", "请设置音乐存放目录");
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        String temp_file;
        

        public void Play(String File)
        {
            timer.Stop();
            player.Stop();
            var uri = new Uri(getMediaPath() + "/" + File);
            player.Open(uri);
            player.Play();
            btn_play.Content = "暂停";
            isPlaying = true;
            temp_file = File;
            title_info.Content = "正在播放 " + File;
            player.MediaEnded += player_MediaEnded;
            timer.Start();
        }

        //int temp_index;
        void player_MediaEnded(object sender, EventArgs e)
        {
            player.Stop();
            //foreach (Datas aFile in datas)
            //{
            //    int idx = datas.IndexOf(aFile);//获取当前aFile的索引 
            //    if (aFile.FileName.Equals(temp_file))
            //    {
            //        temp_index = idx;
            //    }
            //}
            //var uri = new Uri(getMediaPath() + "/" + datas[temp_index+1].FileName);
            //player.Open(uri);
            player.Play();
        }
        public class Datas
        {
            public String FileName { set; get; }
            public String LastDate { set; get; }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                player.Pause();
                title_info.Content = "已暂停";
                btn_play.Content = "播放";
                isPlaying = false;
            }
            else
            {
                player.Play();
                title_info.Content = "正在播放 " + temp_file;
                btn_play.Content = "暂停";
                isPlaying = true;
            }
        }
        
        public void showTips(int type, String title, String text)
        {
            switch (type)
            {
                case 1:
                    this.notifyIcon.BalloonTipIcon = Forms.ToolTipIcon.Info;
                    break;
                case 2:
                    this.notifyIcon.BalloonTipIcon = Forms.ToolTipIcon.Warning;
                    break;
                case 3:
                    this.notifyIcon.BalloonTipIcon = Forms.ToolTipIcon.Error;
                    break;
                case 4:
                    this.notifyIcon.BalloonTipIcon = Forms.ToolTipIcon.None;
                    break;
                default:
                    break;
            };
            this.notifyIcon.BalloonTipTitle = title;
            this.notifyIcon.BalloonTipText = text;
            this.notifyIcon.ShowBalloonTip(1000);
        }
        

        /* Ender codes */
        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            this.DragMove();
        }
        public void CloseWindow(object sender, RoutedEventArgs args)
        {
            if (player != null)
            {
                player.Stop();
                player.Close();
                player = null;
            }
            this.Close();
        }
        public void MinWindow(object sender, RoutedEventArgs args)
        {
            showTips(1, "Circle Player 隐藏到这里啦!", "随时恭候 Master 回来");
            this.Hide();
        }
        public void InfoWindow(object sender, RoutedEventArgs args)
        {
            mContentControl.Content = new InfoPage();
            //MessageBox.Show(Utils.APPNAME + "\n版本号：" + Utils.VERSION + "\n分支：" + Utils.BRANCH);
        }
        public void time_Tick(object sender, EventArgs e)
        {
            try
            {
                progress_bar.Maximum = ((int)player.NaturalDuration.TimeSpan.TotalSeconds); //总时长
            }
            catch
            {
                MessageBox.Show("[DEBUG]捕捉到非致命异常（您仍然可以继续操作）");
                showTips(2, "Circle Player 遇到了异常", "尽管这可能不影响使用，但可能会影响用户体验。烦请您将详细情况反馈给我们!");
                progress_bar.Maximum = ((int)player.NaturalDuration.TimeSpan.TotalSeconds); //总时长
            }
            progress_bar.Value = (player.Position.Ticks / 10000000);
        }

        private void mlistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            progress_bar.Value = 0;
            var selectData = mlistView.SelectedItem as Datas;
            Play(selectData.FileName);
        }

        private void progress_bar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                player.Position = new TimeSpan((long)(progress_bar.Value * 10000000));
            }
        }

        private void vol_bar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                player.Volume = vol_bar.Value;
            }
        }

        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            this.Show();
            WindowState = wsl;
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            ws = WindowState;
            if (ws == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mContentControl.Content = new SettingMediaPathWin();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            openLink("https://github.com/boxlab/CirclePlayer/issues");
            showTips(1,"Circle Player 反馈","正在启动浏览器前往圆心播放器 Github Issues");
            //MessageBox.Show("暂未开放");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SnackbarMessageQueue().Enqueue("Wow, easy!");
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }
    }
}
