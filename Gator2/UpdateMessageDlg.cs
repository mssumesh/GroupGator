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
    public partial class UpdateMessageDlg : Form
        {
        
        Constants.UpdateResponse choice;
        Constants.UpdateMessageType type;
        string msg;

        public UpdateMessageDlg(string messg, Constants.UpdateMessageType typ)
            {
            InitializeComponent();
            type = typ;
            msg = messg;
            }
        public Constants.UpdateResponse Display()
            {
            this.ShowDialog();
            return choice;
            }

        private void UpdateMessage_Load(object sender, EventArgs e)
            {
            if (type == Constants.UpdateMessageType.Question )
                {
                btnOk.Visible = false;
                btnYes.Visible = true;
                btnNo.Visible = true;
                }
            else
                {
                btnOk.Visible = true;
                btnYes.Visible = false;
                btnNo.Visible = false;
                }            
            lblMessage.Text = msg;
            }

        private void btnOk_Click(object sender, EventArgs e)
            {
            choice = Constants.UpdateResponse.Ok;
            this.Close();
            }

        private void btnNo_Click(object sender, EventArgs e)
            {
            choice = Constants.UpdateResponse.No;
            this.Close();
            }

        private void btnYes_Click(object sender, EventArgs e)
            {
            choice = Constants.UpdateResponse.Yes;
            this.Close();
            }
        }
    }