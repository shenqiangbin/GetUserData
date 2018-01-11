using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProxyUrlApp
{
    public partial class Form2 : Form
    {
        private Timer timer = new Timer();

        public Form2()
        {
            InitializeComponent();

            timer.Tick += Timer_Tick; ;
            timer.Interval = 5 * 1000;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                // 在此处理最小化按钮被点击后的过程               
                this.WindowState = FormWindowState.Normal;
            }
        }
    }
}
