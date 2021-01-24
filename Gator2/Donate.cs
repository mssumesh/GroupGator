using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Gator2
    {
    public partial class Donate : Form
        {
        public Donate()
            {
            InitializeComponent();
            }

        private void button1_Click(object sender, EventArgs e)
            {
            try
                {
                
                System.Diagnostics.Process.Start("http://groupgatorcommunity.net/login.php");
                this.Close();                   
                }
            catch (Exception ex)
                {
                ;
                }

            }

        private void button2_Click(object sender, EventArgs e)
            {
            this.Close();
            
            }
        }
    }