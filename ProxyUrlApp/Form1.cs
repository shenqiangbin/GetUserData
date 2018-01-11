using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProxyUrlApp
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class Form1 : Form
    {
        private Timer timer = new Timer();
        public Form1()
        {
            this.ClientSize = new System.Drawing.Size(1077, 643);
            ShellExecute(IntPtr.Zero, "open", "rundll32.exe", " InetCpl.cpl,ClearMyTracksByProcess 255", "", ShowCommands.SW_HIDE);
            InitializeComponent();
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.ObjectForScripting = this;
            this.webBrowser1.Navigated += WebBrowser1_Navigated;
            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;
            this.webBrowser1.NewWindow += WebBrowser1_NewWindow;
            while (webBrowser1.IsBusy) Application.DoEvents();
            timer.Tick += Timer_Tick;
            timer.Interval = 5 * 1000;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            btnStartMonitor.Text = $"监控中...{DateTime.Now.ToLongTimeString()}";
            HandleMonitor();
            // this.btnStartMonitor.PerformClick();
        }

        private void WebBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElement userNameEle = this.webBrowser1.Document.GetElementById("UserName");
            if (userNameEle != null)
                userNameEle.SetAttribute("value", ConfigurationManager.AppSettings["OAAccount"] ?? "");

            HtmlElement passwordEle = this.webBrowser1.Document.GetElementById("PassWord");
            if (passwordEle != null)
                passwordEle.SetAttribute("value", ConfigurationManager.AppSettings["OAPwd"] ?? "");


            //var mainFrame = webBrowser1.Document.Window.Frames["mainFrame"];
            //var text = webBrowser1.StatusText;
            //var test2 = webBrowser1.ReadyState;
            //if (mainFrame != null)
            //{
            //    if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            //        return;

            //    var m = mainFrame.Frames[0];
            //    HtmlElement ele = m.Document.GetElementById("DivP6");
            //    if (ele != null)
            //    {
            //        string s = ele.OuterHtml;
            //        if(!s.Contains("Processing"))
            //            MessageBox.Show(s); ;
            //    }
            //}           
        }

        private void WebBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {

        }

        private void HandleMonitor()
        {
            if (webBrowser1.Document != null && webBrowser1.Document.Window.Frames.Count != 0)
            {
                foreach (HtmlWindow item in webBrowser1.Document.Window.Frames)
                {
                    if (item.Name == "mainFrame")
                    {
                        if (item.Frames.Count == 0)
                            continue;

                        var m = item.Frames[0];
                        HtmlElement ele = m.Document.GetElementById("DivP6");
                        if (ele != null)
                        {
                            string s = ele.OuterHtml;
                            if (!s.Contains("Processing"))
                            {
                                if (s.Contains("javascript:PIONT_ReadMess"))
                                {
                                    this.timer.Stop();
                                    btnStartMonitor.Text = "监控停止，点我继续";
                                    if (this.WindowState == FormWindowState.Minimized)
                                    {
                                        this.Show();
                                        this.ShowInTaskbar = true;
                                        this.WindowState = FormWindowState.Normal;
                                        notifyIcon1.Visible = false;
                                    }                                    
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                //this.webBrowser1.Navigate("http://oa.cnki.net/TTKN/Default.html");
            }

            //HtmlElement ele = this.webBrowser1.Document.GetElementById("FormShield1");
        }

        private void btnStartMonitor_Click(object sender, EventArgs e)
        {
            //webBrowser1.Document.Cookie.Remove(0, (webBrowser1.Document.Cookie.Length - 1));
            timer.Start();
        }

        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            this.webBrowser1.GoBack();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1 * 1000);
            this.webBrowser1.Navigate(ConfigurationManager.AppSettings["url"]);
            this.SizeChanged += Form1_SizeChanged;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)  //判断是否最小化
            {
                notifyIcon1.Visible = true;
                this.Hide();
                this.ShowInTaskbar = false;

                Initializenotifyicon();
            }
        }

        private ContextMenu notifyiconMnu;

        #region 最小化到任务栏
        /// <summary>
        /// 最小化到任务栏
        /// </summary>
        private void Initializenotifyicon()
        {
            //定义一个MenuItem数组，并把此数组同时赋值给ContextMenu对象 
            MenuItem[] mnuItms = new MenuItem[3];
            mnuItms[0] = new MenuItem();
            mnuItms[0].Text = "显示窗口";
            mnuItms[0].Click += new System.EventHandler(this.notifyIcon1_showfrom);

            mnuItms[1] = new MenuItem("-");

            mnuItms[2] = new MenuItem();
            mnuItms[2].Text = "退出系统";
            mnuItms[2].Click += new System.EventHandler(this.ExitSelect);
            mnuItms[2].DefaultItem = true;

            notifyiconMnu = new ContextMenu(mnuItms);
            notifyIcon1.ContextMenu = notifyiconMnu;

            notifyIcon1.Click += notifyIcon1_DoubleClick;
            //notifyIcon1.Click += notifyIcon1_showfrom;
            //为托盘程序加入设定好的ContextMenu对象 
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = false;
            }
        }

        public void notifyIcon1_showfrom(object sender, System.EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = false;
            }
        }

        public void ExitSelect(object sender, System.EventArgs e)
        {
            //隐藏托盘程序中的图标 
            notifyIcon1.Visible = false;
            //关闭系统 
            this.Close();
            this.Dispose(true);
        }

        #endregion
    }

    public enum ShowCommands : int
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11,
        SW_MAX = 11
    }
}
