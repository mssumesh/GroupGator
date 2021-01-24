using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Gator
    {
    public class Player
        {
        
        public string name;
        public string id;
        public string url;
        public string link;

        public ArrayList groups;
        //public ArrayList groupurls;
        //public ArrayList curmemcounts;
  
        public Player ( string init, Constants.PlayerInitType type )
            {
            name = string.Empty;
            id = string.Empty;
            url = string.Empty;
            link = string.Empty;
            groups = new ArrayList();
            groups.Clear();
            
            if (type == Constants.PlayerInitType.playername)
                name = init;
            else if (type == Constants.PlayerInitType.playerid)
                {
                id = init;
                link = "http://steamcommunity.com/profiles/" + id + "/?xml=1";
                }
            else if (type == Constants.PlayerInitType.playerurl)
                {
                url = init;
                link = "http://steamcommunity.com/id/" + url + "/?xml=1";
                }
            else if (type == Constants.PlayerInitType.link)
                {
                string tmp = init;
                int beg = tmp.IndexOf(".com/");
                if (beg != -1)
                    {
                    beg += 5;
                    beg = tmp.IndexOf("/", beg);
                    if (beg == -1)
                        beg = tmp.Length;
                    init = tmp.Substring(beg);
                    }
                if (!init.Contains("?xml=1"))
                    {
                    if ((init.Substring(init.Length - 1, 1)) == "/")
                        init = init + "?xml=1";
                    else
                        init = init + "/?xml=1";
                    }
                link = init;
                }
            GetDetails();
            }

        public Player ( string init, Constants.PlayerInitType type, bool minimal )
            {
            name = string.Empty;
            id = string.Empty;
            url = string.Empty;
            link = string.Empty;
            groups = new ArrayList( );
            groups.Clear( );

            if (type == Constants.PlayerInitType.playername)
                name = init;
            else if (type == Constants.PlayerInitType.playerid)
                {
                id = init;
                link = "http://steamcommunity.com/profiles/" + id + "/?xml=1";
                }
            else if (type == Constants.PlayerInitType.playerurl)
                {
                url = init;
                link = "http://steamcommunity.com/id/" + url + "/?xml=1";
                }
            else if (type == Constants.PlayerInitType.link)
                {
                string tmp = init;
                int beg = tmp.IndexOf( ".com/" );
                if (beg != -1)
                    {
                    beg += 5;
                    beg = tmp.IndexOf( "/", beg );
                    if (beg == -1)
                        beg = tmp.Length;
                    init = tmp.Substring( beg );
                    }
                if (!init.Contains( "?xml=1" ))
                    {
                    if ((init.Substring( init.Length - 1, 1 )) == "/")
                        init = init + "?xml=1";
                    else
                        init = init + "/?xml=1";
                    }
                link = init;
                }
            GetDetailsMinimal( );
            }

        public void GetDetailsMinimal ( )
            {
            MyWeb.MyWeb myweb = new MyWeb.MyWeb( );
            MyUtils.MyUtils utils = new MyUtils.MyUtils( );

            string post = string.Empty;
            while (post == string.Empty)
                {
                post = myweb.GetWebPage( link, 0, 0 );
                if (post == string.Empty)
                    System.Threading.Thread.Sleep( 3000 );
                else
                    break;
                }

            string tmp = utils.GetStrBetween( post, "<steamID64>", "</steamID64>" );
            tmp = tmp.Replace( Environment.NewLine, string.Empty );
            if (tmp != string.Empty)
                id = tmp;

            tmp = utils.GetStrBetween( post, "<customURL>", "</customURL>" );
            tmp = utils.GetStrBetween( tmp, "<", ">" );
            int beg, end;
            beg = end = -1;
            if (tmp != string.Empty)
                {
                beg = tmp.LastIndexOf( "[" );
                end = tmp.IndexOf( "]" );
                if ((beg != -1) && (end != -1) && ((end - beg) >= 0))
                    {
                    url = tmp.Substring( beg + 1, end - beg - 1 );
                    }
                }

            tmp = utils.GetStrBetween( post, "<steamID>", "</steamID>" );
            tmp = utils.GetStrBetween( tmp, "<", ">" );
            beg = end = -1;
            if (tmp != string.Empty)
                {
                beg = tmp.LastIndexOf( "[" );
                end = tmp.IndexOf( "]" );
                if ((beg != -1) && (end != -1) && ((end - beg) >= 0))
                    {
                    name = tmp.Substring( beg + 1, end - beg - 1 );
                    }
                }

            //groups.Clear( );
            //groupurls.Clear( );
            }
        public void GetDetails ()
            {           
            
            MyWeb.MyWeb myweb = new MyWeb.MyWeb();
            MyUtils.MyUtils utils = new MyUtils.MyUtils();

            string post = string.Empty;
            while (post == string.Empty)
                {
                post = myweb.GetWebPage(link, 0, 0);
                if (post == string.Empty)
                    System.Threading.Thread.Sleep(3000);
                else
                    break;
                }

            string tmp = utils.GetStrBetween(post, "<steamID64>", "</steamID64>");
            tmp = tmp.Replace(Environment.NewLine, string.Empty);
            if (tmp != string.Empty)
                id = tmp;

            tmp = utils.GetStrBetween(post, "<customURL>", "</customURL>");
            tmp = utils.GetStrBetween(tmp, "<", ">");
            int beg, end;
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

            tmp = utils.GetStrBetween(post, "<steamID>", "</steamID>");
            tmp = utils.GetStrBetween(tmp, "<", ">");
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

            groups.Clear();
            groups = utils.GetTokensBetween(post, "<groupID64>" + Environment.NewLine, Environment.NewLine + "</groupID64>");
            if(groups.Count==0)
                groups = utils.GetTokensBetween(post, "<groupID64>", "</groupID64>");

            ArrayList arrltmp = new ArrayList();
            arrltmp.Clear();
            for (int i = 0; i < groups.Count; i++)
                {
                Group grp = new Group(groups[i].ToString(), Constants.GroupInitType.groupid);
                arrltmp.Add(grp.url);
                //string strcnt = grp.total.ToString();
                }
            groups.Clear();
            groups = arrltmp;        
            }
        }
    }
