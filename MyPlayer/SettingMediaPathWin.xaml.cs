using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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
using System.Configuration;
using System.Threading;
using Forms = System.Windows.Forms;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace MyPlayer
{
    /// <summary>
    /// SettingMediaPathWin.xaml 的交互逻辑
    /// </summary>
    public partial class SettingMediaPathWin : UserControl
    {

        Utils utils = new Utils();
        NotifyIcon notifyIcon = new NotifyIcon();

        public SettingMediaPathWin()
        {
            InitializeComponent();
            VersionInfo.Content = Utils.APPNAME + Utils.VERSION + "Branch:" + Utils.BRANCH;
            this.notifyIcon = new NotifyIcon();
        }
        private void setMediaPath(string path)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径
            string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "MyPlayer.exe.config";
            doc.Load(strFileName);
            //找出名称为“add”的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性
                XmlAttribute att = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (att.Value == "MediaPath")
                {
                    //对目标元素中的第二个属性赋值
                    att = nodes[i].Attributes["value"];
                    att.Value = path;
                    break;
                }
            }
            //保存上面的修改
            doc.Save(strFileName);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                //MessageBox.Show(() ? "设置目录成功" : "设置失败");
                setMediaPath(openFileDialog.SelectedPath);
                Thread.Sleep(100);
                System.Windows.Forms.Application.Restart();
                Application.Current.Shutdown();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }
    }
}
