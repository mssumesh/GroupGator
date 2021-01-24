using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gator;

namespace Gator2
    {
    public partial class frmDisclamer : Form
        {
        Constants.DislcaimerResponse resp;
        public frmDisclamer( )
            {
            InitializeComponent();           
            }

        private void button1_Click(object sender, EventArgs e)
            {
            resp = Constants.DislcaimerResponse.Agree;
            this.Close();           
            }

        private void button2_Click(object sender, EventArgs e)
            {
            resp = Constants.DislcaimerResponse.Decline; 
            this.Close();
            }

        public Constants.DislcaimerResponse Display()
            {
            this.ShowDialog();
            return resp;
            }        
        }
    }