using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;


namespace Gator
    {
    class GGDisk
        {
        public string dirapp;
        public string dirconfig;
        public string dirlists;
        public string dirgather;
        public string dirinvite;
        public string dirupd;
        public string dirupdtmp;
        public string dirplugin;
        public string dirimgs;
        public string fileupd;
        public string filetogrps;
        public string filefromgrps;
        public string filelogin;
        public string filelog;
        //public string filerecaptcha;
        public string filecfg;
        public string filedefcreat;
        public string filedbdetails;
        
        //public string filetmpcaptcha;

        public string filedonator1;
        public string filedonator2;
        public string filedonator3;
        public string filedonator4;
        public string filedonator5;
        public string filedonator6;

        //public string filecaptchahtml;
        public string filesteamcaptcha;
        public string fileggcaptcha;
        public string fileprices;

        public GGDisk()
            {
            string path = Path.GetFullPath(Application.ExecutablePath);
            int beg = path.LastIndexOf("\\");
            dirapp = path.Substring(0, beg);
            dirconfig = dirapp + "\\config";
            dirlists = dirapp + "\\GatherLists";
            dirgather = dirapp + "\\gather";
            dirinvite = dirapp + "\\invite";
            dirupd = dirapp + "\\update";
            dirupdtmp = dirupd + "\\tmp";
            dirplugin = dirapp + "\\plugins";
            dirimgs = dirconfig + "\\images";

            fileupd = dirupdtmp + "\\setup.exe";
            filecfg = dirconfig + "\\cfg";
            filetogrps = dirconfig + "\\togrps";
            filefromgrps = dirconfig + "\\frmgrps";
            filelogin = dirconfig + "\\login.html";
            filelog = dirconfig + "\\activity.log";
            //filerecaptcha = dirconfig + "\\recaptcha.html";
            //filetmpcaptcha = dirconfig + "\\captcha.png";
            //filecaptchahtml = dirconfig + "\\captcha.html";
            filesteamcaptcha = dirconfig + "\\steamcaptcha.png";
            fileggcaptcha = dirconfig + "\\ggcaptcha.png";

            filedonator1 = dirconfig + "\\donor1.html";
            filedonator2 = dirconfig + "\\donor2.html";
            filedonator3 = dirconfig + "\\donor3.html";
            filedonator4 = dirconfig + "\\donor4.html";
            filedonator5 = dirconfig + "\\donor5.html";
            filedonator6 = dirconfig + "\\donor6.html";

            filedefcreat = dirimgs + "\\default.gif";
            fileprices = dirconfig + "\\prices";
            filedbdetails = dirconfig + "\\dbdetails";

            //Write(Gator25.Properties.Resources.steamcaptcha, filecaptchahtml);
            //WritePayPalFiles();
            try
                {
                if (!Directory.Exists(dirgather))
                    Directory.CreateDirectory(dirgather);
                if (!Directory.Exists(dirinvite))
                    Directory.CreateDirectory(dirinvite);
                if (!Directory.Exists(dirconfig))
                    Directory.CreateDirectory(dirconfig);
                if (!Directory.Exists(dirlists))
                    Directory.CreateDirectory(dirlists);
                if (!Directory.Exists(dirupd))
                    Directory.CreateDirectory(dirupd);
                if (!Directory.Exists(dirupdtmp))
                    Directory.CreateDirectory(dirupdtmp);
                if (!Directory.Exists(dirplugin))
                    Directory.CreateDirectory(dirplugin);
                if (!Directory.Exists(dirimgs))
                    Directory.CreateDirectory(dirimgs);
                
                }
            catch (Exception ex)
                {
                ;
                }
            }

        //private void WritePayPalFiles()
        //    {
        //    Write(Gator25.Properties.Resources.donor1, filedonator1);
        //    Write(Gator25.Properties.Resources.donor2, filedonator2);
        //    Write(Gator25.Properties.Resources.donor3, filedonator3);
        //    Write(Gator25.Properties.Resources.donor4, filedonator4);
        //    Write(Gator25.Properties.Resources.donor5, filedonator5);
        //    Write(Gator25.Properties.Resources.donor6, filedonator6);            
        //    }

        
        public void Write( string text, string filepath )
            {
            try
                {
                File.WriteAllText(filepath, text);
                }
            catch (Exception ex)
                {
                ;
                }
            }
        public void Append(string text, string filepath)
            {
            try
                {
                File.AppendAllText(filepath, text);
                }
            catch (Exception ex)
                {
                ;
                }
            }
        public string Read(string filepath)
            {
            string buff = string.Empty;
            try
                {
                if (File.Exists(filepath))
                    buff = File.ReadAllText(filepath);
                }
            catch (Exception ex)
                {
                ;
                }
            return buff;
            }

        //public int LoadGatherList(ref StringBuilder list)
        //    {
        //    int ngathered = 0;

        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.Filter = "*.txt|*.txt|*.*|*.*";
        //    ofd.InitialDirectory = dirlists;
        //    ofd.Title = "Open Gather List";
        //    DialogResult res =  ofd.ShowDialog();
        //    try
        //        {
        //        if (res != DialogResult.Cancel)
        //            {
        //            string filepath = Path.GetFullPath(ofd.FileName);
        //            if (File.Exists(filepath))
        //                {
        //                string text = Read(filepath);
        //                MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //                ArrayList groupurls = new ArrayList();
        //                ArrayList uidlists = new ArrayList();
        //                if ((text.Substring(0, 8)) == "<GGV2.5>")  //version 2.5 signature
        //                    {
        //                    groupurls = utils.GetTokensBetween(text, "<GURL>", "</GURL>");
        //                    uidlists = utils.GetTokensBetween(text, "</GID>", "</GROUP>");
        //                    for (int i = 0; i < groupurls.Count; i++)
        //                        {
        //                        Write(uidlists[i].ToString(), dirgather + "\\" + groupurls[i].ToString());
        //                        }
        //                    }
        //                ngathered = GetGatherTotal(list);
        //                }
        //            }
        //        else
        //            ngathered = -1;
        //        }
        //    catch (Exception ex)
        //        {
        //        ;
        //        }
        //    return ngathered;
        //    }
        
        //public void WhiteListIt(Group frmgrp, string uid)
        //    {
        //    Append("<UID>" + uid + "</UID>", dirgather + "\\" + frmgrp.url); 
        //    }
        //public void BlackListIt(Group togrp, string uid)
        //    {
        //    Append("<UID>" + uid + "</UID>", dirinvite + "\\" + togrp.url);
        //    }
        //public void AddListToBlackList(Group togrp, StringBuilder lst)
        //    {
        //    Append(lst.ToString(), dirinvite + "\\" + togrp.url);
        //    }
        //public bool IsWhiteListed(Group frmgrp, string uid)
        //    {
        //    string buff = Read(dirgather + "\\" + frmgrp.url);
        //    if (buff.Contains("<UID>" + uid + "</UID>"))
        //        return true;
        //    else
        //        return false;
        //    }
        //public bool IsBlackListed(Group togrp, string uid)
        //    {
        //    string buff = Read(dirinvite + "\\" + togrp.url);
        //    if (buff.Contains("<UID>"+uid + "</UID>"))
        //        return true;
        //    else
        //        return false;
        //    }
        //public void RemoveWhiteLists(StringBuilder gatherlist)
        //    {
        //    string list = gatherlist.ToString();            
        //    MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //    ArrayList groupurls = new ArrayList();
        //    if ((list.Substring(0, 8)) == "<GGV2.5>")  //version 2.5 signature
        //        {
        //        groupurls = utils.GetTokensBetween(list, "<GURL>", "</GURL>");
        //        for (int i = 0; i < groupurls.Count; i++)
        //            {
        //            try
        //                {
        //                File.Delete(dirgather + "\\" + groupurls[i].ToString());
        //                }
        //            catch (Exception ex)
        //                {
        //                ;
        //                }
        //            }
        //        }
        //    }
        //public void RemoveWhiteList(Group frmgrp)
        //    {
        //    try
        //        {
        //        File.Delete(dirgather + "\\" + frmgrp.url);
        //        }
        //    catch (Exception ex)
        //        {
        //        ;
        //        }
        //    }

        //public void SaveGatherList(StringBuilder list)
        //    {
            
        //    SaveFileDialog  sfd = new SaveFileDialog();
        //    sfd.InitialDirectory = dirlists;
        //    sfd.Title = "Select location to save gatherlist";
        //    sfd.Filter = "*.txt|*.txt|*.*|*.*";
        //    DialogResult res = sfd.ShowDialog();
        //    try
        //        {
        //        if (res != DialogResult.Cancel)
        //            {

        //            string writefile = Path.GetFullPath(sfd.FileName);
        //            if (writefile != string.Empty)
        //                {
        //                Write(list.ToString(), writefile);
        //                }
        //            }
        //        }
        //    catch (Exception ex)
        //        {
        //        ;
        //        }
        //    }


        //public void SetLastInvitee(ref StringBuilder list, string lastinvitee)
        //    {
        //    string tmp = "<LASTINV>" + lastinvitee + "</LASTINV>";
        //    try
        //        {
        //        list.Append(tmp);
        //        }
        //    catch (Exception ex)
        //        {
        //        ;
        //        }
        //    }
        //public string GetLastInvitee(StringBuilder list)
        //    {
        //    string ret = "-1";
        //    MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //    string tmp = string.Empty;
        //    int beg = -1;
        //    beg = list.ToString().LastIndexOf("<LASTINV>");
        //    if (beg != -1)
        //        {
        //        tmp = list.ToString().Substring(beg);
        //        ret = utils.GetStrBetween(tmp, "<LASTINV>", "</LASTINV>");
        //        }
        //    return ret;
        //    }
        //public void AddToGatherList(ref StringBuilder list, string idtoadd)
        //    {
        //    string tmp = "<UID>" + idtoadd + "</UID>";
        //    try
        //        {
        //        list.Append(tmp);
        //        }
        //    catch (Exception ex)
        //        {
        //        ;
        //        }
        //    }
        //public int GetGatherTotal(StringBuilder list)
        //    {
        //    int ret = 0;
        //    string tmp = string.Empty;
        //    string val = string.Empty;
        //    int beg = list.ToString().LastIndexOf("<NTOTAL>");
        //    if (beg != -1)
        //        {
        //        tmp  = list.ToString().Substring(beg);
        //        MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //        val = utils.GetStrBetween(tmp, "<NTOTAL>", "</NTOTAL>");
        //        try
        //            {
        //            ret = Convert.ToInt32(val);
        //            }
        //        catch (Exception ex)
        //            {
        //            ret = 0;
        //            }
        //        }
        //    return ret;
        //    }


        /////////////
        //public void RemoveBlackList(Group togrp)
        //    {
        //    try
        //        {
        //        File.Delete(dirinvite + "\\" + togrp.url);
        //        }
        //    catch (Exception ex)
        //        {
        //        ;
        //        }
        //    }


        //public bool IsSteamOnline()
        //    {
        //    MyWeb.MyWeb myweb = new MyWeb.MyWeb();
        //    string resp = string.Empty;
        //    resp = myweb.GetWebPage("http://steamcommunity.com/", 0, 0);
        //    if ((resp == string.Empty) || (resp.Contains("Error 503 Service Unavailable")) ||(resp.Contains("The Steam Community is currently unavailable")))
        //        return false;
        //    else
        //        return true; 
        //    }

        //public void AddFromGroup(Group grp)
        //    {
        //    string buff = string.Empty;
        //    buff = Read(filefromgrps);
        //    if (!(buff.Contains("<GURL>" + grp.url + "</GURL>")))
        //        {
        //        Append("<GURL>" + grp.url + "</GURL>", filefromgrps);
        //        }
        //    }
        //public void DeleteFromGroup(string grpurl)
        //    {
        //    string buff = string.Empty;
        //    buff = Read(filefromgrps);
        //    if (buff.Contains("<GURL>" + grpurl + "</GURL>"))
        //        {
        //        buff = buff.Replace("<GURL>" + grpurl + "</GURL>", string.Empty);
        //        Write(buff, filefromgrps);
        //        }
        //    }

        //public void ReadPlayerGroupStats(string gurl, ref string beg, ref string cur)
        //    {
        //    string buff = Read(filetogrps);

        //    beg = "0";
        //    cur = "0";

        //    if (buff.Contains("<GURL>" + gurl + "</GURL>"))
        //        {
        //        MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //        string block = string.Empty;
        //        block = utils.GetStrBetween(buff, "<GURL>" + gurl + "</GURL>", "</NCUR>");
        //        if (block != string.Empty)
        //            {
        //            block = block + "</NCUR>";
        //            beg = utils.GetStrBetween(block, "<NBEG>", "</NBEG>");
        //            cur = utils.GetStrBetween(block, "<NCUR>", "</NCUR>");
        //            }
        //        }           

        //    }

        //public void ChangeGroupBeginCount(string gurl, string newcount)
        //    {
        //    string chk = "<GURL>" + gurl + "</GURL>";
        //    string buff = Read ( filetogrps);
        //    if (buff.Contains(chk))
        //        {
        //        MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //        string replace = utils.GetStrBetween(buff, chk, "</NBEG>");
        //        if (replace != string.Empty)
        //            {                    
        //            replace = chk + replace + "</NBEG>";
        //            string toadd = chk + "<NBEG>" + newcount + "</NBEG>";
        //            buff = buff.Replace(replace, toadd);
        //            Write(buff, filetogrps);
        //            }                
        //        }
        //    }

        //public void WritePlayerGroupStats ( Profile profile )
        //    {
        //    string buff = string.Empty;            
        //    buff = Read(filetogrps);
        //    string toadd = string.Empty;
        //    string strbeg = string.Empty;
        //    string strcur = string.Empty;

        //    for (int i = 0; i < profile.player.groups.Count; i++)
        //        {
        //        //string chk = "<GID>" + profile.player.groups[i].ToString() + "</GID>";
        //        string chk = "<GURL>" + profile.player.groups[i].ToString() + "</GURL>";
        //        if (buff.Contains(chk))
        //            {
        //            MyUtils.MyUtils utils = new MyUtils.MyUtils();
        //            string replace = utils.GetStrBetween(buff, chk, "</NCUR>");
        //            if (replace != string.Empty)
        //                {
        //                replace = chk + replace + "</NCUR>";
        //                strbeg = utils.GetStrBetween(replace, "<NBEG>", "</NBEG>");
        //                //strcur = utils.GetStrBetween(replace, "<NCUR>", "</NCUR>");
        //                //Group grp = new Group(profile.player.groups[i].ToString(), Constants.GroupInitType.groupid);
        //                Group grp = new Group(profile.player.groups[i].ToString(), Constants.GroupInitType.groupurl);
        //                strcur = grp.total.ToString();
        //                if ((grp.url != string.Empty) && (grp.url != ""))
        //                    {
        //                    toadd = chk + "<GURL>" + grp.url + "</GURL>";
        //                    toadd = toadd + "<NBEG>" + strbeg + "</NBEG>";
        //                    toadd = toadd + "<NCUR>" + strcur + "</NCUR>";
        //                    buff = buff.Replace(replace, toadd);                            
        //                    }
        //                }
        //            }
        //        else
        //            {
        //            //Group grp = new Group(profile.player.groups[i].ToString(), Constants.GroupInitType.groupid);
        //            Group grp = new Group(profile.player.groups[i].ToString(), Constants.GroupInitType.groupurl);
        //            strcur = grp.total.ToString();
        //            strcur = strcur.Replace(",", string.Empty);
        //            strbeg = strcur;
        //            if ((grp.url != string.Empty) && (grp.url != ""))
        //                {
        //                toadd = chk + "<GURL>" + grp.url + "</GURL>";
        //                toadd = toadd + "<NBEG>" + strbeg + "</NBEG>";
        //                toadd = toadd + "<NCUR>" + strcur + "</NCUR>";
        //                buff = buff + toadd;
        //                }
        //            }
        //        System.Threading.Thread.Sleep(500);
        //        }
        //    Write(buff, filetogrps);
        //    }

        public void  GetPluginDlls( ref Queue files )
            {
            string [] dlls = Directory.GetFiles(dirplugin, "*.dll");
            files.Clear();
            foreach (string s in dlls)
                {
                files.Enqueue(s); 
                }
            //return files;
            }
        }
    }
