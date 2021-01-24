using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Resources;




namespace Gator
    {
    public class SteamAuth
        {
        public Constants.SignInStatus status;
        public string sessionid;
        public string cookie;
        public string transurl;
        public string captchaid;
        public string captchatxt;
        public string emailcode;
        public string steamid;
        public string webcookie;
        public string token;
        public string emailauth;
        public string emailsteamid;
        public string steamlogin;

        public string uname;
        public string encpwd;
        public string pwd;
        public string kmod;
        public string kexp;
        public string krsatime;

        public string captchaurl;

        ManualResetEvent authdone;

        public SteamAuth( )
            {
            status = Constants.SignInStatus.LoggedOff;
            transurl = string.Empty;
            webcookie = string.Empty;
            captchaurl = string.Empty;//"https://steamcommunity.com/public/captcha.php?gid=";
            token = string.Empty;
            encpwd = string.Empty;
            kmod = string.Empty;
            kexp = string.Empty;
            krsatime = string.Empty;
            captchaid = string.Empty;
            captchatxt = string.Empty;
            emailcode = string.Empty;
            emailauth = string.Empty;
            emailsteamid = string.Empty;
            sessionid = string.Empty; ;
            cookie = string.Empty; ;
            steamid = string.Empty; ;
            steamlogin = string.Empty; ;
            uname = string.Empty; ;
            pwd = string.Empty; ;             
            authdone = new ManualResetEvent(false);         
            }

        public void GetRSAKey(string name)
            {
            uname = name;
            NameValueCollection formData = new NameValueCollection();
            formData["username"] = uname;

            MyWeb.MyWeb web = new MyWeb.MyWeb();
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            string resp = string.Empty;
            while ((resp == string.Empty) || (resp.Contains("Error 503 Service Unavailable")) || (resp.Contains("The Steam Community is currently unavailable")))
                {
                resp = web.PostPage("https://steamcommunity.com/login/getrsakey/", formData, 0, 0, ref cookie);
                if ((resp == string.Empty) || (resp.Contains("Error 503 Service Unavailable")) || (resp.Contains("The Steam Community is currently unavailable")))
                    {
                    System.Threading.Thread.Sleep(3000);
                    }
                };
            string sessid = GetSessionID(resp);
            if (sessid != string.Empty)
                sessionid = sessid;
            if (sessionid != string.Empty)
                cookie = "sessionid=" + sessionid + ";";
            kmod = utils.GetStrBetween(resp, "\"publickey_mod\":\"", "\"");
            kexp = utils.GetStrBetween(resp, "\"publickey_exp\":\"", "\"");
            krsatime = utils.GetStrBetween(resp, "\"timestamp\":\"", "\"");  
            }
        public void UpdateSessionID()
            {
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string tmp = string.Empty;
            string resp = web.GetWebPage( "https://steamcommunity.com/groups/groupgator", 0, 0, ref tmp );
            if ((!resp.Contains("sessionid=")) &&(!tmp.Contains ("sessionid=")))
                resp = web.GetWebPage( "http://steamcommunity.com/apps/", 0, 0, ref tmp );
            string sessid = string.Empty;
            if (tmp.Contains("sessionid="))
                {                
                sessid = GetSessionID(tmp);
                if (sessid != string.Empty)
                    {
                    sessionid = sessid;
                    UpdateCookie();
                    }
                }
            if (( resp.Contains("sessionid=")) && ( sessid == string.Empty ) )
                {
                sessid = GetSessionID(resp);
                if (sessid != string.Empty)
                    {
                    sessionid = sessid;
                    UpdateCookie();
                    }
                }
            }
        private void UpdateCookie()
            {
            cookie = "steamLogin=" + steamlogin + ";sessionid=" + sessionid + ";";
            }
        public string GetSessionID( string buff)
            {
            string ret = string.Empty;
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            ret = utils.GetStrBetween(buff, "sessionid=", ";");
            if ( ret == string.Empty )
                ret = utils.GetStrBetween(buff, "sessionid=", ",");
        
            //int beg, end;
            //beg = end = -1;
            //if ((cookie != string.Empty) && (cookie != null))
            //    {
            //    beg = cookie.IndexOf("sessionid=");
            //    if (beg != -1)
            //        {
            //        beg += 10;
            //        end = cookie.IndexOfAny(new char[] { ';', ',' }, beg);
            //        if (end == -1)
            //            ret = cookie.Substring(beg);
            //        else
            //            ret = cookie.Substring(beg, end - beg);
            //        }
            //    }
            return ret;

            }


        Gator2.Gator2Main gt;
        public Constants.SignInStatus  Authenticate(string encpass, string captxt, string emailstm, Gator2.Gator2Main gtmain )
            {
            if (status != Constants.SignInStatus.Authenticating  )
                {
                gt = gtmain;
                transurl = string.Empty;
                webcookie = string.Empty;
                token = string.Empty;
                emailauth = emailstm;
                captchatxt = captxt;

                

                authdone.Reset();
                encpwd = encpass;
                status = Constants.SignInStatus.Authenticating;
                Thread thAuth;
                ThreadStart thAuthStart;
                thAuthStart = new ThreadStart(AuthProc);
                thAuth = new Thread(thAuthStart);
                thAuth.IsBackground = true;
                thAuth.Start();
                authdone.WaitOne(Timeout.Infinite);

                }
            else
                {
                //another auth in progress
                }
            return status;
            }
        private void AuthProc()
            {
            gt.lblLoginStatus.Text = "AUTHENTICATING";
            gt.lblLoginStatus.Refresh();
            NameValueCollection formData = new NameValueCollection();
            formData.Clear();
            formData["username"] = uname; 
            formData["password"] = encpwd;
            formData["emailauth"] = emailauth;
            formData["captchagid"] = captchaid;            
            formData["captcha_text"] = captchatxt;
            formData["emailsteamid"] = emailsteamid;
            formData["rsatimestamp"] = krsatime;

            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            while ((resp == string.Empty) || (resp.Contains("Error 503 Service Unavailable")) || (resp.Contains("The Steam Community is currently unavailable")))
                {
                resp = web.PostPage("https://steamcommunity.com/login/dologin/", formData, 0, 0,ref  cookie);
                if ((resp == string.Empty) || (resp.Contains("Error 503 Service Unavailable")) || (resp.Contains("The Steam Community is currently unavailable")))
                    {
                    System.Threading.Thread.Sleep(3000);
                    }
                };

            string sessid = GetSessionID(resp);
            if (sessid != string.Empty)
                sessionid = sessid;
            if ( sessionid != string.Empty )
                cookie = "sessionid=" + sessionid + ";";
//sessionid=NTY4MjAzNjI3; Steam_Language=english; timezoneOffset=19800,0; steamLogin=76561198060247951%7C%7CB758FED3E08631A1A8468DCD27508C49E0CA4EBD


            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            if ( resp.Contains ("\"message\":\"Incorrect login\"" ))
                status = Constants.SignInStatus.Failed;

            if (resp.Contains("login_complete\":true"))
                {
                //{"success":true,"login_complete":true,"transfer_url":"https:\/\/store.steampowered.com\/\/login\/transfer","transfer_parameters":{"steamid":"76561198060247951","token":"4A932916A0F2FC5D1E14EC141545E5758153BFD6"}}
                ///////
//              Cache-Control: no-cache
                //Pragma: no-cache
                //Set-Cookie: steamLogin=76561198060247951%7C%7CB758FED3E08631A1A8468DCD27508C49E0CA4EBD; path=/
                //Set-Cookie: steamMachineAuth76561198060247951=DB9DA53AC036A06AC4D80E5409DADB21088D3991; expires=Fri, 15-Jul-2022 18:12:35 GMT; path=/; secure; httponly
                //Content-Length: 267
                //Connection: close
                //Content-Type: application/json; charset=utf-8
                //{"success":true,"login_complete":true,"transfer_url":"https:\/\/store.steampowered.com\/\/login\/transfer","transfer_parameters":{"steamid":"76561198060247951","token":"B758FED3E08631A1A8468DCD27508C49E0CA4EBD","webcookie":"DB9DA53AC036A06AC4D80E5409DADB21088D3991"}}
//                ////////
                transurl = utils.GetStrBetween ( resp, "\"transfer_url\":\"","\"" );
                transurl = transurl.Replace("\\", string.Empty);
                steamid = utils.GetStrBetween(resp, "steamid\":\"", "\"");
                token = utils.GetStrBetween(resp, "token\":\"", "\"");
                webcookie  = utils.GetStrBetween(resp, "webcookie\":\"", "\"");
                
                steamlogin = steamid + "%7C%7c" + token;
                


                //string tmpckie = string.Empty;
                if (sessionid == string.Empty)
                    {
                    //web.GetWebPage("http://steamcommunity.com/profiles/" + steamid + "/?xml=1", 3, 5000, ref tmpckie);
                    resp = web.GetWebPage("http://steamcommunity.com/profiles/" + steamid + "/?xml=1", 3, 5000);
                    sessid = string.Empty;
                    sessid = GetSessionID(resp);
                    if (sessid != string.Empty)
                        sessionid = sessid;
                    //int beg, end;
                    //beg = end = -1;
                    //if ((tmpckie != string.Empty) && (tmpckie != null))
                    //    {
                    //    beg = tmpckie.IndexOf("sessionid=");
                    //    if (beg != -1)
                    //        {
                    //        beg += 10;
                    //        end = tmpckie.IndexOfAny(new char[] { ';', ',' }, beg);
                    //        if (end == -1)
                    //            sessionid = tmpckie.Substring(beg);
                    //        else
                    //            sessionid = tmpckie.Substring(beg, end - beg);
                    //        }
                    //    }
                    }

                if (transurl != string.Empty)
                    if (sessionid == string.Empty)
                        TransferLogin();

                if ((steamlogin != string.Empty) && (sessionid != string.Empty))
                    cookie = "sessionid=" + sessionid + ";steamLogin=" + steamlogin;

                status = Constants.SignInStatus.LoggedIn;
                }
            if ( resp.Contains ("captcha_needed\":true") )
                {
                //{"success":false,"captcha_needed":false,"captcha_gid":-1,"message":"Incorrect login."}
                //{"success":false,"captcha_needed":true,"captcha_gid":"1317542928092269974","message":"Incorrect login."}
                captchaid = utils.GetStrBetween(resp, "captcha_gid\":\"", "\"");
                captchaurl = "https://steamcommunity.com/public/captcha.php?gid=" + captchaid;
                status = Constants.SignInStatus.CaptchaReqd;
                }
            //{"success":false,"message":"SteamGuard","emailauth_needed":true,"emaildomain":"yahoo.com","emailsteamid":"76561198060247951"}
            if (resp.Contains("emailauth_needed\":true"))
                {
                emailsteamid = utils.GetStrBetween(resp, "emailsteamid\":\"", "\"");
                status = Constants.SignInStatus.SGuardReqd;
                }
           
            authdone.Set();
            }

        public void LogOut()
            {
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            web.GetWebPage("http://steamcommunity.com/?action=doLogout", 0, 0);

            }
        public void TransferLogin()
            {
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            string resp = string.Empty;
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["steamid"] = steamid;
            nvm["token"] = token;
            do
                {
                resp = web.PostPage (transurl, nvm, 0, 0, ref cookie);
                if ((resp.Contains("Error 503 Service Unavailable")) || (resp.Contains("The Steam Community is currently unavailable")))
                    {
                    System.Threading.Thread.Sleep(3000);
                    }
                } while ((resp.Contains("Error 503 Service Unavailable")) || (resp.Contains("The Steam Community is currently unavailable")));
            string sessid = GetSessionID(resp);
            if (sessid != string.Empty)
                sessionid = sessid;

            }
     


        }
    }
