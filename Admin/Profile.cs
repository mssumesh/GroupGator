using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace Gator
    {
    public class Profile
        {
        public string uname;
        public string upass;
        public int gpref;
        public int  idelay;
        public int fdelay;
        
        public string gatoremail;
        public string gatorpass;
        public string sessid;
        public int ntotalinvite;
        public int ntotalblacklist;
        public int miph;
        public bool israndomdelayon;
        public int maxwait;
        
        
        public Player player;

        public Profile()
            {
            uname = "Steam Username";
            upass = "Steam Password";
            gpref = 6;
            idelay = 18;
            fdelay = 15;
            israndomdelayon = false;
            miph = 100;
            gatoremail = string.Empty;
            gatorpass = string.Empty;
            ntotalblacklist = 0;
            ntotalinvite = 0;
            }


        public void Init(string usr, Constants.PlayerInitType type)
            {
            player = new Player(usr, type);
            player.GetDetails();
            }
        //    Thread thUpd;
        //    ThreadStart thUpdStart;
        //    thUpdStart = new ThreadStart(UpdateProc);
        //    thUpd = new Thread(thUpdStart);
        //    thUpd.IsBackground = true;
        //    thUpd.Start();
        //    }
        //private void UpdateProc()
        //    {
        //    do
        //        {
        //        System.Threading.Thread.Sleep(15000);
        //        player.GetDetails();
        //        } while (true);
        //    }
        public void SaveConfig()
            {
            
            string buff = string.Empty;
            string gatherpref = string.Empty;
            gatherpref = gpref.ToString();

            
            buff = buff + "<STNAME>" + uname + "</STNAME>";
            buff = buff + "<STPASS>" + upass + "</STPASS>";
            buff = buff + "<GPREF>" + gatherpref + "</GPREF>";
            buff = buff + "<IWAIT>" + idelay.ToString() + "</IWAIT>";
            buff = buff + "<FWAIT>" + fdelay.ToString() + "</FWAIT>";
            if ( israndomdelayon == true )
                buff = buff + "<RNDFAILON>0</RNDFAILON>";
            else
                buff = buff + "<RNDFAILON>1</RNDFAILON>";
            buff = buff + "<MIPH>" + miph.ToString() +"</MIPH>";
            buff = buff + "<BLTOTAL>" + ntotalblacklist.ToString() + "</BLTOTAL>";
            buff = buff + "<GGNAME>" + gatoremail + "</GGNAME>";
            buff = buff + "<GGPASS>" + gatorpass + "</GGPASS>";
            buff = buff + "<SESSID>" + sessid + "</SESSID>";
            buff = buff + "<INVTOTAL>" + ntotalinvite.ToString() + "</INVTOTAL>";
            buff = buff + "<BLTOTAL>" + ntotalblacklist.ToString() + "</BLTOTAL>";

            GGDisk disk = new GGDisk();
            disk.Write(buff, disk.filecfg);
            }
        public void LoadConfig ()
            {
            
            GGDisk disk = new GGDisk();
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            string buff = disk.Read(disk.filecfg);
            string stridelay = string.Empty;
            string strfdelay = string.Empty;
            string strmiph = string.Empty;

            if (buff.Length != 0)
                {
                string invitedelay = string.Empty;
                string invitefaildelay = string.Empty;
                string gatherpref = string.Empty;
                
                uname = utils.GetStrBetween(buff, "<STNAME>", "</STNAME>");
                upass = utils.GetStrBetween(buff, "<STPASS>", "</STPASS>");
                gatherpref = utils.GetStrBetween(buff, "<GPREF>", "</GPREF>");
                stridelay = utils.GetStrBetween(buff, "<IWAIT>", "</IWAIT>");
                strfdelay = utils.GetStrBetween(buff, "<FWAIT>", "</FWAIT>");
                string tmp = utils.GetStrBetween(buff, "<RNDFAILON>", "</RNDFAILON>");
                if (tmp == "0")
                    israndomdelayon = false;
                else
                    israndomdelayon = true;
                strmiph = utils.GetStrBetween(buff, "<MIPH>", "</MIPH>");
                gatoremail = utils.GetStrBetween(buff, "<GGNAME>", "</GGNAME>");
                gatorpass = utils.GetStrBetween(buff, "<GGPASS>", "</GGPASS>");
                sessid = utils.GetStrBetween(buff, "<SESSID>", "</SESSID>");
                string nti = utils.GetStrBetween(buff, "<INVTOTAL>", "</INVTOTAL>"); ;
                string ntbl = utils.GetStrBetween(buff, "<BLTOTAL>", "</BLTOTAL>"); ;


                try
                    {
                    ntotalblacklist = Convert.ToInt32(ntbl);
                    ntotalinvite = Convert.ToInt32(nti);
                    }
                catch (Exception ex)
                    {
                    ntotalinvite = 0;
                    ntotalblacklist = 0;
                    }

                if (gatherpref == string.Empty)
                    gatherpref = "6";
                try
                    {
                    gpref = Convert.ToInt32(gatherpref);
                    }
                catch (Exception ex)
                    {
                    gpref = 6;
                    }
                try
                    {
                    miph = Convert.ToInt32(strmiph);
                    }
                catch (Exception ex)
                    {
                    miph = 100;
                    }

                try
                    {
                    idelay = Convert.ToInt32(stridelay);
                    
                    }
                catch (Exception ex)
                    {
                    idelay = 18;
                    }

                try
                    {
                    fdelay = Convert.ToInt32(strfdelay);
                    }
                catch (Exception ex)
                    {
                    fdelay = 0;
                    }


                
                }
            }
        }
    }
