using System;
using System.Collections.Generic;
using System.Text;


namespace AdminPanel
    {
    public class AdminPanel : IPlugin
        {
        private string Name;
        private string Version;
        private bool loaded;
        private PlgMain win;
        
        public AdminPanel()
            {
            Name = "AdminPanel";
            Version = "1.0";
            loaded = false;
            win = new PlgMain();
            }

        public void Show()
            {
            if ( loaded == false)
                {
                win.Show();
                loaded = true;
                }
            }
        public void Move(System.Drawing.Point newloc)
            {
            if (loaded == true)
                {
                win.Location = newloc;
                }
            }
        public string GetName()
            {
            return Name;
            }
        public string GetVersion()
            {
            return Version;
            }
        public void Close()
            {
            if (loaded == true)
                {
                win.Close();
                loaded = false;
                }

            }

        }
    }
