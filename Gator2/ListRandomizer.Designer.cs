namespace Gator25
    {
    partial class ListRandomizer
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.thRander = new System.ComponentModel.BackgroundWorker();
            this.panBack = new System.Windows.Forms.Panel();
            this.panFore = new System.Windows.Forms.Panel();
            this.panBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 75);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(409, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Randomizing the gatherlist..";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // thRander
            // 
            this.thRander.WorkerReportsProgress = true;
            // 
            // panBack
            // 
            this.panBack.BackColor = System.Drawing.Color.Red;
            this.panBack.Controls.Add(this.panFore);
            this.panBack.Location = new System.Drawing.Point(16, 43);
            this.panBack.Name = "panBack";
            this.panBack.Size = new System.Drawing.Size(400, 22);
            this.panBack.TabIndex = 2;
            this.panBack.Visible = false;
            // 
            // panFore
            // 
            this.panFore.BackColor = System.Drawing.Color.Lime;
            this.panFore.Location = new System.Drawing.Point(-1, 0);
            this.panFore.Name = "panFore";
            this.panFore.Size = new System.Drawing.Size(10, 22);
            this.panFore.TabIndex = 4;
            this.panFore.Visible = false;
            // 
            // ListRandomizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 119);
            this.ControlBox = false;
            this.Controls.Add(this.panBack);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ListRandomizer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List Randomizer";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ListRandomizer_Load);
            this.panBack.ResumeLayout(false);
            this.ResumeLayout(false);

            }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker thRander;
        private System.Windows.Forms.Panel panBack;
        private System.Windows.Forms.Panel panFore;
        }
    }