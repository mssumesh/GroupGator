using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gator2
    {
    public partial class About : Form
        {
        string cvers = string.Empty;

        public About( string cv)
            {
            InitializeComponent();
            cvers = cv;
            }

        private void button1_Click(object sender, EventArgs e)
            {
            this.Close();
            }

        private void button2_Click(object sender, EventArgs e)
            {
            System.Diagnostics.Process.Start("http://groupgator.enjin.com");
            }

        private void About_Load(object sender, EventArgs e)
            {
            label3.Text = "V"+cvers;
            }
        }
    }