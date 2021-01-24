using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.IO;

namespace Gator25
    {
    class AdAPI
        {
        //Your API key is 1340954630. Your publisher ID is 16046.
        //http://adscendmedia.com/api-get.php
        //http://adscendmedia.com/api-get.php?pubid=1234&key=123456789&mode=leads_count&user_ip=12.34.56.78&user_subid=page1&date_range=1        

        private string reqpage = "http://adscendmedia.com/api-get.php?pubid=16046&key=1340954630&";
        public string subid = string.Empty;
        public string myip = string.Empty;

        MyWeb.MyWeb web;
        MyUtils.MyUtils utils;
        Gator.GGDisk disk;
        Gator.FingerPrint fp;
        Gator.WebService wsrv;
        public AdAPI(string ggemail)
            {
            wsrv = new Gator.WebService();
            fp = new Gator.FingerPrint();
            web = new MyWeb.MyWeb();
            utils = new MyUtils.MyUtils();
            disk = new Gator.GGDisk();

            string id_md5 = string.Empty;
            id_md5 = fp.CreateMD5Hash(ggemail);
            if (id_md5.Length > 32)
                id_md5 = id_md5.Substring(0, 32);
            subid = id_md5;
            myip = wsrv.GetMyIp();
            if (!File.Exists( disk.filedefcreat ))
                {
                web.DownloadFile( "http://adscendmedia.com/creat/default.gif", disk.filedefcreat, 3, 2000 );
                }
            //curpage = 1;
            }

        public int GetUserPoints()
            {
            string revstr = GetUserRevenue();
            int ipts = 0;
            ipts = ConvertMoneyToGP(revstr);
            return ipts;
            }
        public int GetUserBalance ( )
            {
            string revstr = GetUserRevenue( );
            int ipts = 0;
            ipts = ConvertMoneyToGP( revstr );
            ipts /= 10;
            return ipts;
            }
        public int ConvertMoneyToGP( string revstr)
            {
            double rev = 0.0;
            if ((revstr == "0") || (revstr == string.Empty) || (revstr == "0.0"))
                rev = 0.0;
            else
                {
                try
                    {
                    rev = Convert.ToDouble(revstr);
                    }
                catch (Exception ex)
                    {
                    rev = 0.0;
                    }
                }
            double points = rev * 10;
            int ipts = Convert.ToInt32(points);
            //ipts = ipts * 10;
            return ipts;
            }
        public string GetUserRevenue()
            {
            myip = wsrv.GetMyIp();
            string qpage = "http://adscendmedia.com/api-get.php?pubid=16046&key=1340954630&mode=leads_payout&user_subid=" + subid + "&date_range=0";
            string ret = string.Empty;
            ret = web.GetWebPage(qpage, 3, 3000);
            return ret;

            }
        public void GetAllOffers()
            {
            myip = wsrv.GetMyIp();
            string qpage = "http://adscendmedia.com/api-get.php?pubid=16046&key=1340954630&mode=offers&user_ip=" + myip + "&user_subid=" + subid + "&category=0&creative=1&quantity=0";
            string ret = string.Empty;
            ret = web.GetWebPage(qpage, 3, 3000);
            if (ret.Contains("No offers available"))
                count = 0;
            else
                count = ExtractOffers(ret);
            //return count;            
            }
        //{"8779":{"id":"8779","name":"Jabong.com India","cost":"0.00","payout":"0.15","time_to_credit":"0","description":"Sign up with Jabong now & get INR 2000 coupon!","NULL":null,"type":"","conv_point":"13","conv_notes":"Valid Completed Signup.","category":"14","completed":0,"url":"http:\/\/adscendmedia.com\/click.php?aff=16046&camp=8779&crt=0&sid=9854983E1123CB5E568B24E23525BD93","creative":["http:\/\/adscendmedia.com\/creat\/default.gif","http:\/\/adscendmedia.com\/creative.php?id=18717"],"perf_score":0,"preferred":0}}
        
        private int ExtractOffers(string resp)
            {
            int beg, end;
            beg = end = -1;
            beg = resp.IndexOf("\"id\":\"");
            string tmp;
            int i = 0;
            while (beg != -1)
                {
                tmp = string.Empty;
                beg = beg + "\"id\":\"".Length;
                end = resp.IndexOf("\"", beg);
                if (end != -1)
                    {
                    tmp = resp.Substring(beg, end - beg);
                    MyOffers[i].id = tmp;
                    }
                beg = resp.IndexOf("\"name\":\"", end);
                if (beg != -1)
                    {
                    beg = beg + "\"name\":\"".Length;
                    end = resp.IndexOf("\"", beg);
                    if (end != -1)
                        {
                        tmp = resp.Substring(beg, end - beg);
                        MyOffers[i].name = tmp;
                        }
                    }

                beg = resp.IndexOf("\"cost\":\"", end);
                if (beg != -1)
                    {
                    beg = beg + "\"cost\":\"".Length;
                    end = resp.IndexOf("\"", beg);
                    if (end != -1)
                        {
                        tmp = resp.Substring(beg, end - beg);
                        MyOffers[i].cost = tmp;
                        }
                    }


                beg = resp.IndexOf("\"payout\":\"", end);
                if (beg != -1)
                    {
                    beg = beg + "\"payout\":\"".Length;
                    end = resp.IndexOf("\"", beg);
                    if (end != -1)
                        {
                        tmp = resp.Substring(beg, end - beg);
                        MyOffers[i].payout = tmp;
                        }
                    }

                beg = resp.IndexOf("\"time_to_credit\":\"", end);
                if (beg != -1)
                    {
                    beg = beg + "\"time_to_credit\":\"".Length;
                    end = resp.IndexOf("\"", beg);
                    if (end != -1)
                        {
                        tmp = resp.Substring(beg, end - beg);
                        MyOffers[i].time2credit = tmp;
                        }
                    }


                beg = resp.IndexOf("\"description\":\"", end);
                if (beg != -1)
                    {
                    beg = beg + "\"description\":\"".Length;
                    end = resp.IndexOf("\"", beg);
                    if (end != -1)
                        {
                        tmp = resp.Substring(beg, end - beg);
                        MyOffers[i].desc = tmp;
                        }
                    }


                beg = resp.IndexOf("\"url\":\"", end);
                if (beg != -1)
                    {
                    beg = beg + "\"url\":\"".Length;
                    end = resp.IndexOf("\"", beg);
                    if (end != -1)
                        {
                        tmp = resp.Substring(beg, end - beg);
                        tmp = tmp.Replace("\\", string.Empty);
                        MyOffers[i].url = tmp;
                        }
                    }

                MyOffers[i].creative = "default.gif";
                string creat = string.Empty;
                string crid = string.Empty;
                beg = resp.IndexOf("creative\":", end);
                if (beg != -1)
                    {
                    beg = beg + "creative\":".Length;
                    end = resp.IndexOf("perf_score", beg);
                    if (end != -1)
                        {
                        tmp = resp.Substring(beg, end - beg);
                        tmp = tmp.Replace("\\", string.Empty);
                        creat = string.Empty;
                        crid = string.Empty;
                        crid = utils.GetStrBetween(tmp, "creative.php?id=", "\"");
                        if (crid != string.Empty)
                            MyOffers[i].creative = crid;
                        else
                            MyOffers[i].creative = "default.gif";                             
                        }
                    }
                beg = resp.IndexOf("\"id\":\"", end);
                i++;
                if (i >= 500)
                    break;
                };
            return i;
            }
        public struct Offer
            {
            public string id;
            public string desc;
            public string name;
            //public string status;
            public string cost;
            public string payout;
            public string url;
            public string time2credit;
            public string creative;
            }
        public Offer[] MyOffers = new Offer[500];
        public int count = 0;


        //public string GetMyIP(  )
        //    {
        //    Gator.FingerPrint fp = new Gator.FingerPrint();
        //    string mid = fp.GetComuputerID();
        //    MyWeb.MyWeb web = new MyWeb.MyWeb();
        //    NameValueCollection nvm = new NameValueCollection();
        //    nvm.Clear();
        //    nvm["uniqueid"] = mid;
        //    string post = web.PostPage("http://www.groupgatorcommunity.net/GroupGator/2.5/myip.php", nvm, 3, 3000);
        //    string ret = string.Empty;
        //    if ( post.Contains ("{user_ip:fail}" ))
        //        {
        //        ret = string.Empty;
        //        }
        //    else
        //        {
        //        MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //        ret = utils.GetStrBetween ( post, "{user_ip:", "}" );
        //        }
        //    return ret;

        //    }
        //public string GetUserRevenue2()
        //    {
        //    string qpage = "http://adscendmedia.com/api-get.php?pubid=16046&key=1340954630&mode=leads_payout&user_ip=" + myip + "&user_subid=" + subid + "&date_range=0";
        //    string ret = string.Empty;
        //    ret = web.GetWebPage(qpage, 0, 0);
        //    return ret;

        //    }

        //public string GetLeadsCount()
        //    {
        //    string qpage = "http://adscendmedia.com/api-get.php?pubid=16046&key=1340954630&mode=leads_count&user_ip=" + myip + "&user_subid=" + subid + "&date_range=0";
        //    string ret = string.Empty;
        //    ret = web.GetWebPage(qpage, 3, 3000);
        //    return ret;

        //    }

        }
    }
