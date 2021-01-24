using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gator25
    {
    public partial class ToGroupDlg : Form
        {
        private string togrpsel;
        public ToGroupDlg()
            {
            InitializeComponent();
            }

        private void ToGroupDlg_Load(object sender, EventArgs e)
            {
            togrpsel = string.Empty;
            }
        public string Display()
            {
            togrpsel = string.Empty;
            System.Collections.ArrayList alist;// = new System.Collections.ArrayList();
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            Gator.GGDisk disk = new Gator.GGDisk();
            string buff = disk.Read(disk.filetogrps);
            alist = utils.GetTokensBetween(buff, "<GURL>", "</GURL>");
            for (int i = 0; i < alist.Count; i++)
                {
                if (!cboToGroup.Items.Contains(alist[i]))
                    cboToGroup.Items.Add(alist[i].ToString());
                }
            if (cboToGroup.Items.Count > 0)
                cboToGroup.SelectedIndex = 0;
            this.ShowDialog();
            return togrpsel;
            }

        private void btnInviteDlgContinue_Click(object sender, EventArgs e)
            {
            togrpsel = cboToGroup.Text;
            this.Close();
            }

        private void btnInviteDlgCancel_Click(object sender, EventArgs e)
            {
            togrpsel = string.Empty;
            this.Close();
            }
        }
    }