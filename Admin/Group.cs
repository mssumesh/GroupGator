using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.IO;

namespace Gator
    {
    public class Group
        {     
        public string id;
        public string url;
        public string name;
        public string link;
        public int total;
        public int npages;
        public int ingame;
        public int online;
        public string memlistpage1;
        

        public Group(string init, Constants.GroupInitType type)
            {
            id = string.Empty;
            url = string.Empty;
            name = string.Empty;
            link = string.Empty;
            total =0;
            npages  = 0;
            ingame = 0;
            online = 0;            

            if (type == Constants.GroupInitType.groupid)
                {
                id = init;
                link = "http://steamcommunity.com/gid/" + id + "/memberslistxml/?xml=1";   
                }
            else if (type == Constants.GroupInitType.groupurl)
                {
                url = init;
                link = "http://steamcommunity.com/groups/" + url + "/memberslistxml/?xml=1";
                }
            else if (type == Constants.GroupInitType.groupname)
                name = init;
            else if (type == Constants.GroupInitType.link)
                {
                string tmp = init;
                int beg = tmp.IndexOf(".com/");
                int end = -1;
                if (beg != -1)
                    {
                    beg += 5;
                    end = tmp.IndexOf("/", beg);     
                    end = end + 1;
                    beg = tmp.IndexOf("/", end);
                    if (beg == -1)
                        beg = tmp.Length;
                    init = tmp.Substring(0,beg);
                    }

                if (!init.Contains("?xml=1"))
                    {
                    if ((init.Substring(init.Length - 1, 1)) == "/")
                        init = init + "memberslistxml/?xml=1";
                    else
                        init = init + "/memberslistxml/?xml=1";
                    }
                link = init;
                }
            memlistpage1 = GetDetails();


            }
        public string GetDetails()
            {          

            MyWeb.MyWeb myweb = new MyWeb.MyWeb();
            MyUtils.MyUtils utils = new MyUtils.MyUtils();

            string post = string.Empty;
            while (post == string.Empty)
                {
                post = myweb.GetWebPage(link, 100, 2000);
                if (post == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                else
                    break;
                };

            string tmp = utils.GetStrBetween(post, "<groupID64>", "</groupID64>");
            tmp = tmp.Replace(Environment.NewLine, string.Empty);
            if (tmp != string.Empty)
                id = tmp;

            tmp = utils.GetStrBetween(post, "<groupName>", "</groupName>");
            tmp = utils.GetStrBetween(tmp, "<", ">");
            int beg, end;
            beg = end = -1;
            if (tmp != string.Empty)
                {
                beg = tmp.LastIndexOf("[");
                end = tmp.IndexOf("]");
                if ((beg != -1) && (end != -1) && ((end - beg) >= 0))
                    {
                    name = tmp.Substring(beg + 1, end - beg - 1);
                    }
                }

            tmp = utils.GetStrBetween(post, "<groupURL>", "</groupURL>");
            tmp = utils.GetStrBetween(tmp, "<", ">");
            beg = end = -1;
            if (tmp != string.Empty)
                {
                beg = tmp.LastIndexOf("[");
                end = tmp.IndexOf("]");
                if ((beg != -1) && (end != -1) && ((end - beg) >= 0))
                    {
                    url = tmp.Substring(beg + 1, end - beg - 1);
                    }
                }

            tmp = utils.GetStrBetween(post, "<memberCount>", "</memberCount>");
            tmp = tmp.Replace(Environment.NewLine, string.Empty);
            tmp = tmp.Replace(",", string.Empty);
            if (tmp != string.Empty)
                {
                try
                    {
                    total = Convert.ToInt32(tmp);
                    }
                catch (Exception ex)
                    {
                    total = 0;
                    }
                }

            tmp = utils.GetStrBetween(post, "<membersInGame>", "</membersInGame>");
            tmp = tmp.Replace(Environment.NewLine, string.Empty);
            tmp = tmp.Replace(",", string.Empty);
            if (tmp != string.Empty)
                {
                try
                    {
                    ingame = Convert.ToInt32(tmp);
                    }
                catch (Exception ex)
                    {
                    ingame = 0;
                    }
                }


            tmp = utils.GetStrBetween(post, "<membersOnline>", "</membersOnline>");
            tmp = tmp.Replace(Environment.NewLine, string.Empty);
            tmp = tmp.Replace(",", string.Empty);
            if (tmp != string.Empty)
                {
                try
                    {
                    online = Convert.ToInt32(tmp);
                    }
                catch (Exception ex)
                    {
                    online = 0;
                    }
                }  

            tmp = utils.GetStrBetween(post, "<totalPages>", "</totalPages>");
            tmp = tmp.Replace(Environment.NewLine, string.Empty);
            tmp = tmp.Replace(",", string.Empty);
            if (tmp != string.Empty)
                {
                try
                    {
                    npages = Convert.ToInt32(tmp);
                    }
                catch (Exception ex)
                    {
                    npages = 0;
                    }
                }

            tmp = utils.GetStrBetween(post, "<members>", "</members>");
            return tmp;
            
            }

        public void Clear()
            {
            id = string.Empty;
            url = string.Empty;
            name = string.Empty;
            link = string.Empty;
            total = 0;
            npages = 0;
            ingame = 0;
            online = 0;   
            }
        

        }
    }
