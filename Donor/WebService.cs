using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Gator
    {
    class WebService
        {
        private string urlchkusg;
        private string urlupdusg;
        private string urlinout;
        private string urllogin;
        private string urlcaptchaverfiy;
        public string urlcaptchagen;
        public string urlcaptchabase;
        private string urlacheck;
        private string urladminunlock;
        private string urlgettime;
        private string urlunlocknormal;
        private string urladminaddtime;
        private string urladminsettime;
        private string urlactiveusers;
        private string urldonorgift;
        private string urlmyip;
        public string urlcreat;
        public string urlggptsqry;
        public string urlggpts;
        
        public bool queryingserver = false;
        public string mid;
        private FingerPrint fp;
        public struct ServerData
            {
            public int iremain;
            public int gremain;
            public int tremain;
            public string hid;
            public string email;
            public string pass;
            public string lockcomp; //"true","false"  
            //public string version;
            };

        public WebService()
            {
            urlchkusg = "http://www.groupgatorcommunity.net/GroupGator/2.5/chkusg.php";
            urlupdusg = "http://www.groupgatorcommunity.net/GroupGator/2.5/updusg.php";
            urlinout = "http://www.groupgatorcommunity.net/GroupGator/2.5/inout.php";
            urllogin = "http://www.groupgatorcommunity.net/GroupGator/2.5/login.php";
            urlcaptchaverfiy = "http://www.groupgatorcommunity.net/GroupGator/2.5/captverify.php";
            urlcaptchagen = "http://www.groupgatorcommunity.net/GroupGator/2.5/ggcapt.php";
            urlcaptchabase = "http://www.groupgatorcommunity.net/GroupGator/2.5/captchatmp/";
            urlacheck = "http://www.groupgatorcommunity.net/GroupGator/2.5/acheck.php";
            urladminunlock = "http://www.groupgatorcommunity.net/GroupGator/2.5/unlock.php";
            urlgettime = "http://www.groupgatorcommunity.net/GroupGator/2.5/agettime.php";
            urlunlocknormal = "http://www.groupgatorcommunity.net/GroupGator/2.5/unlocknormal.php";
            urladminaddtime = "http://www.groupgatorcommunity.net/GroupGator/2.5/aaddtime.php";
            urladminsettime = "http://www.groupgatorcommunity.net/GroupGator/2.5/asettime.php";
            urlactiveusers = "http://www.groupgatorcommunity.net/getactiveusers.php";
            urldonorgift = "http://www.groupgatorcommunity.net/GroupGator/2.5/donorgift.php";
            urlmyip = "http://www.groupgatorcommunity.net/GroupGator/2.5/myip.php";
            urlcreat = "http://adscendmedia.com/creative.php?id=";
            urlggptsqry = "http://www.groupgatorcommunity.net/GroupGator/2.5/ggpts.php";
            urlggpts = "http://www.groupgatorcommunity.net/GroupGator/2.5/ggpts.php";

            queryingserver = false;
            fp = new FingerPrint();
            mid = fp.GetComuputerID();
            }

        public string GetMyIp()
            {
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["uniqueid"] = mid;
            string post = web.PostPage(urlmyip, nvm, 3, 3000);
            string ret = string.Empty;
            if (post.Contains("{user_ip:fail}"))
                {
                ret = string.Empty;
                }
            else
                {
                MyUtils.MyUtils utils = new MyUtils.MyUtils();
                ret = utils.GetStrBetween(post, "{user_ip:", "}");
                }
            return ret;
            }
        public void Enter( Constants.SoftLicense type  )
            {
            //uniqueid
            //status  - enter(1) / exit (0)
            //type  - paid(1) / free (0)
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["uniqueid"] = mid;
            nvm["status"] = "1";
            if (type == Constants.SoftLicense.Demo)
                nvm["type"] = "0";
            else
                nvm["type"] = "1";
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (!(resp.Contains("GroupGatorv2.5")))
                {
                resp = web.PostPage(urlinout, nvm, 0, 0);
                if (!resp.Contains("GroupGatorv2.5"))
                    System.Threading.Thread.Sleep(2000);
                }
            }
        public void Exit(Constants.SoftLicense type)
            {
            //uniqueid
            //status  - enter(1) / exit (0)
            //type  - paid(1) / free (0)
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["uniqueid"] = mid;
            nvm["status"] = "0";
            if (type == Constants.SoftLicense.Demo)
                nvm["type"] = "0";
            else
                nvm["type"] = "1";
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (!resp.Contains("GroupGatorv2.5"))
                {
                resp = web.PostPage(urlinout, nvm, 0, 0);
                if (!resp.Contains("GroupGatorv2.5"))
                    System.Threading.Thread.Sleep(2000);
                }
            }

       
        public ServerData CheckUsage()
            {
            //uniqueid
            //#IREM#1000#/IREM##GREM#1000#/GREM##TREM#0#/TREM#GroupGatorv2.5  
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["uniqueid"] = mid;

            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while ( !resp.Contains("GroupGatorv2.5") )
                {
                resp = web.PostPage(urlchkusg, nvm, 0, 0);
                if (!resp.Contains("GroupGatorv2.5"))
                    System.Threading.Thread.Sleep(2000);
                }
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            string i = utils.GetStrBetween(resp, "#IREM#", "#/IREM#");
            string g = utils.GetStrBetween(resp, "#GREM#", "#/GREM#");
            string t = utils.GetStrBetween(resp, "#TREM#", "#/TREM#");
            ServerData sdata = new ServerData();
            try
                {
                sdata.iremain = Convert.ToInt32(i);
                }
            catch (Exception ex)
                {
                sdata.iremain = 0;
                }
            try
                {
                sdata.gremain = Convert.ToInt32(g);
                }
            catch (Exception ex)
                {
                sdata.gremain = 0;
                }
            try
                {
                sdata.tremain = Convert.ToInt32(t);
                }
            catch (Exception ex)
                {
                sdata.tremain = 0;
                }
            return sdata;
            }
        public void UpdateUsage(ServerData sdata )
            {
            //uniqueid
            //g
            //i
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["uniqueid"] = mid;
            nvm["g"] = sdata.iremain.ToString();
            nvm["i"] = sdata.gremain.ToString();
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (!resp.Contains("GroupGatorv2.5"))
                {
                resp = web.PostPage(urlupdusg, nvm, 0, 0);
                if (!resp.Contains("GroupGatorv2.5"))
                    System.Threading.Thread.Sleep(2000);
                }

            }
        public Constants.ServerResponse Login(ServerData sdata)
            {
            //email
            //password
            //computer
            //lockcomputer
            //status
            Constants.ServerResponse ret = Constants.ServerResponse.NoAccount;  
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["email"] = sdata.email;
            nvm["password"] = sdata.pass;
            nvm["computer"] = mid;
            nvm["lockcomputer"] = sdata.lockcomp;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty )
                {
                resp = web.PostPage(urllogin, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                }
            if (resp.Contains("login:noaccount"))
                {
                ret = Constants.ServerResponse.NoAccount;
                }
            else if (resp.Contains("login:expired"))
                {
                ret = Constants.ServerResponse.Expired;
                }
            else if (resp.Contains("login:notlocked"))
                {
                ret = Constants.ServerResponse.NotLocked;
                }
            else if (resp.Contains("login:wrongdevice"))
                {
                ret = Constants.ServerResponse.WrongComputer;
                }
            else if (resp.Contains("login:valid"))
                {
                ret = Constants.ServerResponse.Valid;
                }
            else if (resp.Contains("login:justlocked"))
                {
                ret = Constants.ServerResponse.JustLocked;
                }
            return ret;
            }

        public Constants.ServerResponse VerifyCaptcha( string captchatext, string email, string pass )
            {
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["captchatxt"] = captchatext;
            nvm["email"] = email;
            nvm["password"] = pass;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage(urlcaptchaverfiy, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                
                }
            Constants.ServerResponse ret = Constants.ServerResponse.NoAccount;
            if (resp.Contains("captcha:failed"))
                ret = Constants.ServerResponse.CaptchaBad;
            if (resp.Contains("account:duped") )
                ret = Constants.ServerResponse.AccountDuped;
            if (resp.Contains("account:created"))
                ret = Constants.ServerResponse.AccountCreated;

            return ret;
            }

        public Constants.ServerResponse AdminCheck(string aname, string apass)
            {
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["email"] = aname;
            nvm["password"] = apass;
            nvm["uniqueid"] = mid;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage(urlacheck, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);

                }
            Constants.ServerResponse ret = Constants.ServerResponse.AcheckFailed;
            if (resp.Contains("acheck:failed"))
                ret = Constants.ServerResponse.AcheckFailed;
            if (resp.Contains("acheck:success"))
                ret = Constants.ServerResponse.AcheckSuccess;

            return ret;
            }

        public Constants.ServerResponse UnlockNormal(string adminname, string adminpass)
            {
            Constants.ServerResponse ret = Constants.ServerResponse.UnlockFailWrongAccount;
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["email"] = adminname;
            nvm["pass"] = adminpass;
            nvm["uniqueid"] = mid;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage(urlunlocknormal, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                }
            if (resp.Contains("usrunlock:fail_NoAccount"))
                ret = Constants.ServerResponse.UnlockFailWrongAccount;
            if (resp.Contains("usrunlock:success"))
                ret = Constants.ServerResponse.UnlockSuccess;
            return ret;
            }
                
        public Constants.ServerResponse Unlock(string emailToUnlock, string adminname, string adminpass)
            {
            Constants.ServerResponse ret = Constants.ServerResponse.UnlockFailNotAdmin;
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["emailtounlock"] = emailToUnlock;
            nvm["adminname"] = adminname;
            nvm["adminpass"] = adminpass;
            nvm["uniqueid"] = mid;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage(urladminunlock, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                }
            if (resp.Contains("aunlock:fail_NoAccount"))
                ret = Constants.ServerResponse.UnlockFailWrongAccount;
            if (resp.Contains("aunlock:fail_NotAdmin"))
                ret = Constants.ServerResponse.UnlockFailNotAdmin;
            if (resp.Contains("aunlock:success"))
                ret = Constants.ServerResponse.UnlockSuccess;
            return ret;
            }

        public Constants.ServerResponse GetTimeRemaining(string email, ref string tremain)
            {
            Constants.ServerResponse ret = Constants.ServerResponse.Failed;
            tremain = "0";
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["email"] = email;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage(urlgettime, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                }
            if (resp.Contains("agettime:fail"))
                ret = Constants.ServerResponse.Failed;
            else if (resp.Contains("agettime:succss"))
                {
                ret = Constants.ServerResponse.Success;
                MyUtils.MyUtils utils = new MyUtils.MyUtils();
                tremain = utils.GetStrBetween(resp, "rem{", "}");
                tremain = tremain.Replace(",", string.Empty);
                }
            return ret;
            }

        public Constants.ServerResponse AddTime(string emailToUnlock, string adminname, string adminpass, string days)
            {
            Constants.ServerResponse ret = Constants.ServerResponse.UnlockFailNotAdmin;
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["emailtounlock"] = emailToUnlock;
            nvm["adminname"] = adminname;
            nvm["adminpass"] = adminpass;
            nvm["uniqueid"] = mid;
            nvm["days"] = days;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage(urladminaddtime, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                }
            if (resp.Contains("aaddtime:fail_NoAccount"))
                ret = Constants.ServerResponse.UnlockFailWrongAccount;
            if (resp.Contains("aaddtime:fail_NotAdmin"))
                ret = Constants.ServerResponse.UnlockFailNotAdmin;
            if (resp.Contains("aaddtime:success"))
                ret = Constants.ServerResponse.UnlockSuccess;
            return ret;
            }

        public Constants.ServerResponse SetTime(string emailToUnlock, string adminname, string adminpass, string days)
            {
            Constants.ServerResponse ret = Constants.ServerResponse.UnlockFailNotAdmin;
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["emailtounlock"] = emailToUnlock;
            nvm["adminname"] = adminname;
            nvm["adminpass"] = adminpass;
            nvm["uniqueid"] = mid;
            nvm["days"] = days;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage(urladminsettime, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                }
            if (resp.Contains("aaddtime:fail_NoAccount"))
                ret = Constants.ServerResponse.UnlockFailWrongAccount;
            if (resp.Contains("aaddtime:fail_NotAdmin"))
                ret = Constants.ServerResponse.UnlockFailNotAdmin;
            if (resp.Contains("aaddtime:success"))
                ret = Constants.ServerResponse.UnlockSuccess;
            return ret;
            }

        public string GetActiveUsersCount()
            {
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            resp = web.GetWebPage(urlactiveusers, 0, 0);
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            string ret = utils.GetStrBetween(resp, "<h3>Active users: ", "</h3>");
            ret = ret.Replace(" ", string.Empty);
            return ret;
            }

        public Constants.ServerResponse GiftDays(string emailToGift, string donormail, string donorpass, string days)
            {
            Constants.ServerResponse ret = Constants.ServerResponse.UnlockFailNotAdmin;
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["emailtogift"] = emailToGift;
            nvm["donormail"] = donormail;
            nvm["donorpass"] = donorpass;
            nvm["uniqueid"] = mid;
            nvm["days"] = days;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage(urldonorgift, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                }
            if (resp.Contains("gifttime:fail_NoDonorAccount"))
                ret = Constants.ServerResponse.GiftFailNoDonor;
            if (resp.Contains("gifttime:fail_NotEnough"))
                ret = Constants.ServerResponse.GiftFailNotEnough;
            if (resp.Contains("gifttime:fail_NoReceiverAccount"))
                ret = Constants.ServerResponse.GiftFailNoReceiver;
            if (resp.Contains("gifttime:success"))
                ret = Constants.ServerResponse.GiftSuccess;
            return ret;
            }

        public int GetGatorPointsUsed ( string ggemail )
            {
            int ret = 0;

            NameValueCollection nvm = new NameValueCollection( );
            nvm.Clear( );
            nvm["email"] = ggemail;
            MyWeb.MyWeb web = new MyWeb.MyWeb( );
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage( urlggptsqry, nvm, 0, 0 );
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep( 2000 );
                }
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            string tmp = utils.GetStrBetween ( resp, "{USED}", "{/USED}");

            try
                {
                ret = Convert.ToInt32( tmp );
                }
            catch (Exception ex)
                {
                ret = 0;
                }

            ret *= 10;
            return ret;
            }

        public Constants.ServerResponse BuyWithGP ( string ggemail, string ggpts, int totalrev )
            {
            Constants.ServerResponse ret = Constants.ServerResponse.GPNotEnough;

            NameValueCollection nvm = new NameValueCollection( );
            nvm.Clear( );
            nvm["email"] = ggemail;
            nvm["ggpts"] = "0";
            nvm["rev"] = totalrev.ToString();

            MyWeb.MyWeb web = new MyWeb.MyWeb( );
            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = web.PostPage( urlggpts, nvm, 0, 0 );
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep( 2000 );
                }

            if (resp.Contains( "{GGPTS:NOACCOUNT}" ))
                ret = Constants.ServerResponse.GPNoAccount;
            else if (resp.Contains( "{GGPTS:NOTENOUGH}" ))
                ret = Constants.ServerResponse.GPNotEnough;
            else if (resp.Contains( "{GGPTS:BOUGHT}" ))                
                ret = Constants.ServerResponse.GPBought;
         

            return ret;
            }

        }
    }
//string reqpage = "http://www.groupgatorcommunity.net/v2/inout.php?type=" + type.ToString() + "&status=" + status.ToString();
//string reqpage = "http://www.groupgatorcommunity.net/v2/applogin.php?email=" + txtDBName.Text + "&password=" + txtDBPass.Text + "&computer=" + compid; ;






//private string msg = "Sorry, your allowed number of free trials are over, please consider donating!";
//private string tit = "Free Usage Period Over!";
//private string dest = "http://groupgatorcommunity.net/login.php";
//        System.Diagnostics.Process.Start(dest);



//webBrowser2.Navigate("http://www.groupgatorcommunity.net/banner.php");




//string reqpage = "http://www.groupgatorcommunity.net/v2/applogin.php?lockcomputer=true&email=" + txtDBName.Text + "&password=" + txtDBPass.Text + "&computer=" + compid;
