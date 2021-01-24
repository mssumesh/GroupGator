namespace Gator25
    {
    partial class ToGroupDlg
        {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
            {
            if (disposing && (components != null))
                {
                components.Dispose();
                }
            base.Dispose(disposing);
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
            this.label1 = new System.Windows.Forms.Label();
            this.cboToGroup = new System.Windows.Forms.ComboBox();
            this.btnInviteDlgContinue = new System.Windows.Forms.Button();
            this.btnInviteDlgCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkOrange;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 86);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select to which group you wan to invite to, using this gather list; so tha" +
    "t we can avoid gathering user ids which might be already invited";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboToGroup
            // 
            this.cboToGroup.BackColor = System.Drawing.Color.Black;
            this.cboToGroup.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboToGroup.ForeColor = System.Drawing.Color.Snow;
            this.cboToGroup.FormattingEnabled = true;
            this.cboToGroup.Location = new System.Drawing.Point(14, 94);
            this.cboToGroup.Name = "cboToGroup";
            this.cboToGroup.Size = new System.Drawing.Size(265, 26);
            this.cboToGroup.TabIndex = 1;
            // 
            // btnInviteDlgContinue
            // 
            this.btnInviteDlgContinue.Location = new System.Drawing.Point(67, 121);
            this.btnInviteDlgContinue.Name = "btnInviteDlgContinue";
            this.btnInviteDlgContinue.Size = new System.Drawing.Size(84, 28);
            this.btnInviteDlgContinue.TabIndex = 2;
            this.btnInviteDlgContinue.Text = "CONTINUE";
            this.btnInviteDlgContinue.UseVisualStyleBackColor = true;
            this.btnInviteDlgContinue.Click += new System.EventHandler(this.btnInviteDlgContinue_Click);
            // 
            // btnInviteDlgCancel
            // 
            this.btnInviteDlgCancel.Location = new System.Drawing.Point(157, 121);
            this.btnInviteDlgCancel.Name = "btnInviteDlgCancel";
            this.btnInviteDlgCancel.Size = new System.Drawing.Size(84, 28);
            this.btnInviteDlgCancel.TabIndex = 3;
            this.btnInviteDlgCancel.Text = "CANCEL";
            this.btnInviteDlgCancel.UseVisualStyleBackColor = true;
            this.btnInviteDlgCancel.Click += new System.EventHandler(this.btnInviteDlgCancel_Click);
            // 
            // ToGroupDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(291, 158);
            this.Controls.Add(this.btnInviteDlgCancel);
            this.Controls.Add(this.btnInviteDlgContinue);
            this.Controls.Add(this.cboToGroup);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToGroupDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Please select your invite group";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ToGroupDlg_Load);
            this.ResumeLayout(false);

            }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboToGroup;
        private System.Windows.Forms.Button btnInviteDlgContinue;
        private System.Windows.Forms.Button btnInviteDlgCancel;
        }
    }