using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.IO;
using System.Collections.Specialized;
using Gator2;

namespace Gator
    {
    public class GatherList
        {
        public int nICur;
        public int nGCur;
        #region list
        public bool SteamOnline;
        private bool isfreeversion;

        private Gator2Main gtmain;
        private Profile prof;
        public Group togroup;
        private Group fromgroup;
        
        public GatherList(string myid, Constants.SoftLicense lic, Gator2Main gt)
            {
            saved = false;
            gtmain = gt;
            glist=new StringBuilder();
            glist.Length=0;
            glist.Append( "<GGV3>" );
            listtmp = new StringBuilder( );
            listtmp.Length = 0;

            pageblocks = new StringBuilder();
            pageblocks.Length = 0;
            nGathered = 0;
            nSkipGather = 0;
            nInvited = 0;
            nSkipInvite = 0;
            nglobalinvites = 0;
            nThreadsEnded = 0;
            SteamOnline = true;

            lastinvitedid = "-1";
            inviter = myid;

            pageque= new Queue();
            pageque.Clear();
            if(lic==Constants.SoftLicense.Demo)
                isfreeversion=true;
            else
                isfreeversion=false;

            StopGather=false;
            StopInvite=false;
            IsGathering = false;
            IsInviting = false;
            license = lic;

            }
        private void SetFromGroup(string frmgrp)
            {
            Constants.GroupInitType ginit;
            if (frmgrp.Contains("/"))
                ginit = Constants.GroupInitType.link;
            else
                ginit = Constants.GroupInitType.groupurl;
            fromgroup = new Group(frmgrp, ginit);
            }
        private void SetToGroup(string togrp)
            {
            Constants.GroupInitType ginit;
            if (togrp.Contains("/"))
                ginit = Constants.GroupInitType.link;
            else
                ginit = Constants.GroupInitType.groupurl;
            togroup = new Group(togrp, ginit);
            }
        #endregion

        #region gather
        public int nGathered = 0;
        public int nSkipGather = 0;
        public bool saved = true;
        public StringBuilder pageblocks;
        public StringBuilder glist;
        private bool StopGather;
        private int nThreadsEnded = 0;
        private int nThreads = 5;
        private Queue pageque;
        public bool IsGathering;
        private int gathertype = 7;
        public int ntogather = 0;
        static readonly object _lockerQueue = new object();
        static readonly object _lockerPageBlocks = new object();
        static readonly object _lockerNG = new object();
        
        static readonly object _lockerList = new object();
        Constants.SoftLicense license;

        public void ClearGather()
            {
            if (IsGathering == true)
                GatherStop();
            while (IsGathering == true)
                {
                System.Threading.Thread.Sleep(500);
                }
            GGDisk ggfile = new GGDisk();
            ggfile.RemoveWhiteLists(glist);
            glist.Length = 0;
            glist.Append("<GGV3>");
            pageblocks.Length = 0;
            nGathered = 0;
            nSkipGather = 0;
            nThreadsEnded = 0;
            pageque.Clear();
            saved = true;
            }
        

        public string GetMemberListXMLPage( string reqpage )
            {
            string resp = string.Empty;
            string ret = string.Empty;
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            while (resp == string.Empty)
                {
                resp = web.GetWebPage(reqpage, 100, 2000);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                else
                    break;
                }
            if (resp != string.Empty)
                {
                MyUtils.MyUtils utils = new MyUtils.MyUtils();
                ret = utils.GetStrBetween(resp, "<members>", "</members>");
                }
            return ret;
            }

        public string TrimMemberList(string memberbuff, int nToSkip, bool istoskipfrombeginning)
            {
            int lengthOfOneMemberId = "<steamID64>".Length + "</steamID64>".Length + "76561198054213311".Length;
            int lengthToSkip = nToSkip * lengthOfOneMemberId;
            string ret = string.Empty;
            string tmpbuff = memberbuff.Replace("\x0d\x0a", string.Empty);
            memberbuff = tmpbuff;
            if (memberbuff.Length > lengthToSkip)
                {
                if (istoskipfrombeginning == true)
                    {
                    //int lenrem = memberbuff.Length - lengthToSkip;
                    ret = memberbuff.Substring(lengthToSkip);
                    }
                else if (istoskipfrombeginning == false)
                    {
                    int limit = memberbuff.Length - lengthToSkip;
                    ret = memberbuff.Substring(0, limit);
                    }
                }
            return ret;
            }

       
        public StringBuilder listtmp;
        int curp = 1;
        bool allgathered = false;
        int inglimit = 1;
        int offlimit = 1;
        int onlimit = 1;
        static readonly object _lockcurp = new object();
        static readonly object _locklist = new object();        
        static readonly object _lockngot = new object();
        static readonly object _locknskip = new object();
        static Constants.GatherSpeed gs;
        string blacklist = string.Empty;
        string whitelist = string.Empty;

        static readonly object _lockgremain = new object( );


        public void GatherNew(string frmgrp )
            {            
            if (IsGathering == false)
                {
                if (IsInviting == false)
                    {

                    if (listtmp.Length != 0)
                        {
                        glist.Append( listtmp.ToString( ) );
                        listtmp.Length = 0;
                        }
                   

                    IsGathering = true;
                    pageblocks.Length = 0;
                    SetFromGroup(frmgrp);
                    GGDisk disk = new GGDisk();
                    disk.AddFromGroup(fromgroup);

                    //blacklist = disk.Read(disk.dirinvite + "\\" + togroup.url);
                    //whitelist = disk.Read(disk.dirgather + "\\" + fromgroup.url);
                    
                    StopGather = false;

                    //offline bit 0 = 1
                    //online bit 1 = 2
                    //ingame bit 2 = 4

                    allgathered = false;
                    curp = 1;                    

                    //inglimit = fromgroup.ingame / 1000 + 1;
                    //offlimit = fromgroup.online / 1000 + 1;
                    //onlimit = offlimit;

                                       
                    try
                        {
                        listtmp.Append( "<GROUP><GURL>" + fromgroup.url + "</GURL><GID>" + fromgroup.id + "</GID>" );       
                        }
                    catch (Exception ex)
                        {
                        }

                    gtmain.Log("Starting gather : From Group " + fromgroup.url );
                    //exitcleanupdone = false;
                    gtmain.DisableGatherControls();

                    WebService ws = new WebService( );
                    WebService.ServerData sd = new WebService.ServerData( );
                    sd = ws.CheckUsage( );
                    nGCur = 0;

                    if (license == Constants.SoftLicense.Demo)
                        {
                        if (sd.gremain <= 0)
                            {
                            System.Windows.Forms.MessageBox.Show( "Your 3000 free gather limit for demo version has reached! Please donate for unlimited usage. Thank You", "Free gather limit reached!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                            Environment.Exit( 0 );
                            }
                        }
                    else
                        {
                        if (sd.tremain <= 0)
                            {
                            
                            System.Windows.Forms.MessageBox.Show( "Your subscription period has expired. Please renew for continued usage. Thank you", "Subscription Expired!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                            Environment.Exit( 0 );
                            }
                        }

                    nThreadsEnded = 0;
                    Thread thGather;
                    ThreadStart thGatherStart;

                    for (int i = 0; i < nThreads; i++)
                        {
                        thGatherStart = new ThreadStart(GatherProcNew);
                        thGather = new Thread(thGatherStart);
                        thGather.IsBackground = true;
                        thGather.Start();
                        }
                    
                    }
                else
                    {
                    gtmain.Log("Gather error: an invite process is in progress.. Please wait for it to complete.", Constants.LogMsgType.Error, true);
                    return;
                    }
                }
            else
                {
                gtmain.Log("Gather error: annother gather process is in progress.. Please wait for it to complete.", Constants.LogMsgType.Error, true);
                return;
                }
            }
        //bool exitcleanupdone = false;
        private void GatherProcNew()
            {
            //int ntoskip = 0;
            //bool shouldskip = false;
            //bool skipthispage = false;
            int pageno = 0;
            string reqpage = string.Empty;
            string page = string.Empty;
            //bool skipbeginning = true;

            bool quitloop = false;
            while (quitloop == false)
                {
                if ( StopGather == true )
                    break;

                pageno = 1;
                lock (_lockcurp)
                    {
                    if (curp > fromgroup.npages)
                        quitloop = true;
                    else
                        {
                        pageno = curp;
                        Interlocked.Increment( ref curp );
                        }
                    }
                if (quitloop == true)
                    break;
                
                if ( StopGather == true )
                    break;
                
                
                reqpage = "http://steamcommunity.com/gid/" + fromgroup.id + "/memberslistxml/?xml=1&p=" + pageno.ToString();
                page = string.Empty;
                while (page == string.Empty)
                    {
                    page = GetMemberListXMLPage(reqpage);
                    if (page == string.Empty)
                        System.Threading.Thread.Sleep(500);
                    else
                        break;
                    if (StopGather == true)
                        break;
                    }
                page = page.Replace("\x0d\x0a", string.Empty);

                if (StopGather == true)
                    break;
                else
                    {
                    
                    int retr = 1000;
                    if (pageno == fromgroup.npages)
                        retr = fromgroup.total % 1000;
                    lock (_lockngot)
                        {
                        nGCur += retr;
                        if (license == Constants.SoftLicense.Demo)
                            {
                            
                            gtmain.DecGRemain( retr );
                            
                            }
                        nGathered += retr;
                        
                        //gtmain.lblUsable.Text = gtmain.lblIRemain.Text = nGathered.ToString( );
                        gtmain.lblGatherTotal.Text = nGathered.ToString( );
                        }

                    lock (_locklist)
                        {
                        listtmp.Append(page);
                        }
                    gtmain.Log("gathered memberlist page #:"+ pageno.ToString(), Constants.LogMsgType.Success, true);
                    
                    }

                    
                };

            Interlocked.Increment(ref nThreadsEnded);
            if (nThreadsEnded >= nThreads)
                {
                try
                    {
                    //listtmp.Append( listtmp.ToString( ) );
                    listtmp.Append( "</GROUP><NTOTAL>" + nGathered.ToString( ) + "</NTOTAL>" );
                    }
                catch (Exception ex)
                    {
                    gtmain.Log("exception in exit cleanup... " + ex.Message.ToString());
                    }
                if (StopGather == false)
                    {
                    //graceful termination
                    gtmain.Log(" Gather complete - all users gathered", Constants.LogMsgType.Success, true);
                    }
                else
                    {
                    //forced gather stop
                    gtmain.Log("Gather stopped - user break", Constants.LogMsgType.Error, true);
                    gtmain.Log(" ");
                    }
                if ( glist.Length != 0 )
                    gtmain.EnableGatherControls();
                IsGathering = false;
                gtmain.lblStatusGather.Text = "IDLE";
                gtmain.lblStatusGather.ForeColor = System.Drawing.Color.Red;

                
                }
            

                         


            }

        public void SkipFirstPage()
            {
            try
                {
                gtmain.Log( "skipping first page...", Constants.LogMsgType.Gator, false );
                GGDisk disk = new GGDisk( );
                int oldcount = disk.GetGatherTotal( listtmp );

                string tmpstr = listtmp.ToString( );
                int getptr = tmpstr.IndexOf( "<steamID64>" );
                if (getptr == -1)
                    return;
                string head = tmpstr.Substring( 0, getptr );
                string memberbuff = tmpstr.Substring( getptr );

                getptr = tmpstr.LastIndexOf ( "</GROUP>");
                string tail = tmpstr.Substring( getptr );
                string tmpbuff = memberbuff.Replace( "\x0d\x0a", string.Empty );
                memberbuff = tmpbuff;


                int i = fromgroup.ingame;
                int o = fromgroup.online - fromgroup.ingame;
                int off = fromgroup.total - fromgroup.online;
                int t = fromgroup.total;

               
                
                if ((t < 1000) || (nGathered < 1000))
                    {
                    fromgroup.ingame = fromgroup.total = fromgroup.online = 0;
                    listtmp.Length = 0;
                    if ( t < 1000 )
                        t = fromgroup.total;
                    if (nGathered < 1000)
                        {
                        t = nGathered < fromgroup.total ? nGathered : fromgroup.total;
                        }
                    
                    }
                else
                    {
                    t = 1000;
                    fromgroup.total -= 1000;

                    if (i < 1000) //i less than 1000, so it will be squashed. beating now goes to online
                        {
                        fromgroup.ingame = 0;
                        if (o < 1000) //o is not enough, it will be squashed, beating now goes onto offline
                            {
                            fromgroup.online = 0;

                            }
                        else
                            {//o takes the beating and beating ends
                            fromgroup.online -= 1000;
                            }

                        }
                    else // i is greater than 1000, can take the beating, beating stops
                        {
                        fromgroup.ingame -= 1000;
                        }
                    }

                int lengthOfOneMemberId = "<steamID64>".Length + "</steamID64>".Length + "76561198054213311".Length;
                int lengthToSkip = 1000 * lengthOfOneMemberId;
                if (lengthToSkip > memberbuff.ToString( ).Length)
                    lengthToSkip = memberbuff.ToString( ).Length;

                try
                    {
                    memberbuff = memberbuff.Substring( lengthToSkip );
                    }
                catch (Exception ex)
                    {
                    ;
                    }


                listtmp.Length = 0;
                listtmp.Append( head );
                listtmp.Append( memberbuff );
                //glist.Append( tail );
                int newcount = oldcount - 1000;
                if (newcount < 0)
                    newcount = 0;
                listtmp.Append( "</GROUP><NTOTAL>" + newcount.ToString( ) + "</NTOTAL>" );



                nGathered -= t;
                gtmain.lblUsable.Text = gtmain.lblIRemain.Text = nGathered.ToString( );
                nSkipGather += 1000;
                gtmain.Log( "done!" );
                }
            catch (Exception ex)
                {
                gtmain.Log( "exception in skip first page: " + ex.Message.ToString( ), Constants.LogMsgType.Error, true );
                //System.Windows.Forms.MessageBox.Show( "Exception: " + ex.Message.ToString( ), "Exception", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                }

            }

        public void SkipDupes()
            {
            try
                {
                string tmpstr = listtmp.ToString( );
                int getptr = tmpstr.IndexOf( "<steamID64>" );
                if (getptr == -1)
                    return;
                string head = tmpstr.Substring( 0, getptr );
                getptr = tmpstr.LastIndexOf( "</GROUP>" );
                string tail = tmpstr.Substring( getptr );

                gtmain.Log( "skipping duplicate ids...", Constants.LogMsgType.Gator, false );
                GGDisk disk = new GGDisk( );
                int oldcount = disk.GetGatherTotal( listtmp );

                int dupecount = 0;
                int beg, end;
                beg = end = -1;
                StringBuilder sbtmp = new StringBuilder( );
                sbtmp.Length = 0;

                string buff = listtmp.ToString( );
                beg = buff.IndexOf( "<steamID64>" );
                MyUtils.MyUtils utils = new MyUtils.MyUtils( );

                string tmp = string.Empty;
                while (beg != -1)
                    {
                    end = buff.IndexOf( "</steamID64>", beg );
                    end += "</steamID64>".Length;
                    tmp = string.Empty;
                    if ((end != -1) && (end - beg > 0))
                        {
                        tmp = buff.Substring( beg, end - beg );
                        if (!sbtmp.ToString( ).Contains( tmp ))
                            {
                            gtmain.Log( utils.GetStrBetween( tmp, "<steamID64>", "</steamID64>" ) + " is clean (added)", Constants.LogMsgType.Success, true );
                            sbtmp.Append( tmp );
                            }
                        else
                            {
                            gtmain.Log( utils.GetStrBetween( tmp, "<steamID64>", "</steamID64>" ) + " is a duplicate (removed)", Constants.LogMsgType.Error, true );
                            ++dupecount;
                            nGathered--;
                            nSkipGather++;
                            gtmain.lblUsable.Text = gtmain.lblIRemain.Text = nGathered.ToString( );
                            }
                        }
                    beg = buff.IndexOf( "<steamID64>", end );
                    }

                tail = tail.Replace( "<NTOTAL>" + oldcount.ToString( ) + "</NTOTAL>", "<NTOTAL>" + nGathered.ToString( ) + "</NTOTAL>" );
                listtmp.Length = 0;
                listtmp.Append( head );
                listtmp.Append( sbtmp.ToString( ) );
                listtmp.Append( tail );

                gtmain.lblUsable.Text = gtmain.lblIRemain.Text = nGathered.ToString( );
                
                if (dupecount != 0)
                    gtmain.Log( "done! " + dupecount.ToString( ) + " duplicate ids removed", Constants.LogMsgType.Gator, true );
                else
                    gtmain.Log( "done!" );
                }
            catch (Exception ex)
                {
                gtmain.Log( "exception in skip dupes " + ex.Message.ToString( ), Constants.LogMsgType.Error, true );
                //System.Windows.Forms.MessageBox.Show( "Exception: " + ex.Message.ToString( ), "Exception", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                
                }
               
            }

        public void SkipBlackListed(string togroupnew)
            {
            try
                {
                string tmpstr = listtmp.ToString( );
                int getptr = tmpstr.IndexOf( "<steamID64>" );
                if (getptr == -1)
                    return;
                string head = tmpstr.Substring( 0, getptr );
                getptr = tmpstr.LastIndexOf( "</GROUP>" );
                string tail = tmpstr.Substring( getptr );

                gtmain.Log( "skipping blacklisted ids...", Constants.LogMsgType.Gator, false );
                GGDisk disk = new GGDisk( );
                int oldcount = disk.GetGatherTotal( listtmp );

                blacklist = disk.Read( disk.dirinvite + "\\" + togroupnew );

                int dupecount = 0;
                int beg, end;
                beg = end = -1;
                StringBuilder sbtmp = new StringBuilder( );
                sbtmp.Length = 0;

                string buff = listtmp.ToString( );
                beg = buff.IndexOf( "<steamID64>" );
                MyUtils.MyUtils utils = new MyUtils.MyUtils( );

                string tmp = string.Empty;
                bool baddata = false;
                while (beg != -1)
                    {

                    end = buff.IndexOf( "</steamID64>", beg );
                    end += "</steamID64>".Length;

                    baddata = false;
                    tmp = string.Empty;
                    if ((end != -1) && (end - beg > 0))
                        {
                        tmp = buff.Substring( beg, end - beg );
                        baddata = false;
                        if ((blacklist.Contains( tmp )) && (tmp != string.Empty))
                            {
                            baddata = true;
                            gtmain.Log( utils.GetStrBetween( tmp, "<steamID64>", "</steamID64>" ) + " was blacklisted (removed)", Constants.LogMsgType.Error, true );
                            }
                        //if ((whitelist.Contains(tmp)) && (tmp != string.Empty))
                        //    {
                        //    baddata = true;
                        //    gtmain.Log(utils.GetStrBetween(tmp, "<steamID64>", "</steamID64>") + " was whitelisted (removed)", Constants.LogMsgType.Error, true);
                        //    }
                        if (baddata == false)
                            {
                            gtmain.Log( utils.GetStrBetween( tmp, "<steamID64>", "</steamID64>" ) + " is clean (added)", Constants.LogMsgType.Success, true );
                            sbtmp.Append( tmp );
                            }
                        else
                            {
                            ++dupecount;
                            nGathered--;
                            nSkipGather++;
                            gtmain.lblUsable.Text = gtmain.lblIRemain.Text = nGathered.ToString( );
                            }
                        }
                    beg = buff.IndexOf( "<steamID64>", end );
                    }
                listtmp.Length = 0;
                listtmp.Append( head );
                listtmp.Append( sbtmp.ToString( ) );               

                tail = tail.Replace( "<NTOTAL>" + oldcount.ToString( ) + "</NTOTAL>", "<NTOTAL>" + nGathered.ToString( ) + "</NTOTAL>" );
                listtmp.Append( tail );

                gtmain.lblUsable.Text = gtmain.lblIRemain.Text = nGathered.ToString( );
                
                if (dupecount != 0)
                    gtmain.Log( "done! " + dupecount.ToString( ) + " blacklisted ids removed", Constants.LogMsgType.Gator, true );
                else
                    gtmain.Log( "done!" );
                }
            catch (Exception ex)
                {
                gtmain.Log( "exception in skip blacklisted: " + ex.Message.ToString( ), Constants.LogMsgType.Error, true );
                //System.Windows.Forms.MessageBox.Show( "Exception: " + ex.Message.ToString( ), "Exception", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                
                }
            }

        public void GatherTypeAdjust(bool allowInGame, bool allowOnline, bool allowOffline)
            {
            try
                {
                string tmpstr = listtmp.ToString( );
                GGDisk disk = new GGDisk( );
                int oldcount = disk.GetGatherTotal( listtmp );

                int getptr = tmpstr.IndexOf( "<steamID64>" );
                if (getptr == -1)
                    return;
                string head = tmpstr.Substring( 0, getptr );
                string memberbuff = tmpstr.Substring( getptr );
                getptr = tmpstr.LastIndexOf( "</GROUP>" );
                string tail = tmpstr.Substring( getptr );
                memberbuff = memberbuff.Replace( tail, string.Empty );

                
                string tmpbuff = memberbuff.Replace( "\x0d\x0a", string.Empty );
                memberbuff = tmpbuff;

                //extract ingame members, online members and offline members seperately
                string strin = string.Empty;
                string stron = string.Empty;
                string stroff = string.Empty;
                string tmp = string.Empty;

                int lengthOfOneMemberId = "<steamID64>".Length + "</steamID64>".Length + "76561198054213311".Length;
                int offlen = fromgroup.online * lengthOfOneMemberId;
                int bufflen = memberbuff.ToString( ).Length;
                if (offlen > bufflen)
                    offlen = bufflen;
                int inlen = fromgroup.ingame * lengthOfOneMemberId;
                int onlen = fromgroup.online - fromgroup.ingame;
                onlen = onlen * lengthOfOneMemberId;
                if (onlen > bufflen)
                    onlen = bufflen;
                if (inlen > bufflen)
                    inlen = bufflen;

                if (onlen != 0)
                    {
                    try
                        {
                        stroff = memberbuff.Substring( onlen );
                        tmp = memberbuff.Substring( 0, onlen );
                        }
                    catch (Exception ex)
                        {
                        ;
                        }
                    }

                try
                    {
                    if (inlen != 0)
                        {
                        strin = tmp.Substring( 0, inlen );
                        stron = tmp.Substring( inlen );
                        }
                    }
                catch (Exception ex)
                    {
                    ;
                    }



                listtmp.Length = 0;
                listtmp.Append( head );
                int newtotal = oldcount;
                if (allowInGame == false)
                    {
                    gtmain.Log( "removed ingame memebers.." );
                    newtotal -= fromgroup.ingame;
                    nGathered -= fromgroup.ingame;
                    }
                else
                    {
                    gtmain.Log( "included ingame members.." );
                    listtmp.Append( strin );
                    }
                if (allowOnline == false)
                    {
                    gtmain.Log( "removed online memebers.." );
                    newtotal -= ( fromgroup.online - fromgroup.ingame );
                    nGathered -= (fromgroup.online - fromgroup.ingame);
                    
                    }
                else
                    {
                    gtmain.Log( "included online members.." );
                    listtmp.Append( stron );
                    }
                if (allowOffline == false)
                    {
                    gtmain.Log( "removed offline memebers.." );
                    newtotal -= (fromgroup.total - fromgroup.online);
                    nGathered -= (fromgroup.total - fromgroup.online);
                    }
                else
                    {
                    gtmain.Log( "included offline members.." );
                    listtmp.Append( stroff );
                    }
                listtmp.Append( "</GROUP><NTOTAL>" + newtotal.ToString( ) + "</NTOTAL>" );

                gtmain.lblUsable.Text = gtmain.lblIRemain.Text = nGathered.ToString( );
                

                }
            catch (Exception ex)
                {
                gtmain.Log( "exception in gather type adjust: " + ex.Message.ToString( ), Constants.LogMsgType.Error, true );
                //System.Windows.Forms.MessageBox.Show( "Exception: " + ex.Message.ToString( ), "Exception", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                
                }
            }
        public void CreateList ()
            {
            if (listtmp.Length != 0)
                {
                glist.Append( listtmp.ToString( ) );
                listtmp.Length = 0;
                }
            }
        public void AdjustVars(int pageno, ref bool shouldskip, ref int ntoskip, ref bool skipfrombeg, ref bool skipthispage )
            {
            shouldskip = false;
            ntoskip = 0;
            skipfrombeg = false;
            skipthispage = false;

            switch (gathertype)
                {
                case 0:
                    allgathered = true;
                    break;
                case 1: //offline only                    
                    if (pageno == offlimit)
                        {
                        if (fromgroup.online % 1000 == 0)
                            skipthispage = true;
                        shouldskip = true;
                        ntoskip = fromgroup.online % 1000;
                        skipfrombeg = true;
                        }
                    else if (pageno < offlimit)
                        skipthispage = true;
                    else if (pageno > fromgroup.npages)
                        {
                        allgathered = true;
                        skipthispage = true;
                        }
                    break;
                case 2: //online only
                    if (pageno > onlimit)
                        {
                        allgathered = true;
                        skipthispage = true;
                        }
                    else if (pageno == onlimit)
                        {
                        if (fromgroup.online % 1000 == 0)
                            skipthispage = true;

                        shouldskip = true;
                        ntoskip = 1000 - ( fromgroup.online % 1000 );
                        skipfrombeg = false;
                        }
                    break;
                case 3: // online + offline
                    if (pageno == inglimit)
                        {
                        if (fromgroup.ingame % 1000 == 0)
                            skipthispage = true;

                        shouldskip = true;
                        ntoskip = fromgroup.ingame % 1000;
                        skipfrombeg = true;

                        }
                    else if (pageno < inglimit)
                        skipthispage = true;
                    else if (pageno > fromgroup.npages)
                        {
                        allgathered = true;
                        skipthispage = true;
                        }

                    break;
                case 4: //ingame
                    if ( pageno == inglimit )
                        {
                        if ( fromgroup.ingame % 1000 == 0 )
                            skipthispage = true;
                       
                        shouldskip = true;
                        ntoskip = 1000 - ( fromgroup.ingame % 1000 );
                        skipfrombeg = false;                        
                            
                        }
                    else if (pageno > inglimit)
                        {
                        allgathered = true;
                        skipthispage = true;
                        }
                    break;
                case 5: //ingame + offline
                    if ( pageno == inglimit )
                        {
                        if ( fromgroup.ingame % 1000 == 0 )
                            skipthispage = true;

                        shouldskip = true;
                        ntoskip = 1000 - ( fromgroup.ingame % 1000 );
                        skipfrombeg = false;                        
                            
                        }
                    else if (pageno == offlimit)
                        {
                        if (fromgroup.online % 1000 == 0)
                            skipthispage = true;
                        shouldskip = true;
                        ntoskip = fromgroup.online % 1000;
                        skipfrombeg = true;
                        }
                    else if ( (pageno > inglimit ) && ( pageno < offlimit ))
                        skipthispage = true;
                    else if (pageno > fromgroup.npages)
                        {
                        allgathered = true;
                        skipthispage = true;
                        }
                    break;
                case 6://ingame + online
                    if (pageno > onlimit)
                        {
                        allgathered = true;
                        skipthispage = true;
                        }
                    else if (pageno == onlimit)
                        {
                        if (fromgroup.online % 1000 == 0)
                            skipthispage = true;

                        shouldskip = true;
                        ntoskip = 1000 - ( fromgroup.online % 1000 );
                        skipfrombeg = false;
                        }

                    break;
                case 7:        //all   
                    if (pageno > fromgroup.npages)
                        {
                        allgathered = true;
                        skipthispage = true;
                        }
                    else
                        skipthispage = false;
                    break;
                }
            }
                
        private string BlackListCheck(string page, ref int curadd )
            {
            string ret = string.Empty;
            int beg, end;
            beg = end = -1;
            beg = page.IndexOf("<steamID64>");
            GGDisk disk = new GGDisk();
                     
            string tmp = string.Empty;
            bool baddata = false;
            while (beg != -1)
                {
                if (StopGather == true)
                    break;
                beg = beg +  "<steamID64>".Length;
                end = page.IndexOf ( "</steamID64>", beg );
                tmp = string.Empty;
                if ((end != -1) && (end - beg > 0))
                    {
                    tmp = page.Substring(beg, end - beg);
                    baddata = false;
                    if ((blacklist.Contains(tmp)) && (tmp != string.Empty))
                        {
                        baddata = true;
                        GatherMessageLog(tmp, Constants.GatherError.IDBlackListed);
                        //disk.Append(tmp + " - " + Environment.NewLine, "blacklisted.txt");
                        }
                    if ((whitelist.Contains(tmp)) && (tmp != string.Empty))
                        {
                        baddata = true;
                        GatherMessageLog(tmp, Constants.GatherError.IDAlreadyGathered);
                        //disk.Append(tmp + " - " + Environment.NewLine, "whitelisted.txt");
                        }
                    if (baddata == false)
                        {
                        ret = ret + "<steamID64>" + tmp + "</steamID64>";
                        GatherMessageLog(tmp, Constants.GatherError.Success);
                        lock (_lockerNG)
                            {
                            nGathered++;
                            gtmain.lblGatherTotal.Text = nGathered.ToString();
                            }
                        disk.WhiteListIt(fromgroup, tmp);
                        }
                    else
                        {
                        lock (_locknskip)
                            {
                            nSkipGather++;
                            //gtmain.lblSkipped.Text = nSkipGather.ToString();
                            }
                        }
                    }
                else
                    break;
                beg = page.IndexOf("<steamID64>", end);
                };

            return ret;
            }

        public void GatherStop()
            {
            StopGather =true;
            }
        public void SaveGatherList ( )
            {
            if (listtmp.Length != 0)
                {
                glist.Append ( listtmp.ToString());
                listtmp.Length = 0;

                }
            GGDisk ggdisk = new GGDisk();
            ggdisk.SaveGatherList(glist);
            saved = true;
            }
        public void LoadGatherList()
            {
            GGDisk ggdisk = new GGDisk();
            int ret = ggdisk.LoadGatherList(ref glist);
            if (ret == -1)
                {
                //user cancelled list-load dialog
                }
            else
                {
                nGathered = ret;
                }
            
            }
        public void SetGatherType(int gtype)
            {            
            gathertype = gtype;
            }

        private void GatherMessageLog(string id, Constants.GatherError gerr)
            {
            switch (gerr)
                {
                case Constants.GatherError.Success:
                    gtmain.Log(" GATHERING ID : ", Constants.LogMsgType.Success, false);
                    gtmain.Log(id, Constants.LogMsgType.Success, true);
                    gtmain.Log(" ID GATHERED SUCCESSFULLY!", Constants.LogMsgType.Success, true);
                    gtmain.Log(" ADDED TO ACTIVE GATHER LIST", Constants.LogMsgType.Success, true);
                    gtmain.Log(" ");                            
                    break;
                case Constants.GatherError.IDAlreadyGathered:
                    gtmain.Log(" GATHERING ID : ", Constants.LogMsgType.Error, false);
                    gtmain.Log(id, Constants.LogMsgType.Error, true);
                    gtmain.Log(" ID GATHER FAILED! - ALREADY GATHERED", 0, true);
                    gtmain.Log(" NOT ADDED TO ACTIVE GATHER LIST", 0, true);
                    gtmain.Log(" ");
                    break;
                case Constants.GatherError.IDBlackListed:
                    gtmain.Log(" GATHERING ID : ", Constants.LogMsgType.Error, false);
                    gtmain.Log(id, Constants.LogMsgType.Error, true);
                    gtmain.Log(" ID GATHER FAILED! - ID BLACKLISTED", Constants.LogMsgType.Error, true);
                    gtmain.Log(" NOT ADDED TO ACTIVE GATHER LIST", Constants.LogMsgType.Error, true);
                    gtmain.Log(" ");                            
                    break;
                }

            }
        public void RandomizeList()
            {
            //Gator25.ListRandomizer rander = new Gator25.ListRandomizer(glist);
            //glist = rander.Display();
            ThreadStart tsrnd = new ThreadStart( RandProc );
            Thread trnd = new Thread( tsrnd );
            trnd.IsBackground = true;
            trnd.Start( );

            }
        private void RandProc ( )
            {

            int iterations = 20000;
            int stepsmall = 10;
            int totalstepsmall = gtmain.pbarCreateGL.Width / 10;
            int stepbig = iterations / totalstepsmall;

            string list1 = string.Empty;
            string list2 = string.Empty;
            int middle = listtmp.ToString( ).Length / 2;
            if (middle < 160)
                return;
            string checkstrend = "</steamID64>";
            string checkstrbeg = "<steamID64>";
            int beg = listtmp.ToString( ).IndexOf( checkstrend, middle );
            if (beg == -1)
                {
                checkstrend = "</UID>";
                checkstrbeg = "<UID>";
                beg = listtmp.ToString( ).IndexOf( checkstrend, middle );
                if (beg == -1)
                    return;
                }

            beg += checkstrend.Length;
            list1 = listtmp.ToString( ).Substring( 0, beg );
            list2 = listtmp.ToString( ).Substring( beg );

            int start1, stop1, start2, stop2, len1, len2;
            start1 = stop1 = start2 = stop2 = -1;
            len1 = list1.Length;
            len2 = list2.Length;

            string str1, str2, tmp;
            str1 = str2 = tmp = string.Empty;

            int tick1 = Environment.TickCount;



            for (int i = 1; i <= iterations; i++)
                {
                Random rand = new Random( Environment.TickCount );
                start1 = rand.Next( 0, len1 - 40 );
                start1 = list1.IndexOf( checkstrbeg, start1 );
                if (start1 == -1)
                    goto label;
                stop1 = list1.IndexOf( checkstrend, start1 );
                if (stop1 == -1)
                    goto label;
                stop1 += checkstrend.Length;
                str1 = list1.Substring( start1, stop1 - start1 );

                rand = new Random( Environment.TickCount );
                start2 = rand.Next( 0, len2 - 40 );
                start2 = list2.IndexOf( checkstrbeg, start2 );
                if (start2 == -1)
                    goto label;
                stop2 = list2.IndexOf( checkstrend, start2 );
                if (stop2 == -1)
                    goto label;
                stop2 += checkstrend.Length;
                str2 = list2.Substring( start2, stop2 - start2 );

                list1 = list1.Replace( str1, str2 );
                list2 = list2.Replace( str2, str1 );

                label:
                double dstep1 = (i / iterations) * 100;
                //double dstep2 =  dstep1 / 100;
                int iitmp = Convert.ToInt32( dstep1 );
                if (iitmp == 0)
                    iitmp = 1;

                gtmain.pbarCreateGL.Value = iitmp;
                gtmain.pbarCreateGL.PerformStep( );
                gtmain.pbarCreateGL.Refresh( );
                
                }
            listtmp.Length = 0;
            listtmp.Append( list1 );
            listtmp.Append( list2 );
            int tick2 = Environment.TickCount;
            int tim = (tick2 - tick1) / 1000;
            
            
            }

#endregion

        private Constants.LimitOver IsLimitOver ( Constants.LimitOver limitToCheck )
            {
            WebService ws = new WebService( );
            WebService.ServerData sd = ws.CheckUsage( );
            Constants.LimitOver ret = Constants.LimitOver.NotOver;
            switch (limitToCheck)
                {
                case Constants.LimitOver.GatherLimitOver:
                    if (sd.gremain <= 0)
                        ret = Constants.LimitOver.GatherLimitOver;
                    break;
                case Constants.LimitOver.InviteLimitOver:
                    if (sd.iremain <= 0)
                        ret = Constants.LimitOver.InviteLimitOver;
                    break;
                case Constants.LimitOver.TimeLimitOver :
                    if (sd.tremain <= 0)
                        ret = Constants.LimitOver.TimeLimitOver;
                    break;

                }

            return ret;
            }

        #region invite
        private bool StopInvite;
        public bool IsInviting;
        public int nInvited = 0;
        public int nSkipInvite = 0;
        private string inviter;
        public string lastinvitedid = "-1";
        public string cookie;
        public static string sessionid = string.Empty;
        private bool IsRandomWait = false;
        private int CurInviteWait = 5;
        private int CurFailWait = 5;
        private int MaxWait = 20;
        private int MaxIPH = 100;
        private int nglobalinvites = 0;

        public void Invite( string to, string cuki, string sessid, Profile profile)
            {
            
            if (IsInviting == false)
                {
                if (IsGathering == false)
                    {
                    if (listtmp.Length != 0)
                        {
                        glist.Append( listtmp.ToString( ) );
                        listtmp.Length = 0;
                        }

                    prof = profile;
                    IsInviting = true;
                    //SetFromGroup(frm);
                    SetToGroup(to);
                    try
                        {
                        gtmain.wbSteamHistory.Navigate( "http://steamcommunity.com/groups/" + togroup.url );
                        }
                    catch (Exception ex)
                        {
                        ;
                        }
                    StopInvite = false;
                    nInvited = 0;
                    nSkipInvite = 0;
                    cookie = cuki;
                    sessionid = sessid;

                    if (glist.ToString().Contains("<GGV3>"))
                        isversion3 = true;
                    else
                        isversion3 = false;

                    WebService ws = new WebService( );
                    WebService.ServerData sd = new WebService.ServerData( );
                    sd = ws.CheckUsage( );
                    nICur = 0;
                    
                    if (license == Constants.SoftLicense.Demo)
                        {
                        if (sd.iremain <= 0)
                            {
                            
                            System.Windows.Forms.MessageBox.Show( "Your 1000 free invite limit for demo version has reached! Please donate for unlimited usage. Thank You", "Free invite limit reached!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                            Environment.Exit( 0 );
                            }
                        }
                    else
                        {
                        if (sd.tremain <= 0)
                            {

                            System.Windows.Forms.MessageBox.Show( "Your subscription period has expired. Please renew for continued usage. Thank you", "Subscription Expired!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                            Environment.Exit( 0 );
                            }
                        }

                    Thread thInvite;
                    ThreadStart thInviteStart;

                    thInviteStart = new ThreadStart(InviteProc);
                    thInvite = new Thread(thInviteStart);
                    thInvite.IsBackground = true;
                    thInvite.Start();
                    gtmain.Log("Starting Invite To Group " + togroup.url);                     
                    }
                else
                    {
                    //a gather in progress
                    gtmain.Log("Invite error: a gather process is in progress.. Please wait for it to complete.", Constants.LogMsgType.Error, true);                    
                    return;
                    }
                }
            else
                {
                //another invite
                gtmain.Log("Invite error: an invite process is in progress.. Please wait for it to complete.", Constants.LogMsgType.Error, true);
                return;
                }
            }
        public void InviteStop()
            {
            StopInvite = true;
            }
        public static bool isversion3 = false;
        public void InviteProc()
            {
            int tick1 = Environment.TickCount;
            int elapsedtime = 0;
            int tick2 = 0;

            GGDisk disk = new GGDisk();
            int beg, end;
            beg = end = -1;
            string lstid = disk.GetLastInvitee(glist );
            if (lstid == "-1")   //no last invitee. Fresh invite from beginning.
                {
                if ( isversion3 == false )
                    beg = glist.ToString().IndexOf("<UID>");
                else
                    beg = glist.ToString().IndexOf("<steamID64>");
                }
            else//last invitee is saved in list. So start from last invitee
                {
                if (isversion3 == false)
                    {
                    beg = glist.ToString().IndexOf(lstid + "</UID>");
                    if ( beg != -1 )
                        beg = glist.ToString().IndexOf("<UID>", beg);
                    else
                        beg = beg = glist.ToString( ).IndexOf( "<UID>" );
                    }
                else
                    {
                    beg = glist.ToString().IndexOf(lstid + "</steamID64>");
                    if ( beg != -1 )
                        beg = glist.ToString().IndexOf("<steamID64>", beg);
                    else
                        beg = glist.ToString( ).IndexOf( "<steamID64>" ); 
                    }
                }
            string tmp = string.Empty;
            while (beg != -1)
                {
                if (StopInvite == true)
                    break;
                if (license  ==  Constants.SoftLicense.Demo)
                    {
                    if (nInvited >= 1000)
                        {
                        //invite limit over for free version
                        break;
                        }
                    }

                if (isversion3 == false)
                    {
                    beg = beg + "<UID>".Length;
                    end = glist.ToString().IndexOf("</UID>", beg);
                    }
                else
                    {
                    beg = beg + "<steamID64>".Length;
                    end = glist.ToString().IndexOf("</steamID64>", beg);
                    }
                if ( ( beg != -1 ) && ( end != -1 ) && ( ( end - beg ) >= 0 ) )
                    {                    
                    tmp = glist.ToString().Substring(beg, end - beg);
                    lastinvitedid = tmp;
                    if (disk.IsBlackListed(togroup, tmp) == false)
                        {
                        if (SteamOnline == false)
                            {
                            gtmain.Log(" ");
                            gtmain.Log("STEAM IS OFFLINE - GATOR WILL PAUSE INVITE NOW AND WILL RESUME WHEN STEAM IS BACK ONLINE!", Constants.LogMsgType.Error , true);
                            gtmain.Log("waiting for steam..", Constants.LogMsgType.Error, false);
                            }
                        while ( SteamOnline  == false)
                            {
                            gtmain.Log("..", Constants.LogMsgType.Error, false);
                            System.Threading.Thread.Sleep(5000);
                            if (StopInvite == true)
                                break;
                            gtmain.auth.UpdateSessionID();
                            sessionid = gtmain.auth.sessionid; gtmain.auth.UpdateSessionID();
                            };

                        string errmsg = string.Empty;

                        tick2 = Environment.TickCount;
                        elapsedtime = ( tick2 / tick1 ) / 1000;
                        if (elapsedtime > 3600)
                            {
                            elapsedtime = 0;
                            tick1 = tick2;
                            }
                        else if ( elapsedtime <= 3600 )
                            {
                            if ((nInvited + nSkipInvite) > MaxIPH)
                                {
                                int itmp = 3600 - elapsedtime;
                                gtmain.Log("Your current limit for maximum invites per hour has reached. Invite process will pause now for " + itmp.ToString()+ " seconds before you can continue inviting..", Constants.LogMsgType.Error, true);
                                System.Threading.Thread.Sleep( itmp);
                                gtmain.auth.UpdateSessionID();
                                sessionid = gtmain.auth.sessionid;
                                }
                            }
                        Constants.InviteErrors inverr = InviteID(togroup, tmp, ref errmsg );
                        if ( inverr == Constants.InviteErrors.Fail )
                            {
                            //failed invite.. re=add to gatherlist
                            InviteMessageLog(tmp, Constants.InviteError.Failed, errmsg);
                            disk.AddToGatherList(ref glist, tmp);
                            nSkipInvite++;
                            gtmain.lblFailed.Text = nSkipInvite.ToString();
                            gtmain.IncrementBlackListTotal();                            
                            disk.BlackListIt(togroup, tmp);
                            RemoveID(tmp);
                            DelayWithLog(Constants.InviteError.Failed);                            
                            }
                        else if ( inverr == Constants.InviteErrors.Success )
                            {
                            if (license == Constants.SoftLicense.Demo)
                                {
                                nICur += 1;
                                gtmain.DecIRemain( );
                                if (gtmain.iremain <= 0)
                                    {
                                    gtmain.Log( "allowed invite limit of 1000 exceeded for free version, please donate for continued usage", Constants.LogMsgType.Error, true );
                                    StopInvite = true;
                                    System.Windows.Forms.MessageBox.Show( "allowed invite limit of 1000 exceeded for free version, please donate for continued usage", "free version invite limit reached", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                                    break;
                                    }
                                }
                            disk.BlackListIt(togroup, tmp);
                            gtmain.IncrementBlackListTotal();
                            nInvited++;
                            nglobalinvites++;
                            gtmain.lblSuccessful.Text = nInvited.ToString();
                            gtmain.IncrementInviteTotal();

                            InviteMessageLog(tmp, Constants.InviteError.Success, errmsg);
                            RemoveID(tmp);
                            DelayWithLog(Constants.InviteError.Success);
                            
                            }
                        else if (inverr == Constants.InviteErrors.AlreadyInvited)
                            {
                            InviteMessageLog(tmp, Constants.InviteError.Failed, errmsg);
                            nSkipInvite++;
                            gtmain.IncrementBlackListTotal();
                            gtmain.lblFailed.Text = nSkipInvite.ToString();
                            disk.BlackListIt(togroup, tmp);
                            RemoveID(tmp);
                            DelayWithLog(Constants.InviteError.Failed);
                            }
                        else if (inverr == Constants.InviteErrors.Error)
                            {
                            InviteMessageLog(tmp, Constants.InviteError.Failed, errmsg);
                            nSkipInvite++;
                            gtmain.IncrementBlackListTotal();
                            gtmain.lblFailed.Text = nSkipInvite.ToString();
                            disk.BlackListIt(togroup, tmp);
                            RemoveID(tmp);
                            DelayWithLog(Constants.InviteError.Failed);                            

                            }
                        else if (inverr == Constants.InviteErrors.NoPermission)
                            {
                            InviteMessageLog(tmp, Constants.InviteError.Failed, errmsg);
                            nSkipInvite++;
                            gtmain.IncrementBlackListTotal();
                            gtmain.lblFailed.Text = nSkipInvite.ToString();
                            disk.BlackListIt(togroup, tmp);
                            RemoveID(tmp);
                            DelayWithLog(Constants.InviteError.Failed);                            
                            }
                        else if (inverr == Constants.InviteErrors.Declined)
                            {
                            InviteMessageLog(tmp, Constants.InviteError.Failed, errmsg);
                            nSkipInvite++;
                            gtmain.IncrementBlackListTotal();
                            gtmain.lblFailed.Text = nSkipInvite.ToString();
                            disk.BlackListIt(togroup, tmp);
                            RemoveID(tmp);
                            DelayWithLog(Constants.InviteError.Failed);                            
                            }


                        }
                    else
                        {
                        //id already blacklisted
                        InviteMessageLog(tmp, Constants.InviteError.IDAlreadyInvited, string.Empty);
                        nSkipInvite++;
                        gtmain.IncrementBlackListTotal();
                        gtmain.lblFailed.Text = nSkipInvite.ToString();
                        }
                    }
                if (isversion3 == false)
                    beg = glist.ToString().IndexOf("<UID>", end);
                else
                    beg = glist.ToString().IndexOf("<steamID64>", end);
                int itotaltmp = nSkipInvite + nInvited;
                gtmain.lblTotalISent.Text = itotaltmp.ToString();
                };

            
            if (StopInvite == false)
                {               
                GGDisk ggapp = new GGDisk();
                ggapp.SetLastInvitee(ref glist, "-1");
                gtmain.Log(" Invite complete - all users invited", Constants.LogMsgType.Success, true);
              
                //graceful completion
                }
            else
                {
                //forced exit
                GGDisk ggapp = new GGDisk();
                ggapp.SetLastInvitee(ref glist, lastinvitedid);                
                gtmain.Log("Invite stopped - user break", Constants.LogMsgType.Error, true);
                gtmain.Log(" ");
                }
            IsInviting = false;
            gtmain.lblStatusInvite.Text = "IDLE";
            }
        private void DelayWithLog(Constants.InviteError ierr )
            {
            string choice = string.Empty;
            if (ierr == Constants.InviteError.Failed )
                {
                choice = "Set Invite Speed - ";
                gtmain.Log( choice, Constants.LogMsgType.Gator, false);
                gtmain.Log("Waiting ", Constants.LogMsgType.Success, false);
                gtmain.Log(CurFailWait.ToString(), Constants.LogMsgType.Error, false);
                gtmain.Log(" Seconds... ", Constants.LogMsgType.Success, true);
                gtmain.Log(" ");
                System.Threading.Thread.Sleep(CurFailWait * 1000);                
                }
            else if (ierr == Constants.InviteError.Success)
                {
                if (IsRandomWait == true)
                    {
                    gtmain.Log("Randomizing Invite Speed - ", Constants.LogMsgType.Gator, false);
                    gtmain.Log("Waiting ", Constants.LogMsgType.Success, false);
                    gtmain.Log(GetRandomSpeed().ToString(), Constants.LogMsgType.Error, false);
                    gtmain.Log(" Seconds... ", Constants.LogMsgType.Success, true);
                    gtmain.Log(" ");
                    System.Threading.Thread.Sleep(GetRandomSpeed() * 1000);                    
                    }
                else
                    {
                    gtmain.Log("Set Invite Speed - ", Constants.LogMsgType.Gator, false);
                    gtmain.Log("Waiting ", Constants.LogMsgType.Success, false);
                    gtmain.Log(CurInviteWait.ToString(), Constants.LogMsgType.Error, false);
                    gtmain.Log(" Seconds... ", Constants.LogMsgType.Success, true);
                    gtmain.Log(" ");
                    System.Threading.Thread.Sleep(CurInviteWait * 1000);                
                    }
                }            
            }
        public Constants.InviteErrors InviteID(Group to, string invitee, ref string errmsg)
            {
            NameValueCollection nvm = new NameValueCollection();
            nvm.Clear();
            nvm.Clear();
            nvm["xml"] = "1";
            nvm["type"] = "groupInvite";
            nvm["inviter"] = inviter;
            nvm["invitee"] = invitee;
            nvm["group"] = to.id;
            nvm["sessionID"] = System.Web.HttpUtility.UrlDecode(sessionid);

            string target = "http://steamcommunity.com/actions/GroupInvite";

            MyWeb.MyWeb myweb = new MyWeb.MyWeb();
            MyUtils.MyUtils utils = new MyUtils.MyUtils();

            string resp = string.Empty;
            while (resp == string.Empty)
                {
                resp = myweb.PostPage(target, nvm, 0, 0, cookie);
                if (resp == string.Empty)
                    System.Threading.Thread.Sleep(2000);
                else
                    break;
                }
            string stmsg = string.Empty;
            stmsg = utils.GetStrBetween(resp, "[CDATA[", "]]");
            errmsg = stmsg;
            if (stmsg.Contains("OK"))
                return Constants.InviteErrors.Success;
            else if (stmsg.Contains("user may have already been invited"))
                return Constants.InviteErrors.AlreadyInvited;
            else if (stmsg.Contains("chosen not to join that group"))
                return Constants.InviteErrors.Declined;
            else if (stmsg.Contains("#Error"))
                return Constants.InviteErrors.Error;
            else if (stmsg.Contains("do not have permission to invite"))
                return Constants.InviteErrors.NoPermission;
            else
                {
                gtmain.auth.UpdateSessionID();
                sessionid = gtmain.auth.sessionid;
                return Constants.InviteErrors.Fail;
                }
            }
        public void RemoveID(string uid)
            {
            nGathered--;
            GGDisk disk = new GGDisk();
            int icur = disk.GetGatherTotal(glist);
            int inew = icur - 1;
            string buff = string.Empty;
            if ( isversion3 == false )  
                buff = glist.ToString().Replace("<UID>" + uid + "</UID>", string.Empty);
            else
                buff =  glist.ToString().Replace("<steamID64>" + uid + "</steamID64>", string.Empty);
            buff = buff.Replace("<NTOTAL>" + icur.ToString() + "</NTOTAL>", "<NTOTAL>" + inew.ToString() + "</NTOTAL>");
            glist.Length = 0;
            glist.Append(buff);
            gtmain.lblIRemain.Text = inew.ToString();
            ////gtmain.lblUsable.Text = inew.ToString();
            
            }
        public void InviteClear()
            {
            if (IsInviting == true)
                InviteStop();
            
            nSkipGather = 0;
            nInvited = 0;
            gtmain.lblSuccessful.Text = "0";
            gtmain.lblFailed.Text = "0";
            gtmain.lblTotalISent.Text = "0";
            GGDisk disk = new GGDisk();
            while (IsInviting == true)
                {
                System.Threading.Thread.Sleep(500);
                }
            disk.RemoveBlackList(togroup);
            }
        public bool SetInviteSpeed(Constants.StructInviteDelay sdelay)
            {            
            IsRandomWait = sdelay.israndom;
            CurInviteWait = sdelay.curdelay;
            CurFailWait = sdelay.faildelay;
            MaxIPH = sdelay.maxiph;
            MaxWait = sdelay.maxwait;
            
            return true;
            }
        private int GetRandomSpeed()
            {

            int ret = 15;
            try
                {
                Random rand = new Random(Environment.TickCount);
                ret = (rand.Next(CurInviteWait, MaxWait));
                }
            catch (Exception ex)
                {
                gtmain.Log("Exception random gen: " + ex.Message.ToString(), Constants.LogMsgType.Error, true);
                ret = 15;
                }

            return ret;
            }
        private void InviteMessageLog(string id, Constants.InviteError ierr, string errmsg)
            {
            switch (ierr)
                {
                case Constants.InviteError.Success:
                    gtmain.Log("INVITING ID: ", Constants.LogMsgType.Gator, false);
                    gtmain.Log(id, Constants.LogMsgType.Success, true);
                    gtmain.Log("INVITE: ", Constants.LogMsgType.Gator, false);
                    gtmain.Log("SUCCESSFUL!", Constants.LogMsgType.Success, true);
                    gtmain.Log("STEAM RESPONSE: ", Constants.LogMsgType.Gator, false);
                    gtmain.Log(errmsg, Constants.LogMsgType.Success, true);
                    gtmain.Log("ID BLACKLISTED", Constants.LogMsgType.Success, true); 
                    break;
                case Constants.InviteError.Failed:
                    gtmain.Log("INVITING ID: ", Constants.LogMsgType.Gator, false);
                    gtmain.Log(id, Constants.LogMsgType.Success, true);
                    gtmain.Log("INVITE: ", Constants.LogMsgType.Gator, false);
                    gtmain.Log("FAILED!", Constants.LogMsgType.Error, true);
                    gtmain.Log("STEAM RESPONSE: ", Constants.LogMsgType.Gator, false);
                    gtmain.Log(errmsg, Constants.LogMsgType.Error, true);
                    gtmain.Log("ID BLACKLISTED", Constants.LogMsgType.Success, true);
                    break;
                case Constants.InviteError.IDAlreadyInvited:
                    gtmain.Log("INVITING ID: ", Constants.LogMsgType.Gator, false);
                    gtmain.Log(id, Constants.LogMsgType.Success, true);
                    gtmain.Log("INVITE: ", Constants.LogMsgType.Gator, false);
                    gtmain.Log("FAILED!", Constants.LogMsgType.Error, true);
                    gtmain.Log("ALREADY INVITED - USER ID EXISTS IN BLACKLIST", 0, true);
                    gtmain.Log("ID SKIPPED", Constants.LogMsgType.Error, true);
                    gtmain.Log(" ");
                    break;
                }
            }
        #endregion

        }
    }

//int residue  = 1000;
//if ( (gathertype == Constants.GatherType.InGame) && (i == pagecount ) )
//    {                    
//    residue = fromgroup.ingame % 1000;
//    beg = ("<steamID64>76561198021307765</steamID64>".Length) * residue;
//    if ( (block.Substring (beg-1,"<steamID64>".Length ))!= "<steamID64>")
//        beg = block.IndexOf ("<steamID64>", beg );
//    block = block.Substring (0, beg);
//    }
//else if ((gathertype == Constants.GatherType.InGame) && (i == pagecount))
//    {
//    residue = fromgroup.online % 1000;
//    beg = ("<steamID64>76561198021307765</steamID64>".Length) * residue;
//    if ((block.Substring(beg - 1, "<steamID64>".Length)) != "<steamID64>")
//        beg = block.IndexOf("<steamID64>", beg);
//    block = block.Substring(0, beg);
//    }


//We apologize but we are currently experiencing some technical issues, and services are currently unavailable. If you are clicking on an advertisement, please try again later. If you are a current publisher, once service is restored -- which we expect to take a few hours -- we will be applying a credit to your account because of the downtime. Thank you for your understanding and patience.