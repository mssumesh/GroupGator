namespace Gator25
    {
    partial class PayPalWindow
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
            this.wbPaypal = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // wbPaypal
            // 
            this.wbPaypal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbPaypal.Location = new System.Drawing.Point(0, 0);
            this.wbPaypal.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbPaypal.Name = "wbPaypal";
            this.wbPaypal.ScriptErrorsSuppressed = true;
            this.wbPaypal.Size = new System.Drawing.Size(912, 439);
            this.wbPaypal.TabIndex = 0;
            this.wbPaypal.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbPaypal_DocumentCompleted);
            // 
            // PayPalWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 439);
            this.Controls.Add(this.wbPaypal);
            this.Name = "PayPalWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading paypal. Please wait..";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PayPalWindow_Load);
            this.ResumeLayout(false);

            }

        #endregion

        private System.Windows.Forms.WebBrowser wbPaypal;
        }
    }