using QQHelper.core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QQHelper
{
    public partial class Form1 : Form
    {
        private QQExten _qq;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void btnQQLogin_Click(object sender, EventArgs e)
        {
            string qqNumber = "";
            string qqPwd = "";
            _qq = new QQExten();
        }
    }
}
