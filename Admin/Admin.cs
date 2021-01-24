using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;


namespace Gator
    {
    public class Admin : IPlugin
        {
        private string name = "Admin Control Panel";
        private string version = "1.0";
        public void Display()
            {
            //System.Windows.Forms.MessageBox.Show("hello from dll");
            Gator.AdminWindow pwin = new AdminWindow(name);
            pwin.ShowDialog();
            }
        public string Name()
            {
            return name;
            }
        public string Version()
            {
            return version;
            }

        }
    }