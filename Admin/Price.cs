using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using Gator;
//using Gator2;
//using Gator25;

namespace Gator
    {
    class Price
        {
        public string pricegetremote;
        public string pricesetremote;
        public string pricelocal;
        GGDisk disk;
        MyWeb.MyWeb web;
        MyUtils.MyUtils utils;
        private string uid;
        private string upass;
        NameValueCollection nvm;

        public struct PRICE
            {
            public string amount;
            public string gp;
            public string promo;
            public string days;
            public string paypal;
            public string savings;
            }

        
        private ArrayList plist;
        public PRICE[] pricelist = new PRICE[20];
        public int Count;

        public Price(string adminname, string adminpass)
            {
            disk = new GGDisk();
            web = new MyWeb.MyWeb();
            utils = new MyUtils.MyUtils();
            nvm = new NameValueCollection();
            plist = new ArrayList();
            pricelocal = disk.dirconfig + "\\pricelist";
            pricegetremote = "http://www.groupgatorcommunity.net/GroupGator/2.5/getprice.php";
            pricesetremote = "http://www.groupgatorcommunity.net/GroupGator/2.5/setprice.php";
            uid = adminname;
            upass = adminpass;
            Count = 0;
            GetAll();
            }

        public string GetPaypalCode(string days)
            {
            string buff = disk.Read(pricelocal);
            ArrayList temp = new ArrayList();
            string ret = string.Empty;
            //temp = utils.GetTokensBetween(buff, "{PRICETAG}{AMOUNT}" + price + "{/AMOUNT}", "{/PRICETAG}");
            for (int i = 0; i < temp.Count; i++)
                {
                if (temp[i].ToString().Contains("{DAYS}" + days + "{/DAYS}"))
                    {
                    ret = utils.GetStrBetween(buff, "{PAYPAL}", "{/PAYPAL}");
                    break;
                    }
                }
            return ret;
            }
        public void GetAll()
            {
            string resp = string.Empty;
            nvm.Clear();
            nvm["email"] = uid;
            nvm["password"] = upass;
            while (resp == string.Empty)
                {
                resp = web.PostPage(pricegetremote, nvm, 0, 0);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                };
            plist.Clear();
            Count = 0;
            plist = utils.GetTokensBetween(resp, "{PRICETAG}", "{/PRICETAG}");
            Count = plist.Count;
            string buff = string.Empty;
            if (Count != 0)
                {

                for (int i = 0; i < plist.Count; i++)
                    buff = buff + "{PRICETAG}" + plist[i].ToString( ) + "{/PRICETAG}";

                disk.Write( buff, pricelocal );
                }
            else
                {
                buff = disk.Read( pricelocal );
                plist = utils.GetTokensBetween( resp, "{PRICETAG}", "{/PRICETAG}" );
                Count = plist.Count;
                }

            //public string amount;
            //public string gp;
            //public string promo;
            //public string days;
            //public string paypal;
            for (int i = 0; i < Count; i++)
                {
                pricelist[i].amount = utils.GetStrBetween(plist[i].ToString(), "{AMOUNT}", "{/AMOUNT}");
                pricelist[i].gp = utils.GetStrBetween(plist[i].ToString(), "{GP}", "{/GP}");
                pricelist[i].promo = utils.GetStrBetween(plist[i].ToString(), "{PROMO}", "{/PROMO}");
                pricelist[i].days = utils.GetStrBetween(plist[i].ToString(), "{DAYS}", "{/DAYS}");
                pricelist[i].paypal = utils.GetStrBetween(plist[i].ToString(), "{PAYPAL}", "{/PAYPAL}");
                pricelist[i].savings = utils.GetStrBetween(plist[i].ToString(), "{SAVE}", "{/SAVE}");
                }
                
            }

        //MODE = 0, add new price
        //mode = 1, update existing price
        //mode = 2, delete price
        public bool Update( string paypal, PRICE price)
            {
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["email"] = uid;
            nvm["password"] = upass;
            nvm["amount"] = price.amount;
            nvm["gp"] = price.gp;
            nvm["promo"] = price.promo;
            nvm["days"] = price.days;
            nvm["paypal"] = price.paypal;
            nvm["save"] = price.savings;
            nvm["mode"] = "1";//mode = 1, update existing price
            string resp = web.PostPage(pricesetremote, nvm, 0, 0);
            if (resp == string.Empty)
                return false;
            else if (resp.Contains("RES:SUCCESS"))
                return true;
            else
                return false;
            }
        public bool Add(PRICE price)
            {
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["email"] = uid;
            nvm["password"] = upass;
            nvm["amount"] = price.amount;
            nvm["gp"] = price.gp;
            nvm["promo"] = price.promo;
            nvm["days"] = price.days;
            nvm["paypal"] = price.paypal;
            nvm["save"] = price.savings;
            nvm["mode"] = "0"; //MODE = 0, add new price
            string resp = web.PostPage(pricesetremote, nvm, 0, 0);
            if (resp == string.Empty)
                return false;
            else if (resp.Contains("RES:SUCCESS"))
                return true;
            else
                return false;
            }
        public bool Delete(string paypal)
            {
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm["email"] = uid;
            nvm["password"] = upass;
            nvm["paypal"] = paypal;
            nvm["mode"] = "2"; //mode = 2, delete price
            string resp = web.PostPage(pricesetremote, nvm, 0, 0);
            if (resp == string.Empty)
                return false;
            else if (resp.Contains("RES:SUCCESS"))
                return true;
            else
                return false;
            }       
        }
    }
