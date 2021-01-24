using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;


namespace Gator
    {
    public class Donor : IPlugin
        {
        private string name = "Donor C-Panel";
        private string version = "1.0";
        Gator.DonorWindow pwin;
        public void Display()
            {
            //System.Windows.Forms.MessageBox.Show("hello from dll");
            pwin = new DonorWindow(name);
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
        //public void SetVars(string email, string passw, Gator.Constants.SoftLicense licns)
        //public void SetVars(string vars)
        //    {
        //    //MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //    //pwin.usr = utils.GetStrBetween(vars, "<EMAIL>", "</EMAIL>");
        //    //pwin.pass = utils.GetStrBetween(vars, "<PASS>", "</PASS>");
        //    //string tmp = utils.GetStrBetween(vars, "<LIC>", "</LIC>");
        //    //if (tmp == "0")
        //    //    pwin.lic = Constants.SoftLicense.Demo;
        //    //else
        //    //    pwin.lic = Constants.SoftLicense.Paid;
        //    }     
        }
    }
