using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Resources;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Web;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;
using System.Security.Cryptography;
//using mshtml;
using System.Runtime.InteropServices;
using System.Management;
using Microsoft.Win32;
using Gator;
using System.Reflection;

namespace Gator2
    
    {
    public partial class Gator2Main : Form
        {
        
        private string cversion = "3.0.0041";
        private static Region regMain;
        private static Gator2Main MyForm;
        Gator25.ToGroupDlg togrpsel;
                        

        public Constants.SoftLicense lic = Constants.SoftLicense.Demo;
        public GatherList mylist;
        private Profile profile;
        public SteamAuth auth;
        private Updater upd;
        Gator25.Plugin plug;


        public Gator2Main()
            {

            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = this.BackgroundImage.Width;
            this.Height = this.BackgroundImage.Height;

            regMain = BitmapToRegion.getRegionFast((Bitmap)this.BackgroundImage, Color.FromArgb(255, 0, 255), 100);
            this.Region = regMain;
            
            }

        private void ObsoleteCheck ( )
            {
            WebService wserv = new WebService();          

            if ( wserv.IsObsolete(cversion) == true )
                {
                MessageBox.Show("This version of the GroupGATOR software is obsolete, please download a newer version free!", "Obsolete!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);    
                }

            
            }

        private void SideKick ( )
            {
            ObsoleteCheck( );        



            //Log( GetTime( ) + "Looking for plugins..", Constants.LogMsgType.Gator, false );
            plug = new Gator25.Plugin( this );
            if (plug.Count == 0)
                {
                Log( GetTime( ) + "No plugins were found!", Constants.LogMsgType.Gator, true );
                pluginsToolStripMenuItem1.Enabled = false;
                }
            else
                {
                Log( GetTime( ) + plug.Count.ToString( ) + " plugins loaded!", Constants.LogMsgType.Success, true );
                pluginsToolStripMenuItem1.Enabled = true;
                }
            BanCheck( );
            LoadRecaptcha( );
            }

        private void HideListUICues ( )
            {
            pbCGLblack.Visible = false;
            pbCGLdupes.Visible = false;
            pbCGLfirstpage.Visible = false;
            pbCGLingame.Visible = false;
            pbCGLoffline.Visible = false;
            pbCGLonline.Visible = false;
            pbCGLrandmize.Visible = false;
            pbarCreateGL.Visible = false;
            }
        Constants.StructInviteDelay sdelay;
        GGDisk ggfile;
        
        private void Gator2Main_Load(object sender, EventArgs e)
            {          
            Control.CheckForIllegalCrossThreadCalls = false;
            MyForm = this;
            this.Visible = false;
            tabControl1.Visible = false;

            ggfile = new GGDisk();
            upd = new Updater();
            profile = new Profile();
            auth = new SteamAuth();
            mylist = null;
            auth.UpdateSessionID( );
            
            togrpsel =  new Gator25.ToGroupDlg( );


            try
                {
                if (File.Exists( ggfile.filesteamcaptcha ))
                    File.Delete( ggfile.filesteamcaptcha );

                }
            catch (Exception ex)
                {
                ;
                }

           


            sdelay.curdelay = 5;
            sdelay.israndom = false;
            sdelay.maxiph = 100;
            sdelay.maxwait = 20;
            sdelay.faildelay = 5;

            
            profile.LoadConfig();

            txtDBName.Text = profile.gatoremail;
            txtDBPass.Text = profile.gatorpass;
            panMain.Visible = true;
            txtName.Text = profile.uname;
            txtPass.Text = profile.upass;
            txtCaptcha.Text = string.Empty;
            txtGuard.Text = string.Empty;

            ThreadStart tsSideKick = new ThreadStart( SideKick );
            Thread tSideKick = new Thread( tsSideKick );
            tSideKick.IsBackground = true;
            tSideKick.Start( );

            tmrUpdate.Enabled = true;     


            label5.Text = "GG v" + cversion;

            int off = profile.gpref & 1;
            int on = (profile.gpref & 2) >> 1;
            int ing = (profile.gpref & 4) >> 2;
            if (on == 1)
                {
                chkOnline.Checked = true;
                }
            else
                {
                chkOnline.Checked = false;
                }
            if (ing == 1)
                {
                chkIngame.Checked = true;
                }
            else
                {
                chkIngame.Checked = false;
                }
            if (off == 1)
                {
                chkOffLine.Checked = true;
                }
            else
                {
                chkOffLine.Checked = false;
                }
            //cboGatherSpeed.SelectedIndex = 0;
            //label68.Enabled = false;
            //txtMaxDaysOffline.Enabled = false;

            LoadScriptFile();
           

            DateTime dt = DateTime.Now;
            Log( GetTime() + " GroupGATOR V" + cversion + " Session Start");

            
            try
                {
                webBrowser2.Navigate("http://www.groupgatorcommunity.net/banner.php");
                }
            catch (Exception ex)
                {
                Log(ex.Message.ToString(), 0, true);
                }

            //auth.sessionid = profile.sessid;

            //DonorLogOffClick();
            if (profile.israndomdelayon == true)
                {
                txtMaxWaitTime.Enabled = true;
                chkRandomizeISpeed.Checked = true;
                }
            else
                {
                txtMaxWaitTime.Enabled = false;
                chkRandomizeISpeed.Checked = false;
                }
            txtMaxWaitTime.Text = profile.maxwait.ToString();
            txtMaxIPH.Text = profile.miph.ToString();
            trckInviteSpeed.Value = profile.idelay;
            lblCurISpeed.Text = profile.idelay.ToString();
            txtFailDelay.Text = profile.fdelay.ToString();
            
            this.Text = "GroupGATOR v" + cversion;
            this.Visible = true;

            
            GGDisk disk = new GGDisk();
            if ( File.Exists ( disk.filesteamcaptcha ) )
                pbSteamCaptcha.Image = Image.FromFile( disk.filesteamcaptcha );
            if (File.Exists(disk.fileggcaptcha))
                pbGGCaptcha.Image = Image.FromFile(disk.fileggcaptcha);

            //cboPaypalRates.SelectedIndex = 0;
            //cboGoogleRates.SelectedIndex = 0;

            if (File.Exists(disk.filedbdetails))
                File.Delete(disk.filedbdetails);
            //Log(GetTime() + "Looking for plugins..", Constants.LogMsgType.Gator, false);
            //plug = new Gator25.Plugin(this);
            //if (plug.Count == 0)
            //    Log(GetTime() + "No plugins were found!", Constants.LogMsgType.Gator, true);
            //else
            //    Log(GetTime() + plug.Count.ToString() + " plugins loaded!", Constants.LogMsgType.Success, true);
            
            //rtfGatherStatus.BringToFront();
            DisableGatherControls();

            tabControl1.TabPages.Remove(tabPage1);
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            //tabControl1.TabPages.Remove(tabPage4);
            //tabControl1.TabPages.Remove(tabPage5);
            tabControl1.TabPages.Remove(tabPage6);


           
           
            }
        private int nGroupPages = 10;
        private void AutoPopulateProc()
            {
            int i = 1;
            int j = 1;
            bool publicgrpsover = false;
            bool privategrpsover = false;

            while (chkAutoPopulate.Checked == true)
                {
                
                if (( i <= nGroupPages) && ( publicgrpsover == false )) 
                    {

                    //http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=all
                    //http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=public&p=3&filter=public&sortby=SortByMembers
                    string head = "http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=public&p=";
                    string tail = "&filter=public&sortby=SortByMembers";
                    string reqpage = head + i.ToString() + tail;

                    try
                        {
                        MyWeb.MyWeb web = new MyWeb.MyWeb();
                        string res = string.Empty;
                        while (res == string.Empty)
                            {
                            res = web.GetWebPage(reqpage, 0, 0);
                            }

                        int beg;
                        beg = res.IndexOf("<div class=\"groupBlockMedium\">");
                        if (beg != -1)
                            {
                            res = res.Substring(beg);
                            MyUtils.MyUtils utils = new MyUtils.MyUtils();
                            ArrayList tok = new ArrayList();
                            tok.Clear();
                            tok = utils.GetTokensBetween(res, "<a class=\"linkStandard\" href=\"http://steamcommunity.com/groups/", "/members");
                            GGDisk disk = new GGDisk();
                            for (int k = 0; k < tok.Count; k++)
                                disk.AddFromGroup(tok[k].ToString());
                            }


                        i++;


                        }
                    catch (Exception ex)
                        {
                        chkAutoPopulate.Checked = false;
                        }
                    }
                else
                    break;

                System.Threading.Thread.Sleep(1000);
                if ((j <= nGroupPages) &&(privategrpsover == false))
                    {
                    //http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=all
                    //http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=public&p=3&filter=public&sortby=SortByMembers
                    string head = "http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=private&p=";
                    string tail = "&filter=private&sortby=SortByMembers";
                    string reqpage = head + i.ToString( ) + tail;

                    try
                        {
                        MyWeb.MyWeb web = new MyWeb.MyWeb( );
                        string res = string.Empty;
                        while (res == string.Empty)
                            {
                            res = web.GetWebPage( reqpage, 0, 0 );
                            }

                        int beg;
                        beg = res.IndexOf( "<div class=\"groupBlockMedium\">" );
                        if (beg != -1)
                            {
                            res = res.Substring( beg );
                            MyUtils.MyUtils utils = new MyUtils.MyUtils( );
                            ArrayList tok = new ArrayList( );
                            tok.Clear( );
                            tok = utils.GetTokensBetween( res, "<a class=\"linkStandard\" href=\"http://steamcommunity.com/groups/", "/members" );
                            GGDisk disk = new GGDisk( );
                            for (int k = 0; k < tok.Count; k++)
                                disk.AddFromGroup( tok[k].ToString( ) );
                            }


                        j++;


                        }
                    catch (Exception ex)
                        {
                        chkAutoPopulate.Checked = false;
                        }
                    }
                else
                    break;
                }
            }

        private void Gator2Main_FormClosing(object sender, FormClosingEventArgs e)
            {
            //save settings
            profile.SaveConfig();
            Environment.Exit(0);
            }
        private void ExitClick()
            {
            if (mylist != null)
                {
                if (mylist.IsInviting == true)
                    mylist.InviteStop();
                if (mylist.IsGathering == true)
                    mylist.GatherStop();
                System.Threading.Thread.Sleep(2000);
                UnSavedListCheck();
                }
            profile.SaveConfig();
            WebService wserv = new WebService();
            if ( lic == Constants.SoftLicense.Demo )
                wserv.Exit ( Constants.SoftLicense.Demo );
            else
                wserv.Exit ( Constants.SoftLicense.Paid);
            Environment.Exit(0);
            }


        public string GetTime()
            {
            DateTime dt = DateTime.Now;
            string ret = "[" + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "] ";
            return ret;
            }
        private void SaveSettingsTick()
            {
            profile.SaveConfig();
            int count = cboToGroup.Items.Count;
            lblInvGroupCount.Text = count.ToString( );
            }

        private string urlhistorydefault = "http://steamcommunity.com/groups/groupgator";
        private void TabDecorate(Constants.TabSituation sit)
            {
            tabControl1.TabPages.Remove(tabPage1);
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            tabControl1.TabPages.Remove(tabPage4);
            //tabControl1.TabPages.Remove(tabPage5);
            tabControl1.TabPages.Remove(tabPage6);

            GGDisk disk = new GGDisk();
            string towrite = string.Empty;
            WebService ws = new WebService( );
            WebService.ServerData sd = new WebService.ServerData( );
            switch (sit)
                {
                case Constants.TabSituation.DonorLogIn:
                    
                    toolStripMenuItem54.Visible = true;
                    toolStripMenuItem61.Visible = true;
                    donateToolStripMenuItem.Visible = true;
                    pluginsToolStripMenuItem1.Visible = true;

                    tabControl1.TabPages.Insert(0, tabPage1);
                    tabControl1.TabPages.Insert(1, tabPage6);
                    //tabControl1.TabPages.Insert(2, tabPage4);
                    //tabControl1.TabPages.Insert(3, tabPage5);
                    
                    towrite = "<GGID>" + txtDBName.Text + "</GGID><GGPASS>" + txtDBPass.Text + "</GGPASS>";
                    towrite = towrite + "<LIC>PAID</LIC>";
                    disk.Write(towrite, disk.filedbdetails);

                    ws.Enter( Constants.SoftLicense.Paid );

                    sd = ws.CheckUsage( );
                    iremain = sd.iremain;
                    gremain = sd.gremain;
                    tremain = sd.tremain;
                    tickpast = Environment.TickCount;

                    break;
                case Constants.TabSituation.SteamLogIn:
                    gatherToolStripMenuItem.Enabled = true;
                    inviteToolStripMenuItem.Enabled = true;
                    statsToolStripMenuItem.Enabled = true;
                    blackListToolStripMenuItem.Enabled = true;
                    gatherListToolStripMenuItem.Enabled = true;

                    tabControl1.TabPages.Insert(0, tabPage1);
                    tabControl1.TabPages.Insert(1, tabPage2);
                    tabControl1.TabPages.Insert(2, tabPage3);
                    tabControl1.TabPages.Insert(3, tabPage6);
                    tabControl1.TabPages.Insert(4, tabPage4);
                    try
                        {
                        wbSteamHistory.Navigate( urlhistorydefault );
                        }
                    catch (Exception ex)
                        {
                        ;
                        }
                    //if (lic == Constants.SoftLicense.Paid)
                    //    tabControl1.TabPages.Insert(5, tabPage5);
                    lblCurISpeed.Text = profile.idelay.ToString();
                    trckInviteSpeed.Value = profile.idelay;
                    txtFailDelay.Text = profile.fdelay.ToString();
                    break;
                case Constants.TabSituation.FreeLogin:
                    toolStripMenuItem54.Visible = true;
                    toolStripMenuItem61.Visible = true;
                    donateToolStripMenuItem.Visible = true;
                    pluginsToolStripMenuItem1.Visible = true;

                    tabControl1.TabPages.Insert(0, tabPage1);
                    tabControl1.TabPages.Insert(1, tabPage6);
                    tabControl1.TabPages.Insert(2, tabPage4);
                    try
                        {
                        wbSteamHistory.Navigate( urlhistorydefault );
                        }
                    catch (Exception ex)
                        {
                        ;
                        }
                    towrite = "<GGID>" + txtDBName.Text + "</GGID><GGPASS>" + txtDBPass.Text + "</GGPASS>";
                    towrite = towrite + "<LIC>DEMO</LIC>";
                    disk.Write(towrite, disk.filedbdetails);
                    ws.Enter( Constants.SoftLicense.Demo );

                    
                    sd = ws.CheckUsage( );
                    iremain = sd.iremain;
                    gremain = sd.gremain;
                    tremain = sd.tremain;
                    tickpast = Environment.TickCount;
                    break;
                case Constants.TabSituation.SteamLogOff:
                    gatherToolStripMenuItem.Enabled = false;
                    inviteToolStripMenuItem.Enabled = false;
                    statsToolStripMenuItem.Enabled = false;
                    blackListToolStripMenuItem.Enabled = false;
                    gatherListToolStripMenuItem.Enabled = false;

                    tabControl1.TabPages.Insert(0, tabPage1);
                    tabControl1.TabPages.Insert(1, tabPage6);
                    //tabControl1.TabPages.Insert(2, tabPage4);
                    //if ( lic == Constants.SoftLicense.Paid )
                    //    tabControl1.TabPages.Insert(3, tabPage5);
                    
                    break;

                case Constants.TabSituation.DonorLogOff:
                    gatherToolStripMenuItem.Enabled = false;
                    inviteToolStripMenuItem.Enabled = false;
                    statsToolStripMenuItem.Enabled = false;
                    blackListToolStripMenuItem.Enabled = false;
                    gatherListToolStripMenuItem.Enabled = false;
                    toolStripMenuItem54.Visible = false;
                    toolStripMenuItem61.Visible = false;
                    donateToolStripMenuItem.Visible = false;
                    pluginsToolStripMenuItem1.Visible = false;
                    lic = Constants.SoftLicense.Demo;
                    profile.gatoremail = "FREEUSER@groupgatorcommunity.net";
                    break;


                }


            }
        
#region LOG
        static readonly object _lockerLog = new object();
        public void Log(string logmsg, Constants.LogMsgType type, bool newline)
            {
            if ( saveLogToTxtFileToolStripMenuItem.Checked == true )
                if ( newline == true )
                    ggfile.Append (logmsg + Environment.NewLine, ggfile.filelog );
                else
                    ggfile.Append(logmsg, ggfile.filelog);
            lock (_lockerLog)
                {
                rtfGatherStatus.SelectionStart = rtfGatherStatus.Text.ToCharArray().Length;
                rtfGatherStatus.SelectionLength = 0;
                if (type == Constants.LogMsgType.Error)
                    rtfGatherStatus.SelectionColor = Color.Red;
                else if (type == Constants.LogMsgType.Success)
                    rtfGatherStatus.SelectionColor = Color.FromArgb(50, 205, 50); //Color.Green;
                else if (type == Constants.LogMsgType.Gator)
                    rtfGatherStatus.SelectionColor = Color.Blue;

                if (newline == true)
                    rtfGatherStatus.SelectedText = logmsg + Environment.NewLine;
                else
                    rtfGatherStatus.SelectedText = logmsg;
                //rtfGatherStatus.ScrollToCaret();
                //ScrollToBottom(rtfGatherStatus);
                rtfGatherStatus.SelectionStart = rtfGatherStatus.Text.ToCharArray().Length;
                rtfGatherStatus.SelectionLength = 0;
                if (rtfGatherStatus.Lines.Length > 300)
                    rtfGatherStatus.Clear();
                }


            }
        public void Log(string logmsg)
            {
            if ( saveLogToTxtFileToolStripMenuItem.Checked == true )
                ggfile.Append(logmsg + Environment.NewLine, ggfile.filelog);
            lock (_lockerLog)
                {
                rtfGatherStatus.SelectionStart = rtfGatherStatus.Text.ToCharArray().Length;
                rtfGatherStatus.SelectionLength = 0;
                rtfGatherStatus.SelectionStart = rtfGatherStatus.Text.ToCharArray().Length;
                rtfGatherStatus.SelectionLength = 0;
                rtfGatherStatus.SelectionColor = Color.Blue;
                rtfGatherStatus.SelectedText = logmsg + Environment.NewLine;
                //rtb.ScrollToCaret();
                //ScrollToBottom(rtb);
                rtfGatherStatus.SelectionStart = rtfGatherStatus.Text.ToCharArray().Length;
                rtfGatherStatus.SelectionLength = 0;
                if (rtfGatherStatus.Lines.Length > 300)
                    rtfGatherStatus.Clear();
                }
            }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 277;
        private const int WM_LBUTTONDOWN = 513;
        private const int SB_PAGEBOTTOM = 7;

        public static void ScrollToBottom(RichTextBox MyRichTextBox)
            {
            //SendMessage(rtb.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
            //SendMessage(rtb.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
            }



#endregion


#region UI


        private static Point lastPoint;
        private void Gator2Main_MouseDown(object sender, MouseEventArgs e)
            {
            lastPoint = new Point(e.X, e.Y);
            }
        private void Gator2Main_MouseMove(object sender, MouseEventArgs e)
            {
            if (e.Button == MouseButtons.Left)
                {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
                }
            }
        private void pbExit_MouseEnter(object sender, EventArgs e)
            {
            pbExit.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbExit_MouseLeave(object sender, EventArgs e)
            {
            pbExit.BorderStyle = BorderStyle.None;
            }
        private void pbExit_MouseDown(object sender, MouseEventArgs e)
            {
            pbExit.BorderStyle = BorderStyle.Fixed3D;
            }
        private void pbExit_MouseUp(object sender, MouseEventArgs e)
            {
            pbExit.BorderStyle = BorderStyle.FixedSingle;
            }        
        private void pbExit_Click(object sender, EventArgs e)
            {
            ExitClick();
            }
        private void pbHome_MouseEnter(object sender, EventArgs e)
            {
            pbHome.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbHome_MouseLeave(object sender, EventArgs e)
            {
            pbHome.BorderStyle = BorderStyle.None;
            }
        private void pbHome_MouseDown(object sender, MouseEventArgs e)
            {
            pbHome.BorderStyle = BorderStyle.Fixed3D;
            }
        private void pbHome_MouseUp(object sender, MouseEventArgs e)
            {
            pbHome.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbHome_Click(object sender, EventArgs e)
            {
            System.Diagnostics.Process.Start("http://groupgator.enjin.com/");
            }
        private void pbTwitter_MouseDown(object sender, MouseEventArgs e)
            {
            pbTwitter.BorderStyle = BorderStyle.Fixed3D;
            }
        private void pbTwitter_MouseEnter(object sender, EventArgs e)
            {
            pbTwitter.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbTwitter_MouseLeave(object sender, EventArgs e)
            {
            pbTwitter.BorderStyle = BorderStyle.None;
            }
        private void pbTwitter_MouseUp(object sender, MouseEventArgs e)
            {
            pbTwitter.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbTwitter_Click(object sender, EventArgs e)
            {
            System.Diagnostics.Process.Start("https://twitter.com/GroupGATOR");
            }
        private void pbFaceBook_MouseDown(object sender, MouseEventArgs e)
            {
            pbFaceBook.BorderStyle = BorderStyle.Fixed3D;
            }
        private void pbFaceBook_MouseEnter(object sender, EventArgs e)
            {
            pbFaceBook.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbFaceBook_MouseLeave(object sender, EventArgs e)
            {
            pbFaceBook.BorderStyle = BorderStyle.None;
            }
        private void pbFaceBook_MouseUp(object sender, MouseEventArgs e)
            {
            pbFaceBook.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbFaceBook_Click(object sender, EventArgs e)
            {
            System.Diagnostics.Process.Start("http://www.facebook.com/pages/Group-Gator-Invite-Tool/299355590093965?ref=ts");
            }
        private void pbYouTube_MouseDown(object sender, MouseEventArgs e)
            {
            pbYouTube.BorderStyle = BorderStyle.Fixed3D;
            }
        private void pbYouTube_MouseEnter(object sender, EventArgs e)
            {
            pbYouTube.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbYouTube_MouseLeave(object sender, EventArgs e)
            {
            pbYouTube.BorderStyle = BorderStyle.None;
            }
        private void pbYouTube_MouseUp(object sender, MouseEventArgs e)
            {
            pbYouTube.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbYouTube_Click(object sender, EventArgs e)
            {
            System.Diagnostics.Process.Start("http://www.youtube.com/user/GroupGATOR");
            }
        private void pbYouTube_MouseHover(object sender, EventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            pbYouTube.BorderStyle = BorderStyle.None;
            }
        private void pbYouTube_MouseMove(object sender, MouseEventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbFaceBook_MouseHover(object sender, EventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbFaceBook_MouseMove(object sender, MouseEventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbTwitter_MouseHover(object sender, EventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbTwitter_MouseMove(object sender, MouseEventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbHome_MouseHover(object sender, EventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbHome_MouseMove(object sender, MouseEventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbExit_MouseHover(object sender, EventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbExit_MouseMove(object sender, MouseEventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void pbMinimize_Click(object sender, EventArgs e)
            {
            this.WindowState = FormWindowState.Minimized;
            }
        private void pbMinimize_MouseEnter(object sender, EventArgs e)
            {
            pbMinimize.BorderStyle = BorderStyle.FixedSingle;
            }
        private void pbMinimize_MouseLeave(object sender, EventArgs e)
            {
            pbMinimize.BorderStyle = BorderStyle.None;
            }
        private void pbMinimize_MouseDown(object sender, MouseEventArgs e)
            {
            pbMinimize.BorderStyle = BorderStyle.Fixed3D;
            }
        private void pbMinimize_MouseUp(object sender, MouseEventArgs e)
            {
            pbMinimize.BorderStyle = BorderStyle.FixedSingle;
            }
        private void cboFromGroup_MouseMove(object sender, MouseEventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void cboFromGroup_MouseHover(object sender, EventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void cboToGroup_MouseHover(object sender, EventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        private void cboToGroup_MouseMove(object sender, MouseEventArgs e)
            {
            Cursor.Current = Cursors.Hand;
            }
        
        ///////////////////////////////////////////////// menu remnant help
        //int filestatus = 0;
        //private void fileToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        //    {
        //    filestatus = 1;
        //    fileToolStripMenuItem.ForeColor = Color.Black;
        //    }
        //private void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        //    {
        //    filestatus = 0;
        //    fileToolStripMenuItem.ForeColor = Color.White;
        //    }
        //private void fileToolStripMenuItem_MouseHover(object sender, EventArgs e)
        //    {
        //    fileToolStripMenuItem.ForeColor = Color.Black;
        //    }
        //private void fileToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        //    {
        //    fileToolStripMenuItem.ForeColor = Color.Black;
        //    }
        //private void fileToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        //    {
        //    if (filestatus == 0)
        //        fileToolStripMenuItem.ForeColor = Color.White;
        //    }
        //private void wait20SecondToolStripMenuItem_Click(object sender, EventArgs e)
        //    {
        //    //profile.fdelay = Constants.FailedDelay.Wait20S;

        //    //wait20SecondToolStripMenuItem.Checked = true;
        //    //Log(GetTime() + "Failed invite delay set to : 20 Seconds");
        //    }
        //private void randomFastToolStripMenuItem_Click_1(object sender, EventArgs e)
        //    {
        //    profile.idelay = Constants.InviteDelay.RandomFast;
        //    Log(GetTime() + "Invite delay set to : Random Fast");
        //    }

        //////////////////////////////////////////////

        private void cboFromGroup_DropDown(object sender, EventArgs e)
            {
            FillFromGroupCombo();
            }
        private void cboToGroup_DropDown(object sender, EventArgs e)
            {
            FillToGroupCombo();
            }
        private void cboToGroup_SelectedIndexChanged(object sender, EventArgs e)
            {
            
            LoadBeginAndCurrentMemberCount();
            }
        private void txtInviteBeginCount_TextChanged(object sender, EventArgs e)
            {
            lblStartCount.Text = txtInviteBeginCount.Text;
            }
        private void cboFromGroup_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Delete)
                {
                DeleteFromGroupClick();
                }
            }
        private void txtInviteBeginCount_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                SaveInviteBeginClick();
                }
            }
        private bool rtfcopyon = false;
        private void rtfGatherStatus_MouseDown(object sender, MouseEventArgs e)
            {
            rtfcopyon = true;
            }
        private void rtfGatherStatus_MouseUp(object sender, MouseEventArgs e)
            {
            if (rtfcopyon == true)
                {
                if (rtfGatherStatus.SelectedText.Length > 0)
                    Clipboard.SetText(rtfGatherStatus.SelectedText);
                }
            rtfcopyon = false;
            }


        public static DialogResult InputBox(string title, string promptText, ref string value)
            {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
            }

        private void txtDBPass_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                DBLoginClick();
                btnGGLogin.Enabled = true;
                }
            }
        private void txtDBName_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                DBLoginClick();
                btnGGLogin.Enabled = true;
                }
            }
        private void txtEmailNewAccount_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                RegNewAccount();
                }
            }
        private void txtPassNewAccount_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                RegNewAccount();
                }

            }
        private void txtRePassNewAccount_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                RegNewAccount();
                }

            }
        private void txtCaptchaNewAccount_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                RegNewAccount();
                }

            }
        private void txtName_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                SteamLoginClick();
                }
            }
        private void txtPass_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                SteamLoginClick();
                }

            }
        private void txtCaptcha_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                SteamLoginClick();
                }

            }
        private void txtGuard_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter)
                {
                SteamLoginClick();
                }

            }
        private void btnSteamLogin_Click(object sender, EventArgs e)
            {
            SteamLoginClick();
            }
        private void btnInviteStart_Click(object sender, EventArgs e)
            {
            
            StartInviteClick();
            }
        private void btnInviteStop_Click(object sender, EventArgs e)
            {
            StopInviteClick();
            }
        private void btnGatherStart_Click(object sender, EventArgs e)
            {
            HideListUICues( );
            StartGatherClick();
            }
        private void btnGatherStop_Click(object sender, EventArgs e)
            {
            StopGatherClick();
            }
        private void btnLoadList_Click(object sender, EventArgs e)
            {
            LoadListClick();
            }
        private void btnSaveList_Click(object sender, EventArgs e)
            {
            SaveListClick();
            }
        private void btnClearGStat_Click(object sender, EventArgs e)
            {
            lblUsable.Text = "0";
            if ( bgwCreateList.IsBusy )
                bgwCreateList.CancelAsync( );
            while (bgwCreateList.IsBusy == true)
                {
                System.Threading.Thread.Sleep( 1000 );
                }
            
            DisableGatherControls( );
            }
        private void btnSaveInviteBegin_Click(object sender, EventArgs e)
            {
            SaveInviteBeginClick();
            }
        private void btnStartFreeVersion_Click(object sender, EventArgs e)
            {

            StartFreeVersionClick();
            btnStartFreeVersion.Enabled = true;
            //rtfGatherStatus.Parent = tabPage1;
            //rtfGatherStatus.Top = pbSteamCaptcha.Top + pbSteamCaptcha.Height + 10;
            //rtfGatherStatus.Left = tabPage1.Left + 10;
            //rtfGatherStatus.Width = tabPage1.Width - 20;
            //rtfGatherStatus.Height = tabPage1.Height - 10 - rtfGatherStatus.Top;
            }
        private void btnGGLogin_Click(object sender, EventArgs e)
            {
            DBLoginClick();
            btnGGLogin.Enabled = true;
            //rtfGatherStatus.Parent = tabPage1;
            //rtfGatherStatus.Top = pbSteamCaptcha.Top + pbSteamCaptcha.Height + 10;
            //rtfGatherStatus.Left = tabPage1.Left + 10;
            //rtfGatherStatus.Width = tabPage1.Width - 20;
            //rtfGatherStatus.Height = tabPage1.Height - 10 - rtfGatherStatus.Top;
            }
        private void chkIngame_CheckedChanged(object sender, EventArgs e)
            {
            SetGatherPreference(Constants.GatherType.InGame);
            }
        private void chkOnline_CheckedChanged(object sender, EventArgs e)
            {
            SetGatherPreference(Constants.GatherType.Online);
            }
        private void chkOffLine_CheckedChanged(object sender, EventArgs e)
            {
            SetGatherPreference(Constants.GatherType.Offline);
            }
        private void btnRegNewAccount_Click(object sender, EventArgs e)
            {
            RegNewAccount();
            }
        //private void btnAdminLogin_Click(object sender, EventArgs e)
        //    {
        //    AdminLoginClick();
        //    }
        //private void btnDonorLogOff_Click(object sender, EventArgs e)
        //    {
        //    if ( lic == Constants.SoftLicense.Demo )
        //        Log(GetTime() + "FREEUSER Logged out of GroupGATOR", Constants.LogMsgType.Success, true);
        //    else
        //        Log(GetTime() + profile.gatoremail + " Logged out of GroupGATOR", Constants.LogMsgType.Success, true);           
        //    DonorLogOffClick();
        //    }
        //private void txtAdminName_KeyDown(object sender, KeyEventArgs e)
        //    {
        //    if (e.KeyCode == Keys.Enter)
        //        {
        //        AdminLoginClick();
        //        }
        //    }
        //private void txtAdminPass_KeyDown(object sender, KeyEventArgs e)
        //    {
        //    if (e.KeyCode == Keys.Enter)
        //        {
        //        AdminLoginClick();
        //        }

        //    }
        private void txtName_TextChanged(object sender, EventArgs e)
            {
            profile.uname = txtName.Text;
            }
        private void txtPass_TextChanged(object sender, EventArgs e)
            {
            profile.upass = txtPass.Text;
            }



        private void tmrGGStats_Tick(object sender, EventArgs e)
            {
            UpdateGGStats();
            }
        
        private void timerSaveSettings_Tick(object sender, EventArgs e)
            {
            SaveSettingsTick();
            }
        private void tmrUpdate_Tick(object sender, EventArgs e)
            {
            UpdateTick();
            }
        private void tmrSettingsUpdate_Tick(object sender, EventArgs e)
            {
            SettingsUpdateTick();
            }
        //private void txtAcUnlock_KeyDown(object sender, KeyEventArgs e)
        //    {
        //    if (e.KeyCode == Keys.Enter)
        //        UnlockAccountClick();
        //    }
        //private void btnAdminUnlock_Click(object sender, EventArgs e)
        //    {
        //    UnlockAccountClick();
        //    }

        //private void btnAdminGetUserTime_Click(object sender, EventArgs e)
        //    {
        //    GetTimeRemainingClick(txtAcGetTime.Text);
        //    }
        //private void btnUnlock_Click(object sender, EventArgs e)
        //    {
        //    UnlockNormalClick();
        //    }
        //private void btnAdminAddDays_Click(object sender, EventArgs e)
        //    {
        //    AddTimeClick();
        //    }
        //private void btnAdminSetTime_Click(object sender, EventArgs e)
            //{
            //SetTimeClick();
            //}
        //private void btnDonate1_Click(object sender, EventArgs e)
        //    {
        //    switch (cboPaypalRates.SelectedIndex)
        //        {
        //        case 0:
        //            DonateClicked(Constants.DonateType.Donate1, true);
        //            break;
        //        case 1:
        //            DonateClicked(Constants.DonateType.Donate2, true);
        //            break;
        //        case 2:
        //            DonateClicked(Constants.DonateType.Donate3, true);
        //            break;
        //        case 3:
        //            DonateClicked(Constants.DonateType.Donate4, true);
        //            break;
        //        case 4:
        //            DonateClicked(Constants.DonateType.Donate5, true);
        //            break;
        //        case 5:
        //            DonateClicked(Constants.DonateType.Donate6, true);
        //            break;
        //        }
            
        //    }

        //private void btnGift_Click(object sender, EventArgs e)
        //{
        //    DonorGiftClick();

        //}
        private void btnRefreshGGCaptcha_Click(object sender, EventArgs e)
            {
            LoadRecaptcha();
            }

        private void toolStripMenuItem55_Click(object sender, EventArgs e)
            {
            StartGatherClick();
            }
        private void toolStripMenuItem56_Click(object sender, EventArgs e)
            {
            StopGatherClick();
            }
        private void toolStripMenuItem57_Click(object sender, EventArgs e)
            {
            StartInviteClick();
            }
        private void toolStripMenuItem58_Click(object sender, EventArgs e)
            {
            StopInviteClick();
            }
        private void toolStripMenuItem59_Click(object sender, EventArgs e)
            {
            SaveListClick();
            }
        private void toolStripMenuItem60_Click(object sender, EventArgs e)
            {
            LoadListClick();
            }
        private void toolStripMenuItem62_Click(object sender, EventArgs e)
            {
            ClearGatherClick();
            }
        private void toolStripMenuItem64_Click(object sender, EventArgs e)
            {
            ClearInviteClick();
            }
        private void toolStripMenuItem65_Click(object sender, EventArgs e)
            {
            EditStartCountClick();
            }
        private void toolStripMenuItem66_Click(object sender, EventArgs e)
            {
            rtfGatherStatus.Clear();
            }
        private void clearBlackListToolStripMenuItem_Click(object sender, EventArgs e)
            {
            ClearBlackListClick();
            }
        private void addIDToBlackListToolStripMenuItem1_Click(object sender, EventArgs e)
            {
            AddIDToBlackListClick();
            }
        private void addEntireGatherListToolStripMenuItem_Click(object sender, EventArgs e)
            {
            AddEntireListToBlackListClick();
            }
        private void checkForUpdatesToolStripMenuItem1_Click(object sender, EventArgs e)
            {
            upd.HandleUpdate(Constants.UpdateType.Manual, cversion);
            }
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
            {
            //About about = new About(cversion);
            //about.ShowDialog();
            }
        private void toolStripMenuItem98_Click(object sender, EventArgs e)
            {
            try
                {
                System.Diagnostics.Process.Start( "http://www.groupgatorcommunity.net" );
                }
            catch (Exception ex)
                {
                Log(GetTime() + " exception in forum start: " + ex.Message.ToString(), Constants.LogMsgType.Error, true);
                }
            }
        private void toolStripMenuItem100_Click(object sender, EventArgs e)
            {
            MessageBox.Show("Current GroupGATOR is v" + cversion, "Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        private void toolStripMenuItem101_Click(object sender, EventArgs e)
            {
            

            }
        private void cboFromGroup_DropDown_1(object sender, EventArgs e)
            {
            FillFromGroupCombo();
            lblFromGrpTotal.Text = "(total:" + cboFromGroup.Items.Count.ToString( ) + ")";
            }

        private void chkSkipFirstPage_CheckedChanged(object sender, EventArgs e)
            {
            if (chkSkipFirstPage.Checked == true)
                {
                DialogResult res = MessageBox.Show("By skipping first page of memberlist, you can miss atleast 1000 members. Are you sure you want to skip?", "Go Ahead?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No)
                    chkSkipFirstPage.Checked = false;
                }
            }     

       
#endregion

#region invite
        private void SaveInviteBeginClick()
            {
            btnSaveInviteBegin.Visible = false;
            lblStartCount.Text = txtInviteBeginCount.Text;
            txtInviteBeginCount.Visible = false;
            lblStartCount.Visible = true;
            GGDisk disk = new GGDisk();
            if ((cboToGroup.Text == string.Empty) || (cboToGroup.SelectedIndex == -1))
                {
                Log("please choose an invite-to group first", Constants.LogMsgType.Error, true);
                return;
                }
            else
                {
                if (auth.status == Constants.SignInStatus.LoggedIn)
                    {
                    disk.ChangeGroupBeginCount(cboToGroup.Text, txtInviteBeginCount.Text);
                    }
                else
                    {
                    Log("please login first before editing group details..", Constants.LogMsgType.Error, true);
                    return;
                    }
                }
            LoadBeginAndCurrentMemberCount();
            //btnSaveInviteBegin.Visible = false;
            ////btnSaveInviteBegin.SendToBack();
            //lblStartCount.Visible = true;
            ////lblStartCount.BringToFront();

            //string beg = "0";
            //string cur = "0";
            //disk.ReadPlayerGroupStats(cboToGroup.Text, ref beg, ref cur);
            //lblStartCount.Text = beg;
            //lblNewCount.Text = cur;

            //int intbeg = 0;
            //int intcur = 0;
            //try
            //    {
            //    intbeg = Convert.ToInt32(beg);
            //    intcur = Convert.ToInt32(cur);
            //    }
            //catch (Exception ex)
            //    {
            //    intbeg = 0;
            //    intcur = 0;
            //    }
            //if (intbeg >= intcur)
            //    lblNewCount.ForeColor = Color.Red;
            //else
            //    lblNewCount.ForeColor = Color.Lime;
            }
        private void LoadBeginAndCurrentMemberCount()
            {
            string beg = string.Empty;
            string cur = string.Empty;

            
            if (cboToGroup.SelectedIndex != -1)
                {
                if (cboToGroup.Text != string.Empty)
                    {
                    GGDisk disk = new GGDisk();
                    disk.ReadPlayerGroupStats(cboToGroup.Text, ref beg, ref cur);
                    lblStartCount.Text = beg;
                    lblNewCount.Text = cur;

                    double dbeg = 0;
                    double dcur = 0;
                    try
                        {
                        dbeg = Convert.ToInt32(beg);
                        dcur = Convert.ToInt32(cur);
                        }
                    catch (Exception ex)
                        {
                        dbeg = 0;
                        dcur = 0;
                        }
                    double perc = ( dcur - dbeg ) / dbeg * 100;
                    lblIncrease.Text = perc.ToString("0.00");

                    if (dbeg >= dcur)
                        {
                        lblNewCount.ForeColor = Color.Red;
                        lblIncrease.ForeColor = Color.Red;
                        }
                    else
                        {
                        lblNewCount.ForeColor = Color.Lime;
                        lblIncrease.ForeColor = Color.Red;
                        }

                    }
                }

            }
        public void FillToGroupCombo()
            {
            System.Collections.ArrayList alist;// = new System.Collections.ArrayList();
            MyUtils.MyUtils utils = new MyUtils.MyUtils( );
            GGDisk disk = new GGDisk( );
            if (File.Exists( disk.filetogrps ))
                {
                string buff = disk.Read( disk.filetogrps );
                alist = utils.GetTokensBetween( buff, "<GURL>", "</GURL>" );
                for (int i = 0; i < alist.Count; i++) 
                    {
                    if (!cboToGroup.Items.Contains( alist[i] ))
                        cboToGroup.Items.Add( alist[i].ToString( ) );
                    }
                    
                }
            if (auth.status == Constants.SignInStatus.LoggedIn)
                {
                if (profile != null)
                    {
                    if (profileinitcomplete == true)
                        {
                        if (profile.player.groups.Count > 0)
                            {
                            if (cboToGroup.Items.Count > 0)
                                cboToGroup.Items.Clear( );
                            for (int i = 0; i < profile.player.groups.Count; i++)
                                {
                                //Group gptmp = new Group( profile.player.groups[i].ToString(), Constants.GroupInitType.groupid );
                                cboToGroup.Items.Add( profile.player.groups[i].ToString( ) );

                                }
                            cboToGroup.SelectedIndex = 0;
                            }
                        }
                    }
                }
              
            int ic = cboToGroup.Items.Count;
            lblInvGroupCount.Text = ic.ToString();
            }
        private void ClearBlackListClick()
            {
            if (mylist != null)
                {
                mylist.InviteClear();
                Log("Current blacklist cleared!", Constants.LogMsgType.Success, true);
                }
            }
        public void StartInviteClick()
            {
            //if (profileinitcomplete == false)
            //    {
            //    MessageBox.Show("Downloading your settings.. Please wait for it to complete", "Download in progress", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    tabControl1.SelectedTab = tabPage6;
            //    return;
            //    }
            if (lic == Constants.SoftLicense.Demo)
                {
                if (iremain <= 0)
                    {
                    if (ierrshown == false)
                        {
                        ierrshown = true;
                        MessageBox.Show( "Your free invite limit (1000) has been exceeded, please renew for continued usage! Thank you", "Free gather limit reached!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                        }
                    }
                }
            else
                {
                int ticknow = Environment.TickCount;
                int diff = ticknow - tickpast;


                if (tremain - diff <= 0)
                    {
                    terrshown = true;
                    MessageBox.Show( "Your subscription has expired, please renew for continued usage! Thank you", "Subscription expired", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                    }
                }
            if (chkInviteToAll.Checked == false)
                {
                if (mylist != null)
                    {
                    if (mylist.IsInviting == true)
                        {
                        Log("Invite error: another inviting is in progress, please wait for it to finish..", Constants.LogMsgType.Error, true);
                        MessageBox.Show("Another inviting is in progress, please wait for it to finish..", "Invite Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        }
                    else
                        {
                        if (cboToGroup.Text == string.Empty)
                            {
                            Log("Invite error: to group is empty. Please choose a group to invite to..", Constants.LogMsgType.Error, true);
                            MessageBox.Show("To group is empty. Please choose a group to invite to..", "Invite Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lblStatusInvite.Text = "IDLE";
                            return;
                            }
                        else
                            {
                            //ObsoleteCheck();       

                            lblStatusInvite.Text = "INVITING";
                            auth.UpdateSessionID();
                            int tmpidelay = trckInviteSpeed.Value;
                            int tmpmaxw = 0;
                            try
                                {
                                tmpmaxw = Convert.ToInt32(txtMaxWaitTime.Text);
                                }
                            catch (Exception ex)
                                {
                                tmpmaxw = 16;
                                }
                            int tmpmaxiph = 0;
                            try
                                {
                                tmpmaxiph = Convert.ToInt32(txtMaxIPH.Text);
                                }
                            catch (Exception ex)
                                {
                                tmpmaxiph = 100;
                                }
                            int tmpfdelay = 0;
                            try
                                {
                                tmpfdelay = Convert.ToInt32(txtFailDelay.Text);
                                }
                            catch (Exception ex)
                                {
                                tmpfdelay = 15;
                                }
                            profile.idelay = tmpidelay;
                            profile.fdelay = tmpfdelay;
                            profile.miph = tmpmaxiph;
                            profile.maxwait = tmpmaxw;
                            mylist.Invite(cboToGroup.Text, auth.cookie, auth.sessionid, profile);
                            //lblInviteBLTotal.Text = profile.ntotalblacklist.ToString();
                            }
                        }
                    }
                else
                    {
                    Log("Invite error: gatherlist is empty. Please load or create a gatherlist first..", Constants.LogMsgType.Error, true);
                    lblStatusInvite.Text = "IDLE";
                    MessageBox.Show("Gatherlist is emply, please load or create a gatherlist before you can invite", "Invite Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }
                }
            else
                {
                ObsoleteCheck();       
                icurtogrp = 0;
                chkInviteToAll.Enabled = false;
                tmrInviteAll.Enabled = true;
                Log("GroupGATOR will now invite to all groups", Constants.LogMsgType.Gator, true);
                }


            }
        public void IncrementInviteTotal()
            {
            profile.ntotalinvite++;
            lblTotalISent.Text = profile.ntotalinvite.ToString();
            }
        public void IncrementBlackListTotal()
            {
            profile.ntotalblacklist++;
            //lblGatherBLTotal.Text = profile.ntotalblacklist.ToString();
            //lblInviteBLTotal.Text = profile.ntotalblacklist.ToString();
            }
        private void ClearInviteClick()
            {
            lblFailed.Text = "0";
            lblSuccessful.Text = "0";
            lblTotalISent.Text = "0";
            if (mylist != null)
                {
                mylist.nInvited = 0;
                mylist.nSkipInvite = 0;
                }
            }
        private void StopInviteClick()
            {
            if (mylist != null)
                {
                mylist.InviteStop();
                lblStatusInvite.Text = "IDLE";
                }
            }
        private void AddIDToBlackListClick()
            {
            string uidmy = string.Empty;
            if (cboToGroup.Text == string.Empty)
                {
                Log("Invite-to group empty. Please choose an invite group first..", Constants.LogMsgType.Error, true);
                return;
                }
            else
                {
                if (auth.status == Constants.SignInStatus.LoggedIn)
                    {
                    if (InputBox("Add ID To Blacklist", "Enter the id to be blacklisted", ref uidmy) == DialogResult.OK)
                        {
                        GGDisk disk = new GGDisk();
                        Group grp = new Group(cboToGroup.Text, Constants.GroupInitType.groupurl);
                        disk.BlackListIt(grp, uidmy);
                        Log(GetTime() + "ID : " + uidmy + " added to blacklist : " + cboToGroup.Text, Constants.LogMsgType.Success, true);
                        IncrementBlackListTotal();
                        }
                    }
                else
                    {
                    Log("Not logged into Steam! Please login first", Constants.LogMsgType.Error, true);
                    return;
                    }
                }
            }
        private void AddEntireListToBlackListClick()
            {
            if (mylist == null)
                {
                MessageBox.Show("You don't have a valid gatherlist yet. Please create or load a gatherlist first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            if (auth.status != Constants.SignInStatus.LoggedIn)
                {
                MessageBox.Show("You must login to steam first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            if (cboToGroup.Text == string.Empty)
                {
                MessageBox.Show("Invite-to group empty. Please choose an invite group first..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log("Invite-to group empty. Please choose an invite group first..", Constants.LogMsgType.Error, true);
                return;
                }
            DialogResult res = MessageBox.Show("This will blacklist your current gatherlist completely. Are you sure?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                {
                GGDisk disk = new GGDisk();
                Group grp = new Group(cboToGroup.Text, Constants.GroupInitType.groupurl);
                disk.AddListToBlackList(grp, mylist.glist);
                Log(GetTime() + "Entire list added to blacklist : " + cboToGroup.Text, Constants.LogMsgType.Success, true);
                int listcnt = disk.GetGatherTotal(mylist.glist);
                mylist.nSkipGather = mylist.nSkipGather + listcnt;
                //lblSkipped.Text = mylist.nSkipGather.ToString();
                profile.ntotalblacklist += listcnt;
                //lblInviteBLTotal.Text = profile.ntotalblacklist.ToString();
                //lblGatherBLTotal.Text = profile.ntotalblacklist.ToString();
                }
            }

#endregion

#region gather
        private void FillFromGroupCombo()
            {
            if (auth.status == Constants.SignInStatus.LoggedIn)
                {
                System.Collections.ArrayList alist;// = new System.Collections.ArrayList();
                MyUtils.MyUtils utils = new MyUtils.MyUtils();
                GGDisk disk = new GGDisk();
                string buff = disk.Read(disk.filefromgrps);
                alist = utils.GetTokensBetween(buff, "<GURL>", "</GURL>");
                if (auth.status == Constants.SignInStatus.LoggedIn)
                    {
                    for (int i = 0; i < alist.Count; i++)
                        {
                        if (!cboFromGroup.Items.Contains(alist[i]))
                            cboFromGroup.Items.Add(alist[i].ToString());
                        }
                    }
                }
            }
        private void SaveListClick()
            {
            if (lic == Constants.SoftLicense.Demo)
                MessageBox.Show("Sorry, this feature is not available in freeversion", "GroupGATOR2");
            else
                {
                if (mylist != null)
                    {
                    mylist.SaveGatherList();
                    Log("Gather List Saved!", Constants.LogMsgType.Success, true);
                    }
                else
                    Log("GatherList is empty, can't save!", Constants.LogMsgType.Error, true);
                }
            }
        private void LoadListClick()
            {
            if (lic == Constants.SoftLicense.Demo)
                MessageBox.Show("Sorry, this feature is not available in freeversion", "GroupGATOR2");
            else
                {
                UnSavedListCheck();
                lblGatherTotal.Text = "0";
                if (mylist == null)
                    {
                    mylist = new GatherList(auth.steamid, lic, this);
                    }
                mylist.LoadGatherList();
                cboFromGroup.SelectedIndex = -1;
                EnableGatherControls();
                lblUsable.Text = lblIRemain.Text = mylist.nGathered.ToString();
                }
            }
        private void ClearGatherClick()
            {
            if (mylist != null)
                {
                DialogResult result;
                result = MessageBox.Show("Are you sure you want to clear the list", "Group Gator v25", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    {
                    mylist.ClearGather();
                    lblGatherTotal.Text = "0";
                    //lblSkipped.Text = "0";
                    //lblInvitable.Text = "0";
                    System.Threading.Thread.Sleep(1000);
                    mylist = null;
                    Log("Gather list cleared!", Constants.LogMsgType.Success, true);
                    DisableGatherControls();
                    lblUsable.Text = lblIRemain.Text = "0";
                    }
                }
            else
                Log("Gather list is empty! ", Constants.LogMsgType.Error, true);
            }
        public void SetGatherPreference(Constants.GatherType type)
            {
            int off = profile.gpref & 1;
            int on = (profile.gpref & 2) >> 1;
            int ing = (profile.gpref & 4) >> 2;

            if (type == Constants.GatherType.Offline)
                off = off ^ 1;
            else if (type == Constants.GatherType.Online)
                on = on ^ 1;
            else if (type == Constants.GatherType.InGame)
                ing = ing ^ 1;

            int pref = off | (on << 1) | (ing << 2);
            profile.gpref = pref;
            if (auth.status == Constants.SignInStatus.LoggedIn)
                if (mylist != null)
                    mylist.SetGatherType(pref);
            }
        private void StopGatherClick()
            {
            if (mylist != null)
                {
                if (allgroupsgatheringon == true)
                    {
                    tmrGatherAllGroups.Enabled = false;
                    allgroupsgatheringon = false;
                    }
                if ( mylist.IsGathering == true )
                    pbAjax.Visible = true;
                mylist.GatherStop();
                lblStatusGather.Text = "IDLE";
                lblStatusGather.ForeColor = Color.Red;
                
                
                }
            }
        private void DeleteFromGroupClick()
            {
            if ((cboFromGroup.Items.Count > 0) && (cboFromGroup.SelectedIndex != -1))
                {
                int index = cboFromGroup.SelectedIndex;
                string gurl = cboFromGroup.Items[index].ToString();
                GGDisk disk = new GGDisk();
                disk.DeleteFromGroup(gurl);
                }
            FillFromGroupCombo();
               
            }
        public void StartGatherClick()
            {
            //if (profileinitcomplete == false)
            //    {
            //    MessageBox.Show("Downloading your settings.. Please wait for it to complete", "Download in progress", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    tabControl1.SelectedTab = tabPage6;
            //    return;
            //    }
            if (lic == Constants.SoftLicense.Demo)
                {
                if (gremain <= 0)
                    {
                    if (gerrshown == false)
                        {
                        gerrshown = true;
                        MessageBox.Show( "Your free gather limit (3000) has been exceeded, please renew for continued usage! Thank you", "Free gather limit reached!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                        }
                    }
                }
            else
                {
                int ticknow = Environment.TickCount;
                int diff = ticknow - tickpast;


                if (tremain - diff <= 0)
                    {
                    terrshown = true;
                    MessageBox.Show( "Your subscription has expired, please renew for continued usage! Thank you", "Subscription expired", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                    }
                }
            if (chkGatherAllGroups.Checked == false)
                {
                if (mylist != null)
                    {
                    if (mylist.IsGathering == true)
                        {
                        Log("Gather error: another gathering is in progress, please wait for it to finish..", Constants.LogMsgType.Error, true);
                        return;
                        }
                    }
                if (cboFromGroup.Text == string.Empty)
                    {
                    Log("Gather error: from group is empty. Please choose a group to gather from..", Constants.LogMsgType.Error, true);
                    lblStatusGather.Text = "IDLE";
                    lblStatusGather.ForeColor = Color.Red;
                    return;
                    }

                //ObsoleteCheck();       
                
                int off, on, ing;
                off = 0;
                on = 0;
                ing = 0;
                if (chkIngame.Checked == true)
                    ing = 1;
                if (chkOffLine.Checked == true)
                    off = 1;
                if (chkOnline.Checked == true)
                    on = 1;                    
                int pref = off | (on << 1) | (ing << 2);
                profile.gpref = pref;

                if (mylist == null)
                    mylist = new GatherList(auth.steamid, lic, this);

                GGDisk disk = new GGDisk();
                lblStatusGather.Text = "GATHERING";
                lblStatusGather.ForeColor = Color.Lime;
                //if (cboGatherSpeed.SelectedIndex == 0)
                //    mylist.GatherNew(cboFromGroup.Text, togroupnew, profile.gpref, Constants.GatherSpeed.Quick);
                //else if (cboGatherSpeed.SelectedIndex == 1)
                    mylist.GatherNew(cboFromGroup.Text);
                //else if (cboGatherSpeed.SelectedIndex == 2)
                //    mylist.GatherNew(cboFromGroup.Text, togroupnew, profile.gpref, Constants.GatherSpeed.Medium);

                //lblGatherBLTotal.Text = profile.ntotalblacklist.ToString();

                }
            else
                {
                ObsoleteCheck();       
                icurfromgrop = 0;
                chkGatherAllGroups.Enabled = false;
                tmrGatherAllGroups.Enabled = true;
                Log("GroupGATOR will now gather from all groups", Constants.LogMsgType.Gator, true);
                }

            }
        private void UnSavedListCheck()
            {
            if (mylist != null)
                {
                if (mylist.saved == false)
                    {
                    DialogResult res = MessageBox.Show("You have an unsaved gatherlist. Would you like to save it first?", "GatherList not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                        {
                        mylist.SaveGatherList();
                        Log("Gather List Saved!", Constants.LogMsgType.Success, true);
                        }
                    mylist.ClearGather();
                    }
                }
            }

        private int icurfromgrop = 0;
        private bool allgroupsgatheringon = false;
        private void tmrGatherAllGroups_Tick(object sender, EventArgs e)
            {
            if (mylist == null)
                mylist = new GatherList(profile.player.id, lic, this);
            if (mylist.IsGathering == true)
                return;

            if (icurfromgrop > cboFromGroup.Items.Count - 1)
                {
                if (mylist.IsGathering == false )
                    {
                    tmrGatherAllGroups.Enabled = false;
                    Log("GroupGATOR has completed gathering from all previous groups!", Constants.LogMsgType.Success, true);
                    chkGatherAllGroups.Enabled = true;
                    if (chkSkipFirstPage.Checked == true)
                        {
                        Log("skipping first page...");
                        string tmpold = string.Empty;
                        string tmpnew = string.Empty;
                        string head = string.Empty;
                        string tail = string.Empty;
                        int beg, end;
                        beg = end = -1;
                        beg = mylist.glist.ToString().IndexOf("<UID>");
                        if (beg != -1)
                            {
                            head = mylist.glist.ToString().Substring(0, beg);
                            end = mylist.glist.ToString().LastIndexOf("</UID>");
                            if (end != -1)
                                {
                                end += "</UID>".Length;
                                tail = mylist.glist.ToString().Substring(end);
                                tmpold = mylist.glist.ToString().Substring(beg, end - beg);
                                tmpnew = mylist.TrimMemberList(tmpold, 1000, true);
                                mylist.glist.Length = 0;
                                mylist.glist.Append(head);
                                mylist.glist.Append(tmpnew);
                                mylist.glist.Append(tail);
                                Log("first 1000 ids skipped!");
                                }
                            }
                        }

                    if (chkRandomizeList.Checked == true)
                        {
                        Log("randomizing..");
                        mylist.RandomizeList();
                        }
                    }
                return;
                }



            if (icurfromgrop == 0)
                {
                allgroupsgatheringon = true;
                if (allgroupsgatheringon == false)
                    {
                    if (cboToGroup.Text == string.Empty)
                        {
                        Gator25.ToGroupDlg togrpsel = new Gator25.ToGroupDlg( );
                        cboToGroup.Text = togrpsel.Display( );
                        allgroupsgatheringon = false;
                        }
                    }
                }
            cboFromGroup.SelectedIndex = icurfromgrop;
            

            int off, on, ing;
            off = 0;
            on = 0;
            ing = 0;
            if (chkIngame.Checked == true)
                ing = 1;
            if (chkOffLine.Checked == true)
                off = 1;
            if (chkOnline.Checked == true)
                on = 1;
            int pref = off | (on << 1) | (ing << 2);
            profile.gpref = pref;

            lblStatusGather.Text = "GATHERING";
            lblStatusGather.ForeColor = Color.Lime;
            //if (cboGatherSpeed.SelectedIndex == 0)
            //    mylist.GatherNew(cboFromGroup.Text, cboToGroup.Text, profile.gpref, Constants.GatherSpeed.Quick);
            //else if (cboGatherSpeed.SelectedIndex == 1)
                mylist.GatherNew(cboFromGroup.Text);
            //else if (cboGatherSpeed.SelectedIndex == 2)
            //    mylist.GatherNew(cboFromGroup.Text, cboToGroup.Text, profile.gpref, Constants.GatherSpeed.Medium);

            //lblGatherBLTotal.Text = profile.ntotalblacklist.ToString();
            
            icurfromgrop++;
            }
#endregion
              


#region UPDATER
        private void UpdateTick()
            {
            upd.HandleUpdate(Constants.UpdateType.Auto, cversion);
            ObsoleteCheck( );
            }
        public bool updatingprofile = false;
        private void SettingsUpdateTick()
            {
            if (auth.status == Constants.SignInStatus.LoggedIn)
                {
                if (updatingprofile == false)
                    {
                    updatingprofile = true;
                    Thread thUpdProf;
                    ThreadStart thUpdProfStart;
                    thUpdProfStart = new ThreadStart(UpdateProfileSettingsProc);
                    thUpdProf = new Thread(thUpdProfStart);
                    thUpdProf.IsBackground = true;
                    thUpdProf.Start();
                    }
                }
            }
        private void UpdateProfileSettingsProc ()
            {
            profile.player.GetDetails();
            GGDisk disk = new GGDisk();
            disk.WritePlayerGroupStats(profile);
            updatingprofile = false;
            
            string buff = disk.Read(disk.filetogrps);
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            int ic = utils.CountString(buff, "<NBEG>");
            lblInvGroupCount.Text = ic.ToString();

            }
#endregion


#region SteamAUTH
        //private bool authprocworking = false;
        string encpass = string.Empty;
        private bool profileinitcomplete = false;
        private void wbLogin_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
            if (wbLogin.ReadyState != WebBrowserReadyState.Complete)
                return;
            object[] args = new object[3];
            args[0] = auth.kmod;
            args[1] = auth.kexp;
            args[2] = profile.upass;
            encpass = Convert.ToString(wbLogin.Document.InvokeScript("DoNewLogin", args));
            
            auth.captchatxt = txtCaptcha.Text;
            auth.emailauth = txtGuard.Text;                
            
            Log(GetTime() + "Sending login parameters..");
            
            Constants.SignInStatus stat = auth.Authenticate(encpass, auth.captchatxt, auth.emailauth, this);
            Log(GetTime() + "Complete!");

            GGDisk disk = new GGDisk( );
            MyWeb.MyWeb web = new MyWeb.MyWeb( );


            if (stat != Constants.SignInStatus.LoggedIn)
                {
                btnSteamLogin.Text = "LOGIN";
                Log("Login failed : ", Constants.LogMsgType.Error, false);
                if (stat == Constants.SignInStatus.CaptchaReqd)
                    {

                    //web.DownloadFile(auth.captchaurl, disk.filetmpcaptcha, 0, 0);
                    //webBrowser3.Navigate(auth.captchaurl);
                    //webBrowser3.Navigate(disk.filetmpcaptcha);
                    //webBrowser3.Navigate(disk.filecaptchahtml);
                    //webBrowser3.Refresh(WebBrowserRefreshOption.Completely);
                    //webBrowser3.Visible = true;
                    
                    web.DownloadFile(auth.captchaurl, disk.filesteamcaptcha, 0, 0);
                    LoadSteamCaptcha();
                    pbSteamCaptcha.Visible = true;
                    txtCaptcha.Visible = true;
                    Log("Captcha Required! Please retry login with captcha text..", Constants.LogMsgType.Error, false);
                    }
                else
                    {
                    pbSteamCaptcha.Visible = false;
                    webBrowser3.Visible = false;
                    txtCaptcha.Visible = false;
                    }

                if (stat == Constants.SignInStatus.SGuardReqd)
                    {
                    txtGuard.Visible = true;
                    webBrowser3.Visible = false;
                    pbSteamCaptcha.Visible = false;
                    Log("SteamGuard is on! Please retry login with SteamGuard email code..", Constants.LogMsgType.Error, false);
                    }
                else
                    {
                    txtGuard.Visible = false;
                    }
                Log(" ");
                lblLoginStatus.Text = "LOGIN FAILED";
                lblLoginStatus.ForeColor = Color.Red;
                btnSteamLogin.Enabled = true;
                }
            else if (stat == Constants.SignInStatus.LoggedIn)
                {
                btnSteamLogin.Text = "LOGOFF";
                tabPage1.Text = "LOGOFF";
                lblLoginStatus.Text = "LOGGED IN";
                lblLoginStatus.ForeColor = Color.Lime;
                webBrowser3.Visible = false;
                pbSteamCaptcha.Visible = false;
                txtGuard.Visible = false;
                txtCaptcha.Visible = false;
                Log(GetTime() + "Logged in successfully!", Constants.LogMsgType.Success, true);
                //mylist = new GatherList(auth.steamid, lic, this);

                Thread thrd;
                ThreadStart thrdStart;
                thrdStart = new ThreadStart(ProfileInitProc);
                thrd = new Thread(thrdStart);
                thrd.IsBackground = true;
                thrd.Start();

                

                btnSteamLogin.Enabled = true;
                txtCaptcha.Text = string.Empty;
                txtGuard.Text = string.Empty;
                TabDecorate(Constants.TabSituation.SteamLogIn);
                FillFromGroupCombo();
                if (cboFromGroup.Items.Count > 0)
                    cboFromGroup.SelectedIndex = 0;
                //rtfGatherStatus.Parent = tabPage1;
                //rtfGatherStatus.Top = pbSteamCaptcha.Top + pbSteamCaptcha.Height + 10;
                //rtfGatherStatus.Left = tabPage1.Left + 10;
                //rtfGatherStatus.Width = tabPage1.Width - 20;
                //rtfGatherStatus.Height = tabPage1.Height - 10 - rtfGatherStatus.Top;
                }
            else if (stat == Constants.SignInStatus.Authenticating)
                {
                btnSteamLogin.Text = "LOGGING IN";
                Log("Another authentication request is in progress, please wait..", Constants.LogMsgType.Error, true);
                btnSteamLogin.Enabled = false;
                }

            }
        private void ProfileInitProc()
            {
            Log(GetTime() + "Initializing profile...");
            profile.InitMinimal(auth.steamid, Constants.PlayerInitType.playerid);
            profileinitcomplete = true;
            Log(GetTime() + "Profile init complete!");
            
            FillToGroupCombo();
            updatingprofile = true;
            GGDisk disk = new GGDisk();
            disk.WritePlayerGroupStats(profile);
            updatingprofile = false;
            tmrSettingsUpdate.Enabled = true;
            //GGDisk disk = new GGDisk();
            //if (!(File.Exists(disk.filetogrps)))
            //    {
            //    updatingprofile = true;
            //    Log(GetTime() + "Downloading settings.. Please wait");
            //    UpdateProfileSettingsProc();
            //    Log("Settings update complete!", Constants.LogMsgType.Gator, true);
            //    }

            profile.SaveConfig();
            }
        private void LoadSteamCaptcha()
            {
            GGDisk disk = new GGDisk();
            FileStream fileStream = File.OpenRead(disk.filesteamcaptcha);
            MemoryStream memStream = new MemoryStream();
            memStream.SetLength(fileStream.Length);
            fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

            pbSteamCaptcha.Image = Image.FromStream(memStream);
            pbSteamCaptcha.Refresh();
            fileStream.Dispose();
            memStream.Dispose();
            }
        //public void CreateMyThread()
        //    {
        //    Thread thrd;
        //    ThreadStart thrdStart;
        //    thrdStart = new ThreadStart(targetproc);
        //    thrd = new Thread(thrdStart);
        //    thrd.IsBackground = true;
        //    thrd.Start();
        //    }
        private void SteamLoginClick()
            {
            btnSteamLogin.Enabled = false;
            profileinitcomplete = false;
            if (auth.status == Constants.SignInStatus.LoggedIn)
                {
                tmrSettingsUpdate.Enabled = false;
                Log(GetTime() + "Logging out..");
                auth.LogOut();
                Log(GetTime() + "Log out complete! Group Gator " + cversion + " Session End");
                auth.status = Constants.SignInStatus.LoggedOff;
                btnSteamLogin.Text = "LOGIN";
                tabPage1.Text = "LOGIN";
                lblLoginStatus.Text = "LOGGED OFF";
                lblLoginStatus.ForeColor = Color.Red;
                btnSteamLogin.Enabled = true;
                txtCaptcha.Text = string.Empty;
                txtGuard.Text = string.Empty;
                txtCaptcha.Visible = false;
                txtGuard.Visible = false;
                webBrowser3.Visible = false;
                pbSteamCaptcha.Visible = false;
                TabDecorate(Constants.TabSituation.SteamLogOff);
                }
            else
                {
                profile.uname = txtName.Text;
                profile.upass = txtPass.Text;
                lblLoginStatus.ForeColor = Color.Red;
                lblLoginStatus.Text = "INITIALIZING";
                Thread thrd;
                ThreadStart thrdStart;
                thrdStart = new ThreadStart(AuthProc1);
                thrd = new Thread(thrdStart);
                thrd.IsBackground = true;
                thrd.Start();                
                }
            }
        private void AuthProc1()
            {
            Log(GetTime() + "Initializing authentication..");
            lblLoginStatus.Text = "GETTING LOGIN PARAMETERS";
            auth.GetRSAKey(profile.uname);
            GGDisk disk = new GGDisk();
            wbLogin.Navigate(disk.filelogin);
            }
        public void LoadScriptFile()
            {
            //this.wbLogin.Navigate("about:blank");
            //IHTMLDocument2 doc2 = (IHTMLDocument2)this.wbLogin.Document.DomDocument; ;
            //string body = Gator25.Properties.Resources.steamjs;
            ////GGDisk disk = new GGDisk();
            ////disk.Write(body, disk.dirconfig + "\\login.html");
            ////wbLogin.Navigate(disk.dirconfig + "\\login.html");
            //doc2.write(body);
            //wbLogin.Refresh(WebBrowserRefreshOption.Completely);
            string body = Gator25.Properties.Resources.steamjs;

            GGDisk disk = new GGDisk();
            disk.Write(body, disk.filelogin);
            
            ////wbLogin.Navigate(disk.dirconfig + "\\login.html");


            }
#endregion

        #region GGDonor
        //public void DonorLogOffClick()
        //{
        //    panMain.Visible = true;
        //    tabControl1.Visible = false;
        //    panAdminLO.Visible = false;
        //    panAdminLI.Visible = false;
        //    txtAdminName.Text = "";
        //    txtAdminPass.Text = "";
        //    lblUsersOnline.Text = "";
        //    lblTimeLeft.Text = "";
        //    lblUserMail.Text = "FREE USER";

        //    //disable unlock and other options for freeuser
        //    btnSurvey.Visible = false;

        //    label64.Visible = false;
        //    txtDonAcToGiftTo.Visible = false;
        //    btnGift.Visible = false;
        //    btnUnlock.Visible = false;

        //    label66.Visible = false;
        //    tmrGGStats.Enabled = false;
        //    label10.Visible = false;
        //    txtDonDaysToGift.Visible = false;
        //    TabDecorate(Constants.TabSituation.DonorLogOff);

        //    if (mylist != null)
        //    {
        //        if (mylist.IsInviting == true)
        //            mylist.InviteStop();
        //        if (mylist.IsGathering == true)
        //            mylist.GatherStop();
        //        System.Threading.Thread.Sleep(2000);
        //        UnSavedListCheck();
        //    }
        //    profile.SaveConfig();

        //}
        //public void DonorLoggedInMakeup(string loggeduser)
        //{
        //    profile.gatoremail = txtDBName.Text;
        //    profile.gatorpass = txtDBPass.Text;

        //    lblFreeRem.Visible = false;
        //    panAdminLO.Visible = true;
        //    panAdminLI.Visible = false;
        //    TabDecorate(Constants.TabSituation.DonorLogIn);
        //    tabControl1.Visible = true;
        //    panMain.Visible = false;

        //    lblUserMail.Text = loggeduser;
        //    UpdateGGStats();
        //    tmrGGStats.Enabled = true;

        //    //enable unlock and other options for paid user
        //    btnSurvey.Visible = true;

        //    label64.Visible = true;
        //    txtDonAcToGiftTo.Visible = true;
        //    btnGift.Visible = true;
        //    btnUnlock.Visible = true;

        //    label66.Visible = true;
        //    label10.Visible = true;
        //    txtDonDaysToGift.Visible = true;

        //    WebService serv = new WebService();
        //    string trem = "0";
        //    serv.GetTimeRemaining(loggeduser, ref trem);
        //    double dbltime = 0.0;
        //    try
        //    {
        //        dbltime = Convert.ToDouble(trem);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log(GetTime() + "Conversion exception in LoggedInMakeup " + ex.Message, Constants.LogMsgType.Error, true);
        //    }
        //    TimeSpan t = TimeSpan.FromSeconds(dbltime);
        //    lblRemDays.Text = t.Days.ToString();
        //    lblRemHours.Text = t.Hours.ToString();
        //    lblRemMinutes.Text = t.Minutes.ToString();
        //    lblRemSeconds.Text = t.Seconds.ToString();
        //    lblFreeRem.Visible = false;
        //    lblRemDays.Visible = true;
        //    lblRemHours.Visible = true;
        //    lblRemMinutes.Visible = true;
        //    lblRemSeconds.Visible = true;
        //    label70.Visible = true;
        //    label71.Visible = true;
        //    label72.Visible = true;
        //    label73.Visible = true;

        //    lblTimeLeft.Visible = false;
        //    label57.Visible = false;


        //}
        public void LoadRecaptcha()
        {
            ////wbRecaptcha.Refresh(WebBrowserRefreshOption.Completely);

            MyWeb.MyWeb web = new MyWeb.MyWeb();
            WebService serv = new WebService();
            GGDisk disk = new GGDisk();
            web.DownloadFile(serv.urlcaptchagen, disk.fileggcaptcha, 0, 0);

            //MyUtils.MyUtils utils = new MyUtils.MyUtils();
            //string cid = utils.GetStrBetween(resp, "cid={", "}");
            //wbRecaptcha.Navigate(serv.urlcaptchabase + cid + ".png");
            //wbRecaptcha.Refresh(WebBrowserRefreshOption.Completely);
            ////wbRecaptcha.DocumentText = "<html><body><img src=\"" + serv.urlcaptchabase + cid + ".png\"></body></html>";
            try
            {
                if (File.Exists(disk.fileggcaptcha))
                {
                    FileStream fileStream = File.OpenRead(disk.fileggcaptcha);
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                    pbGGCaptcha.Image = Image.FromStream(memStream);
                    pbGGCaptcha.Refresh();
                    fileStream.Dispose();
                    memStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log(GetTime() + "LoadGGCaptcha Exception : " + ex.Message.ToString(), Constants.LogMsgType.Error, true);
            }
        }
        private void RegNewAccount()
        {

            Log(GetTime() + "Attempting to register new account...", Constants.LogMsgType.Success, false);
            if (txtPassNewAccount.Text != txtRePassNewAccount.Text)
            {
                MessageBox.Show("Your passwords don't match!", "Wrong Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log("Failed! Your passwords don't match, please check both passwords", Constants.LogMsgType.Error, true);
                return;
            }
            if (txtEmailNewAccount.Text == string.Empty)
            {
                MessageBox.Show("Your email field can not be blank", "Required field empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log("Failed! Your email field can not be blank", Constants.LogMsgType.Error, true);
                return;
            }
            if (txtPassNewAccount.Text == string.Empty)
            {
                MessageBox.Show("Your password field can not be blank", "Required field empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log("Failed! Your password field can not be blank", Constants.LogMsgType.Error, true);
                return;
            }

            btnRegNewAccount.Enabled = false;
            WebService wserv = new WebService();
            Constants.ServerResponse res = wserv.VerifyCaptcha(txtCaptchaNewAccount.Text, txtEmailNewAccount.Text, txtPassNewAccount.Text);
            if (res == Constants.ServerResponse.CaptchaBad)
            {
                MessageBox.Show("Your entered captcha text is wrong", "Captcha Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log("Failed! Your entered captcha text is wrong", Constants.LogMsgType.Error, true);
                LoadRecaptcha();
                btnRegNewAccount.Enabled = true;
                return;
            }
            else if (res == Constants.ServerResponse.AccountDuped)
            {
                MessageBox.Show("This email is already registerd, please choose a different email address", "Account Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log("Failed! This email is already registerd, please choose a different email address", Constants.LogMsgType.Error, true);
                LoadRecaptcha();
                btnRegNewAccount.Enabled = true;
                return;
            }
            else if (res == Constants.ServerResponse.AccountCreated)
            {
                Log("Success!", Constants.LogMsgType.Success, true);
                //DonorLoggedInMakeup(txtEmailNewAccount.Text);
                FingerPrint fp = new FingerPrint();
                string mid = fp.GetComuputerID();
                lic = Constants.SoftLicense.NotActivated;
                WebService serv = new WebService();
                serv.mid = mid;
                serv.Enter(Constants.SoftLicense.Demo);
                LoadRecaptcha();
                btnRegNewAccount.Enabled = true;
                return;
            }
        }
        private void StartFreeVersionClick()
        {
            btnStartFreeVersion.Enabled = false;
            //panAdminLI.Visible = false;
            //panAdminLO.Visible = false;
            FingerPrint fp = new FingerPrint();
            string mid = fp.GetComuputerID();
            lic = Constants.SoftLicense.Demo;
            WebService serv = new WebService();
            serv.mid = mid;
            serv.Enter(Constants.SoftLicense.Demo);
            TabDecorate(Constants.TabSituation.FreeLogin);
            tabControl1.Visible = true;
            panMain.Visible = false;

            //lblRemDays.Visible = false;
            //lblRemHours.Visible = false;
            //lblRemMinutes.Visible = false;
            //lblRemSeconds.Visible = false;
            //label70.Visible = false;
            //label71.Visible = false;
            //label72.Visible = false;
            //label73.Visible = false;

            WebService.ServerData sd = serv.CheckUsage();
            //lblUserMail.Text = "FREEUSER";
            Log("Thank you for trying free version of GroupGATOR " + cversion, Constants.LogMsgType.Error, true);
            Log("You have " + sd.iremain.ToString() + " invites and " + sd.gremain.ToString() + " gathers remaining", Constants.LogMsgType.Error, true);

            //lblFreeRem.Visible = true;
            //lblFreeRem.Text = sd.iremain.ToString() + " INVITES and " + sd.gremain.ToString() + " GATHERS";

            btnStartFreeVersion.Enabled = true;
        }
        private void DBLoginClick()
        {
            btnGGLogin.Enabled = false;
            Log(GetTime() + "Attemping to login as " + txtDBName.Text + "...", Constants.LogMsgType.Success, false);
            //BanCheck();

            FingerPrint fp = new FingerPrint();
            string mid = fp.GetComuputerID();

            WebService serv = new WebService();
            WebService.ServerData sdata = new WebService.ServerData();
            sdata.email = profile.gatoremail = txtDBName.Text;
            sdata.pass = profile.gatorpass = txtDBPass.Text;
            sdata.hid = mid;
            sdata.lockcomp = "false";
            Constants.ServerResponse resp = serv.Login(sdata);
            lic = Constants.SoftLicense.Demo;

            string timerem = string.Empty;
            Constants.ServerResponse tremres;

            tabControl1.Visible = false;
            panMain.Visible = true;

            switch (resp)
            {
                case Constants.ServerResponse.Valid:
                    Log("Success!", Constants.LogMsgType.Success, true);
                    lic = Constants.SoftLicense.Paid;
                    serv.Enter(lic);
                    //DonorLoggedInMakeup(txtDBName.Text);
                    tremres = serv.GetTimeRemaining(txtDBName.Text, ref timerem);
                    if (tremres == Constants.ServerResponse.Success)
                        Log("You have " + FormatTime(timerem, Constants.FormatTimeType.Verbose) + " of usage remaining", Constants.LogMsgType.Gator, true);

                    TabDecorate(Constants.TabSituation.DonorLogIn);
                    tabControl1.Visible = true;
                    panMain.Visible = false;



                    break;
                case Constants.ServerResponse.Expired:
                    //DonorLogOffClick();
                    Log("Failed! Subscription expired", Constants.LogMsgType.Error, true);
                    MessageBox.Show("Your current subscription expired! Please renew", "Subscription expired!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case Constants.ServerResponse.JustLocked:
                    Log("Success! Your licence has been locked for this device", Constants.LogMsgType.Success, true);
                    lic = Constants.SoftLicense.Paid;
                    serv.Enter(lic);
                    TabDecorate(Constants.TabSituation.DonorLogIn);
                    tabControl1.Visible = true;
                    panMain.Visible = false;

                    tremres = serv.GetTimeRemaining(txtDBName.Text, ref timerem);
                    if (tremres == Constants.ServerResponse.Success)
                        Log("You have " + FormatTime(timerem, Constants.FormatTimeType.Verbose) + " remaining", Constants.LogMsgType.Gator, true);

                    break;
                case Constants.ServerResponse.NoAccount:
                    //DonorLogOffClick();
                    Log("Failed! Account does not exist", Constants.LogMsgType.Error, true);
                    MessageBox.Show("No registered account found for this username", "Invalid Username/Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case Constants.ServerResponse.NotLocked:
                    //disclaimer
                    //DonorLogOffClick();
                    frmDisclamer ddlg = new frmDisclamer();
                    Constants.DislcaimerResponse dres = ddlg.Display();
                    if (dres == Constants.DislcaimerResponse.Agree)
                    {
                        sdata.email = txtDBName.Text;
                        sdata.pass = txtDBPass.Text;
                        sdata.hid = mid;
                        sdata.lockcomp = "true";
                        resp = serv.Login(sdata);
                        if (resp == Constants.ServerResponse.JustLocked)
                        {
                            Log("Success! Your licence has been locked for this device", Constants.LogMsgType.Success, true);
                            lic = Constants.SoftLicense.Paid;
                            serv.Enter(lic);
                            TabDecorate(Constants.TabSituation.DonorLogIn);
                            tabControl1.Visible = true;
                            panMain.Visible = false;
                            
                            tremres = serv.GetTimeRemaining(txtDBName.Text, ref timerem);
                            if (tremres == Constants.ServerResponse.Success)
                                Log("You have " + FormatTime(timerem, Constants.FormatTimeType.Verbose) + " remaining", Constants.LogMsgType.Gator, true);
                            break;
                        }
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                    break;
                case Constants.ServerResponse.WrongComputer:
                    //DonorLogOffClick();
                    Log("Failed! Licence invalid for this device", Constants.LogMsgType.Error, true);
                    MessageBox.Show("Your current licence is not valid for this device", "Lock Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            btnGGLogin.Enabled = true;
        }
        //private void UnlockNormalClick()
        //{
        //    WebService serv = new WebService();

        //    Log(GetTime() + "Attempting to unlock account - " + profile.gatoremail + " ", Constants.LogMsgType.Success, false);
        //    Constants.ServerResponse res = serv.UnlockNormal(profile.gatoremail, profile.gatorpass);
        //    if (res == Constants.ServerResponse.UnlockSuccess)
        //    {
        //        Log("Success!", Constants.LogMsgType.Success, true);
        //        //MessageBox.Show("Account (" + profile.gatoremail + ") unlocked successfully", "Success", MessageBoxButtons.OK);
        //        MessageBox.Show("The account (" + profile.gatoremail + ") has been unlocked. Please exit and restart GroupGATOR on the machine of your choice", "Unlocked!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        DonorLogOffClick();
        //        Environment.Exit(0);
        //    }
        //    else if (res == Constants.ServerResponse.UnlockFailWrongAccount)
        //    {
        //        Log("Failed! The provided account is not valid for unlocking", Constants.LogMsgType.Error, true);
        //        MessageBox.Show("Failed! The provided account is not valid for unlocking", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //private void DonateClicked(Constants.DonateType dtype, bool ispaypal)
        //{
        //    Gator25.PayPalWindow paypal = new Gator25.PayPalWindow();
        //    if (ispaypal == true)
        //        paypal.DisplayPaypal(dtype);
        //    else
        //        paypal.DisplayGoogle(dtype);

        //    //string reqfile = string.Empty;
        //    //GGDisk disk = new GGDisk();
        //    //switch (dtype)
        //    //    {
        //    //    case Constants.DonateType.Donate1:
        //    //        reqfile = disk.filedonator1;
        //    //        break;
        //    //    case Constants.DonateType.Donate2:
        //    //        reqfile = disk.filedonator2;
        //    //        break;
        //    //    case Constants.DonateType.Donate3:
        //    //        reqfile = disk.filedonator3;
        //    //        break;
        //    //    case Constants.DonateType.Donate4:
        //    //        reqfile = disk.filedonator4;
        //    //        break;
        //    //    case Constants.DonateType.Donate5:
        //    //        reqfile = disk.filedonator5;
        //    //        break;
        //    //    case Constants.DonateType.Donate6:
        //    //        reqfile = disk.filedonator6;
        //    //        break;
        //    //    }
        //    //if (File.Exists(reqfile))
        //    //    {
        //    //    try
        //    //        {
        //    //        System.Diagnostics.Process.Start(reqfile);
        //    //        }
        //    //    catch (Exception ex)
        //    //        {
        //    //        Log("Exception in donate : " + ex.Message.ToString(), Constants.LogMsgType.Error, true);
        //    //        }
        //    //    }

        //}
        //private void DonorGiftClick()
        //{
        //    DialogResult dlgres = MessageBox.Show("This will deduct " + txtDonDaysToGift.Text + "days from your subscription and will add the amount to " + txtDonAcToGiftTo.Text + ". Are you sure you want to continue?", "Confirmation Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (dlgres == DialogResult.Yes)
        //    {
        //        WebService serv = new WebService();
        //        Log(GetTime() + "Attempting to gift " + txtDonDaysToGift.Text + "days to " + txtDonAcToGiftTo.Text + "...", Constants.LogMsgType.Success, false);
        //        Constants.ServerResponse res = serv.GiftDays(txtDonAcToGiftTo.Text, profile.gatoremail, profile.gatorpass, txtDonDaysToGift.Text);
        //        if (res == Constants.ServerResponse.GiftSuccess)
        //        {
        //            Log("Success!", Constants.LogMsgType.Success, true);
        //            MessageBox.Show("Account (" + txtDonAcToGiftTo.Text + ") gifted with " + txtDonDaysToGift.Text + " days", "Success", MessageBoxButtons.OK);
        //            string timerem = "0";
        //            res = serv.GetTimeRemaining(profile.gatoremail, ref timerem);
        //            if (res == Constants.ServerResponse.Success)
        //            {
        //                Log("You have " + FormatTime(timerem, Constants.FormatTimeType.Verbose) + " remaining", Constants.LogMsgType.Gator, true);
        //                double dbltime = 0.0;
        //                try
        //                {
        //                    dbltime = Convert.ToDouble(timerem);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Log(GetTime() + "Conversion exception in FormatTime2 " + ex.Message, Constants.LogMsgType.Error, true);
        //                }
        //                TimeSpan t = TimeSpan.FromSeconds(dbltime);
        //                lblRemDays.Text = t.Days.ToString();
        //                lblRemHours.Text = t.Hours.ToString();
        //                lblRemMinutes.Text = t.Minutes.ToString();
        //                lblRemSeconds.Text = t.Seconds.ToString();
        //            }

        //        }
        //        else if (res == Constants.ServerResponse.GiftFailNoDonor)
        //        {
        //            Log("Failed! You are not a valid donor to gift days", Constants.LogMsgType.Error, true);
        //            MessageBox.Show("Failed! You are not a valid donor to gift days", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        else if (res == Constants.ServerResponse.GiftFailNoReceiver)
        //        {
        //            Log("Failed! The provided receiver account does not exist", Constants.LogMsgType.Error, true);
        //            MessageBox.Show("Failed! The provided receiver account does not exist", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        else if (res == Constants.ServerResponse.GiftFailNotEnough)
        //        {
        //            Log("Failed! You dont have sufficient time left in your account to gift", Constants.LogMsgType.Error, true);
        //            MessageBox.Show("Failed! You dont have sufficient time left in your account to gift", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}
        #endregion

        //#region GGAdmin
        //private void AdminLoginClick()
        //{
        //    WebService serv = new WebService();
        //    Log(GetTime() + "Attempting to log in as admin.. ", Constants.LogMsgType.Success, false);
        //    Constants.ServerResponse res = serv.AdminCheck(txtAdminName.Text, txtAdminPass.Text);
        //    if (res == Constants.ServerResponse.AcheckSuccess)
        //    {
        //        panAdminLI.Visible = true;
        //        panAdminLO.Visible = false;
        //        Log("Success!", Constants.LogMsgType.Success, true);
        //        tmrGGStats.Enabled = true;
        //        //lblUsersOnline.Text = serv.GetActiveUsersCount();
        //    }
        //    else
        //    {
        //        panAdminLI.Visible = false;
        //        panAdminLO.Visible = true;
        //        MessageBox.Show("Your attempt to login as administrator failed because your account is not valid", "Login Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        Log("Failed! Your administrator account is not valid", Constants.LogMsgType.Error, true);
        //    }
        //}
        //private void UnlockAccountClick()
        //{
        //    WebService serv = new WebService();
        //    Log(GetTime() + "Attempting to unlock account - " + txtAcUnlock.Text + " ", Constants.LogMsgType.Success, false);
        //    Constants.ServerResponse res = serv.Unlock(txtAcUnlock.Text, txtAdminName.Text, txtAdminPass.Text);
        //    if (res == Constants.ServerResponse.UnlockSuccess)
        //    {
        //        Log("Success!", Constants.LogMsgType.Success, true);
        //        MessageBox.Show("Account (" + txtAcUnlock.Text + ") unlocked successfully", "Success", MessageBoxButtons.OK);
        //        if (txtAcUnlock.Text == profile.gatoremail)
        //        {
        //            if (mylist != null)
        //            {
        //                if (mylist.IsInviting == true)
        //                    mylist.InviteStop();
        //                if (mylist.IsGathering == true)
        //                    mylist.GatherStop();
        //                System.Threading.Thread.Sleep(2000);
        //                UnSavedListCheck();
        //            }
        //            profile.SaveConfig();
        //            Environment.Exit(0);
        //        }
        //    }
        //    else if (res == Constants.ServerResponse.UnlockFailNotAdmin)
        //    {
        //        Log("Failed! You must be logged in as administrator to unlock", Constants.LogMsgType.Error, true);
        //        MessageBox.Show("Failed! You must have administrator privileges to unlock", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    else if (res == Constants.ServerResponse.UnlockFailWrongAccount)
        //    {
        //        Log("Failed! The provided account does not exist", Constants.LogMsgType.Error, true);
        //        MessageBox.Show("Failed! The provided account does not exist", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //private void GetTimeRemainingClick(string email)
        //{
        //    label57.Visible = true;
        //    lblTimeLeft.Visible = true;
        //    WebService serv = new WebService();
        //    Log(GetTime() + "checking remaining time for account - " + email + " ", Constants.LogMsgType.Success, false);
        //    string tremain = string.Empty;
        //    Constants.ServerResponse res = serv.GetTimeRemaining(txtAcGetTime.Text, ref tremain);
        //    if (res == Constants.ServerResponse.Success)
        //    {
        //        Log("Success!", Constants.LogMsgType.Success, true);
        //        lblTimeLeft.Text = FormatTime(tremain, Constants.FormatTimeType.Dot);
        //    }
        //    else if (res == Constants.ServerResponse.Failed)
        //    {
        //        Log("Failed!", Constants.LogMsgType.Error, true);
        //    }
        //}
        private string FormatTime(string time, Constants.FormatTimeType type)
        {
            double dbltime = 0.0;
            try
            {
                dbltime = Convert.ToDouble(time);
            }
            catch (Exception ex)
            {
                Log(GetTime() + "Conversion exception in FormatTime " + ex.Message, Constants.LogMsgType.Error, true);
            }
            TimeSpan t = TimeSpan.FromSeconds(dbltime);
            string ret = string.Empty;
            if (type == Constants.FormatTimeType.Dot)
            {
                ret = t.Days.ToString() + "." + t.Hours.ToString() + "." + t.Minutes.ToString() + "." + t.Seconds.ToString();
            }
            else
            {
                ret = t.Days.ToString() + " days " + t.Hours.ToString() + " hours " + t.Minutes.ToString() + " minutes " + t.Seconds.ToString() + " seconds ";
            }
            return ret;
        }
        //private void AddTimeClick()
        //{
        //    WebService serv = new WebService();
        //    Log(GetTime() + "Attempting to add " + txtDaysToAdd.Text + "days to " + txtAcAddTime.Text, Constants.LogMsgType.Success, false);
        //    Constants.ServerResponse res = serv.AddTime(txtAcAddTime.Text, txtAdminName.Text, txtAdminPass.Text, txtDaysToAdd.Text);
        //    if (res == Constants.ServerResponse.UnlockSuccess)
        //    {
        //        Log("Success!", Constants.LogMsgType.Success, true);
        //        MessageBox.Show("Account (" + txtAcAddTime.Text + ") added with " + txtDaysToAdd.Text + " days", "Success", MessageBoxButtons.OK);
        //    }
        //    else if (res == Constants.ServerResponse.UnlockFailNotAdmin)
        //    {
        //        Log("Failed! You must be logged in as administrator to add days", Constants.LogMsgType.Error, true);
        //        MessageBox.Show("Failed! You must have administrator privileges to add days", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    else if (res == Constants.ServerResponse.UnlockFailWrongAccount)
        //    {
        //        Log("Failed! The provided account does not exist", Constants.LogMsgType.Error, true);
        //        MessageBox.Show("Failed! The provided account does not exist", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //private void SetTimeClick()
        //{
        //    WebService serv = new WebService();
        //    Log(GetTime() + "Attempting to set " + txtDaysToSet.Text + "days to " + txtAcSetTime.Text, Constants.LogMsgType.Success, false);
        //    Constants.ServerResponse res = serv.SetTime(txtAcSetTime.Text, txtAdminName.Text, txtAdminPass.Text, txtDaysToSet.Text);
        //    if (res == Constants.ServerResponse.UnlockSuccess)
        //    {
        //        Log("Success!", Constants.LogMsgType.Success, true);
        //        MessageBox.Show("Account (" + txtAcSetTime.Text + ") set with " + txtDaysToSet.Text + " days", "Success", MessageBoxButtons.OK);
        //    }
        //    else if (res == Constants.ServerResponse.UnlockFailNotAdmin)
        //    {
        //        Log("Failed! You must be logged in as administrator to set days", Constants.LogMsgType.Error, true);
        //        MessageBox.Show("Failed! You must have administrator privileges to set days", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    else if (res == Constants.ServerResponse.UnlockFailWrongAccount)
        //    {
        //        Log("Failed! The provided account does not exist", Constants.LogMsgType.Error, true);
        //        MessageBox.Show("Failed! The provided account does not exist", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //#endregion

        private void chkRandomizeISpeed_CheckedChanged(object sender, EventArgs e)
            {
            if (chkRandomizeISpeed.Checked == true)
                {
                txtMaxWaitTime.Enabled = true;
                int tmp = trckInviteSpeed.Value + 1;
                txtMaxWaitTime.Text = tmp.ToString();
                }
            else
                {
                txtMaxWaitTime.Enabled = false;

                }
            InviteSpeedChange();
            }
        int trcklast = 15;
        private void trckInviteSpeed_ValueChanged(object sender, EventArgs e)
            {
            if (editvalueworking == false)
                {

                tmrEdit.Enabled = true;
                tickcounts = Environment.TickCount;
                pbValueGot.Image = null;
                }
            lblCurISpeed.Text = trckInviteSpeed.Value.ToString();
            //InviteSpeedChange();            
            }

        private bool isdlgon = false;
        private void InviteSpeedChange()
            {
            int tmpinv = trckInviteSpeed.Value;
            int tmpmaxw = 0;
            try
                {
                tmpmaxw = Convert.ToInt32(txtMaxWaitTime.Text);
                }
            catch (Exception ex)
                {
                tmpmaxw = 16;
                }
            int tmpmaxiph = 0;
            try
                {
                tmpmaxiph = Convert.ToInt32(txtMaxIPH.Text);
                }
            catch (Exception ex)
                {
                tmpmaxiph = 100;
                }
            int tmpfdelay = 0;
            try
                {
                tmpfdelay = Convert.ToInt32(txtFailDelay.Text);
                }
            catch (Exception ex)
                {
                tmpfdelay = 15;
                }

            if (auth != null)
                {
                if (auth.status == Constants.SignInStatus.LoggedIn)
                    {
                    if (tmpinv < 15)
                        {
                        if (isdlgon == false)
                            {
                            isdlgon = true;
                            DialogResult res = MessageBox.Show("WARNING!!! Setting your invite speed faster than 1 every 15 seconds could get your account flagged or banned. Press YES if you agree to assume the full risk of doing so, press NO if you would like to set it back to what it was", "WARNING!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (res == DialogResult.No)
                                {
                                //trckInviteSpeed.Value = 15;
                                //lblCurISpeed.Text = trckInviteSpeed.Value.ToString();
                                //tmpinv = 15;
                                tmpinv = trcklast;
                                if (chkRandomizeISpeed.Checked == true)
                                    {
                                    if (tmpmaxw < 15)
                                        tmpmaxw = 16;
                                    }
                                }
                            }
                        }
                    }
                }
            

            if (chkRandomizeISpeed.Checked == true)
                {
                if ( (tmpmaxw < tmpinv) && ( tmpmaxw > 15 ) )
                    {                    
                    tmpinv = tmpmaxw - 1;
                    }
                else if ((tmpmaxw < tmpinv) && (tmpmaxw <= 15))
                    {
                    tmpinv = 15;
                    tmpmaxw = 16;
                    }
                }
            trcklast = tmpinv;

            try
                {
                sdelay.maxiph = Convert.ToInt32(txtMaxIPH.Text);
                }
            catch (Exception ex)
                {
                sdelay.maxiph = 100;
                Log("conversion invite speed exception : " + ex.Message.ToString(), Constants.LogMsgType.Error, true);
                }
            sdelay.curdelay = tmpinv;
            sdelay.faildelay = tmpfdelay;
            if (chkRandomizeISpeed.Checked == true)
                sdelay.israndom = true;
            else
                sdelay.israndom = false;
            sdelay.maxwait = tmpmaxw;

            bool ret = true;
            if (mylist != null)
                {
                ret = mylist.SetInviteSpeed(sdelay);
                }
            if (profile != null)
                {
                profile.idelay = sdelay.curdelay;
                profile.fdelay = sdelay.faildelay;
                profile.israndomdelayon = sdelay.israndom;
                profile.maxwait = sdelay.maxwait;
                profile.miph = sdelay.maxiph;

                }
            trckInviteSpeed.Value = tmpinv;
            txtMaxWaitTime.Text = tmpmaxw.ToString();
            txtFailDelay.Text = tmpfdelay.ToString();
            lblCurISpeed.Text = tmpinv.ToString();
            }

        private void EditStartCountClick()
            {
            txtInviteBeginCount.Text = lblStartCount.Text;
            txtInviteBeginCount.Visible = true;
            lblStartCount.Visible = false;
            btnSaveInviteBegin.Visible = true;
            }
        private void btnEditStartCount_Click(object sender, EventArgs e)
            {
            EditStartCountClick();
            }
        private void txtInviteBeginCount_TextChanged_1(object sender, EventArgs e)
            {
            lblStartCount.Text = txtInviteBeginCount.Text;
            }

        private void button1_Click(object sender, EventArgs e)
            {
            WebService ws = new WebService( );
            ws.UpdateGlobalInviteCount( 5 );
            

            }



        private int tickcounts = 0;
        private bool editvalueworking = false;
        private void txtFailDelay_TextChanged(object sender, EventArgs e)
            {
            if (editvalueworking == false)
                {

                tmrEdit.Enabled = true;
                tickcounts = Environment.TickCount;
                pbValueGot.Image = null;

                }
            
            }
        private void txtMaxIPH_TextChanged(object sender, EventArgs e)
            {
            //InviteSpeedChange();
            if (editvalueworking == false)
                {
                editvalueworking = true;
                tmrEdit.Enabled = true;
                tickcounts = Environment.TickCount;
                pbValueGot.Image = null;
                }
            }
        private void txtMaxWaitTime_TextChanged(object sender, EventArgs e)
            {
            if (editvalueworking == false)
                {
                
                tmrEdit.Enabled = true;
                tickcounts = Environment.TickCount;
                pbValueGot.Image = null;
                }

            }
        private void tmrEdit_Tick(object sender, EventArgs e)
            {
            int tickcntcur = Environment.TickCount;
            if ((tickcntcur - tickcounts) >= 1200)
                {

                InviteSpeedChange();
                tmrEdit.Enabled = false;
                editvalueworking = false;
                pbValueGot.Image = Gator25.Properties.Resources.gtick;

                }

            }

       

        private void btnSurvey_Click(object sender, EventArgs e)
            {
            Gator25.frmAds adDlg = new Gator25.frmAds(profile.gatoremail);
            adDlg.ShowDialog();
            }

       


        private void UpdateGGStats()
            {

            WebService serv = new WebService();
            //lblUsersOnline.Text = serv.GetActiveUsersCount();

            Gator25.AdAPI ads = new Gator25.AdAPI(profile.gatoremail);
            //Gator25.AdAPI ads = new Gator25.AdAPI("grouppropagator@gmail.com");              
            int n = ads.GetUserPoints();
            //label67.Text = n.ToString() + "GP";
            
            if (lic == Constants.SoftLicense.Paid)
                {
                //serv = new WebService();
                string trem = "0";
                serv.GetTimeRemaining(profile.gatoremail, ref trem);
                double dbltime = 0.0;
                try
                    {
                    dbltime = Convert.ToDouble(trem);
                    }
                catch (Exception ex)
                    {
                    Log(GetTime() + "Conversion exception in LoggedInMakeup " + ex.Message, Constants.LogMsgType.Error, true);
                    }
                TimeSpan t = TimeSpan.FromSeconds(dbltime);
                //lblRemDays.Text = t.Days.ToString();
                //lblRemHours.Text = t.Hours.ToString();
                //lblRemMinutes.Text = t.Minutes.ToString();
                //lblRemSeconds.Text = t.Seconds.ToString();
                }
            else
                {
                WebService.ServerData sd = serv.CheckUsage();
                //lblUserMail.Text = "FREEUSER";
                //lblFreeRem.Text = sd.iremain.ToString() + " INVITES and " + sd.gremain.ToString() + " GATHERS";

                }
            }

        private void disclaimerToolStripMenuItem1_Click(object sender, EventArgs e)
            {
            frmDisclamer ddlg = new frmDisclamer();
            Constants.DislcaimerResponse dres = ddlg.Display();
            if (dres == Constants.DislcaimerResponse.Decline)
                {
                Environment.Exit(0);
                }
            }

        //private void cboGatherSpeed_SelectedIndexChanged(object sender, EventArgs e)
        //    {
        //    if (cboGatherSpeed.SelectedIndex == 2)
        //        {
        //        label68.Enabled = true;
        //        txtMaxDaysOffline.Enabled = true;
        //        }
        //    else
        //        {
        //        label68.Enabled = false;
        //        txtMaxDaysOffline.Enabled = false;
        //        }
        //    }

        private void toolStripMenuItem65_Click_1(object sender, EventArgs e)
            {
            EditStartCountClick();
            }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
            {
            ExitClick();
            }

        private void saveLogToTxtFileToolStripMenuItem_Click(object sender, EventArgs e)
            {
            if (saveLogToTxtFileToolStripMenuItem.Checked == true)
                saveLogToTxtFileToolStripMenuItem.Checked = false;
            else
                saveLogToTxtFileToolStripMenuItem.Checked = true;
            }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
            {
            if (File.Exists(ggfile.filelog))
                {
                try
                    {
                    System.Diagnostics.Process.Start(ggfile.filelog);
                    }
                catch (Exception ex)
                    {
                    }
                }
            }

        private void clearLogFileToolStripMenuItem_Click(object sender, EventArgs e)
            {
            try
                {
                if (File.Exists(ggfile.filelog))
                    File.Delete(ggfile.filelog);
                }
            catch (Exception ex)
                {
                }

            }

        private void toolStripMenuItem66_Click_1(object sender, EventArgs e)
            {
            rtfGatherStatus.Clear();
            }



        //private void btnGoogleDonate_Click(object sender, EventArgs e)
        //    {
        //    switch (cboGoogleRates.SelectedIndex)
        //        {
        //        case 0:
        //            DonateClicked(Constants.DonateType.Donate1, false);
        //            break;
        //        case 1:
        //            DonateClicked(Constants.DonateType.Donate2, false);
        //            break;
        //        case 2:
        //            DonateClicked(Constants.DonateType.Donate3, false);
        //            break;
        //        case 3:
        //            DonateClicked(Constants.DonateType.Donate4, false);
        //            break;
        //        case 4:
        //            DonateClicked(Constants.DonateType.Donate5, false);
        //            break;
        //        case 5:
        //            DonateClicked(Constants.DonateType.Donate6, false);
        //            break;
        //        }
        //    }

        

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)

            {
            if (tabControl1.SelectedTab == tabPage1)
                {
                //rtfGatherStatus.Parent = tabPage1;
                //rtfGatherStatus.Top = pbSteamCaptcha.Top + pbSteamCaptcha.Height + 10;
                //rtfGatherStatus.Left = tabPage1.Left + 5;
                //rtfGatherStatus.Width = tabPage1.Width - 10;
                //rtfGatherStatus.Height = tabPage1.Height - 10 - rtfGatherStatus.Top;
                }
            //else if (tabControl1.SelectedTab == tabPage2)
            //    {
            //    rtfGatherStatus.Parent = tabPage2;
            //    rtfGatherStatus.Top = btnGatherStart.Top + btnGatherStart.Height + 5 ;
            //    rtfGatherStatus.Left = 5;
            //    rtfGatherStatus.Width = tabPage2.Width / 2;
            //    rtfGatherStatus.Height = tabPage6.Height -20;
            //    }
            else
                {
                rtfGatherStatus.Parent = tabPage6;
                rtfGatherStatus.Top = 10;
                rtfGatherStatus.Left =10;
                rtfGatherStatus.Width = tabPage6.Width -20 ;
                rtfGatherStatus.Height = tabPage6.Height -20;
                }
        
            }


        private void chkAutoPopulate_CheckedChanged(object sender, EventArgs e)
            {
            if (chkAutoPopulate.Checked == true)
                {
                //thread autopopulate launch
                ThreadStart ts = new ThreadStart(AutoPopulateProc);
                Thread t = new Thread(ts);
                t.Start();
                t.IsBackground = true;
                }
           
            }

        private void btnCreate_Click(object sender, EventArgs e)
            {
            if (auth.status == Constants.SignInStatus.LoggedIn)
                {
                if (mylist != null)
                    {
                    DecorateGatherList( );
                    //ThreadStart tstmp = new ThreadStart( DecorateGatherList );
                    //Thread ttmp = new Thread( tstmp );
                    //ttmp.IsBackground = true;
                    //ttmp.Start( );
                    
                    }
                }
            }
        string togroupnew = string.Empty;
        private void DecorateGatherList ( )
            {
            btnGatherStart.Enabled = false;
            btnGatherStop.Enabled = false;
            btnCreate.Enabled = false;
            //try
            //    {
            Log( "starting to create the gather list.." );
            togroupnew = string.Empty;
            if (chkSkipBlackListed.Checked == true)
                {
                togroupnew = cboToGroup.Text;
                if (togroupnew == string.Empty)
                    {
                    togroupnew = togrpsel.Display( );
                    cboToGroup.Text = togroupnew;
                    }
                }


            bgwCreateList.RunWorkerAsync( );

            
                //}
            //catch (Exception ex)
            //    {
            //    Log("exception in creating list: " + ex.Message.ToString(), Constants.LogMsgType.Error, true);
            //    }
            }


        public void DisableGatherControls()
            {
            chkSkipFirstPage.Enabled = false;
            chkSkipDupes.Enabled = false;
            chkSkipBlackListed.Enabled = false;
            chkIngame.Enabled = false;
            chkOnline.Enabled = false;
            chkOffLine.Enabled = false;
            chkRandomizeList.Enabled = false;
            chkSkipFirstPage.Checked = false;
            chkSkipDupes.Checked = false;
            chkSkipBlackListed.Checked = false;
            chkIngame.Checked = false;
            chkOnline.Checked = false;
            chkOffLine.Checked = false;
            chkRandomizeList.Checked = false;
            btnCreate.Enabled = false;
            pbCGLblack.Visible = false;
            pbCGLdupes.Visible = false;
            pbCGLfirstpage.Visible = false;
            pbCGLingame.Visible = false;
            pbCGLoffline.Visible = false;
            pbCGLonline.Visible = false;
            pbCGLrandmize.Visible = false;
            pbarCreateGL.Visible = false;

            }
        public void EnableGatherControls()
            {
            chkSkipFirstPage.Enabled = true;
            chkSkipDupes.Enabled = true;
            chkSkipBlackListed.Enabled = true;
            chkIngame.Enabled = true;
            chkOnline.Enabled = true;
            chkOffLine.Enabled = true;
            chkRandomizeList.Enabled = true;
            btnCreate.Enabled = true;
            pbAjax.Visible = false;
            }

        private int icurtogrp = 0;
        private void tmrInviteAll_Tick(object sender, EventArgs e)
            {
            if (mylist == null)
                mylist = new GatherList(profile.player.id, lic, this);
            if (mylist.IsInviting == true)
                return;

            if (icurtogrp > cboToGroup.Items.Count - 1)
                {
                if (mylist.IsInviting == false)
                    {
                    tmrInviteAll.Enabled = false;
                    Log("GroupGATOR has completed iniviting to all groups!", Constants.LogMsgType.Success, true);
                    chkInviteToAll.Enabled = true;                    
                    }
                return;
                }




            cboToGroup.SelectedIndex = icurtogrp;


            
            lblStatusGather.Text = "INVITING";
            lblStatusGather.ForeColor = Color.Lime;
            auth.UpdateSessionID();
            int tmpidelay = trckInviteSpeed.Value;
            int tmpmaxw = 0;
            try
                {
                tmpmaxw = Convert.ToInt32(txtMaxWaitTime.Text);
                }
            catch (Exception ex)
                {
                tmpmaxw = 16;
                }
            int tmpmaxiph = 0;
            try
                {
                tmpmaxiph = Convert.ToInt32(txtMaxIPH.Text);
                }
            catch (Exception ex)
                {
                tmpmaxiph = 100;
                }
            int tmpfdelay = 0;
            try
                {
                tmpfdelay = Convert.ToInt32(txtFailDelay.Text);
                }
            catch (Exception ex)
                {
                tmpfdelay = 15;
                }
            profile.idelay = tmpidelay;
            profile.fdelay = tmpfdelay;
            profile.miph = tmpmaxiph;
            profile.maxwait = tmpmaxw;
            mylist.Invite(cboToGroup.Text, auth.cookie, auth.sessionid, profile);


            icurtogrp++;
            }

 

        private void tmrSecurity_Tick(object sender, EventArgs e)
            {
            if (mylist != null)
                {
                
                if (auth.status == Constants.SignInStatus.LoggedIn)
                    {
                    SecurityCheckTick( );
                    }
                }
            }

        public int iremain = 0;
        public int gremain = 0;
        private int tremain = 0;
        private int tickpast = 0;


        public void DecIRemain ( )
            {
            --iremain;
            if (iremain <= 0)
                {
                if (lic == Constants.SoftLicense.Demo)
                    {
                    if (mylist != null)
                        mylist.InviteStop( );
                    //btnInviteStart.Enabled = false;
                    //btnInviteStart.ForeColor = Color.White;
                    if (ierrshown == false)
                        {
                        ierrshown = true;
                        MessageBox.Show( "Your free invite limit (1000) has been exceeded, please renew for continued usage! Thank you", "Free invite limit reached!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        }                    
                    }
                }
            }

        private static readonly object _lockgremain = new object( );
        private bool gerrshown = false;
        private bool ierrshown = false;
        private bool terrshown = false;
        public void DecGRemain ( int n )
            {
            lock (_lockgremain)
                {
                gremain -= n;
                if (gremain <= 0)
                    {
                    if (lic == Constants.SoftLicense.Demo)
                        {
                        if (mylist != null)
                            mylist.GatherStop( );
                        ////btnGatherStart.Enabled = false;
                        ////btnGatherStart.ForeColor = Color.White;
                        if (gerrshown == false)
                            {
                            gerrshown = true;
                            MessageBox.Show( "Your free gather limit (3000) has been exceeded, please renew for continued usage! Thank you", "Free gather limit reached!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                            }
                        }
                    }
                }
            }

        private void BanCheck ()
            {
            WebService ws = new WebService( );
            if (ws.IsBanned( profile.gatoremail ) == true)
                {
                if (mylist != null)
                    {
                    mylist.GatherStop( );
                    mylist.InviteStop( );
                    }
                //btnInviteStart.Enabled = false;
                //btnGatherStart.Enabled = false;
                MessageBox.Show( "You are banned from GroupGATOR network", "Banned!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                Environment.Exit( 0 );
                }
            }

        private void SecurityCheckTick ( )
            {
            WebService ws = new WebService( );
            WebService.ServerData sd = new WebService.ServerData( );
            sd.iremain = 0;
            sd.gremain = 0;
            sd.tremain = 0;

            BanCheck();

            if (mylist != null)
                {
                sd.iremain = mylist.nICur;
                sd.gremain = mylist.nGCur;
                ws.UpdateUsage( sd );
                mylist.nICur = 0;
                mylist.nGCur = 0;
                ws.UpdateGlobalInviteCount( sd.iremain );

                sd = ws.CheckUsage( );
                gremain = sd.gremain;
                iremain = sd.iremain;
                tremain = sd.tremain;
                }

            if (lic == Constants.SoftLicense.Paid)
                {
                int ticknow = Environment.TickCount;
                int diff = ticknow - tickpast;
               

                if (tremain - diff <= 0)
                    {       
                    if ( mylist != null )
                        {
                        mylist.GatherStop( );
                        mylist.InviteStop( );
                        }
                    //btnInviteStart.Enabled = false;
                    //btnGatherStart.Enabled = false;
                    //btnGatherStart.ForeColor = Color.White;
                    //btnInviteStart.ForeColor = Color.White;
                    if (terrshown == false)
                        {
                        terrshown = true;
                        MessageBox.Show( "Your subscription period has ended, please renew for continued usage! Thank you", "Subscription expired!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        }
                    ExitClick( );
                    }


                }
            else if (lic == Constants.SoftLicense.Demo)
                {
                
                    
                //if ((gerrshown == true) && (ierrshown == true))
                //    Environment.Exit( 0 );
                if (gremain <= 0)
                    {
                    if (mylist != null)
                        {
                        mylist.GatherStop( );
                        sd.iremain = 0;
                        sd.gremain = 4000;
                        ws.UpdateUsage( sd );
                        mylist.nICur = 0;
                        mylist.nGCur = 0;
                        ws.UpdateGlobalInviteCount( sd.iremain );
                        }
                    if (gerrshown  == false)
                        {
                        //btnGatherStart.Enabled = false;
                        //btnGatherStart.ForeColor = Color.White;
                        gerrshown = true;
                        if (mylist != null)
                            {
                            if (mylist.IsGathering == true)
                                {
                                mylist.GatherStop( );
                                MessageBox.Show( "Your free gather limit (3000) has been exceeded, please renew for continued usage! Thank you", "Free gather limit reached!", MessageBoxButtons.OK, MessageBoxIcon.Error );

                                }
                            }
                        
                        }
                    }
                if (iremain <= 0)
                    {
                    if ( mylist != null )
                        {
                         mylist.InviteStop( );
                         sd.iremain = 2000;
                         sd.gremain = 0;
                         ws.UpdateUsage( sd );
                        }
                    if (btnInviteStart.Enabled == true)
                        {
                        //btnInviteStart.Enabled = false;
                        //btnInviteStart.ForeColor = Color.White;
                        if (ierrshown == false)
                            {
                            ierrshown = true;
                            if (mylist != null)
                                {
                                if (mylist.IsInviting == true)
                                    {
                                    mylist.InviteStop( );
                                    MessageBox.Show( "Your free invite limit (1000) has been exceeded, please renew for continued usage! Thank you", "Free invite limit reached!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                                    }
                                }
                            }
                        
                        }
                    }
                if ((gerrshown == true) && (ierrshown == true))
                    Environment.Exit( 0 );
                }
            }

        private void donateToolStripMenuItem_Click ( object sender, EventArgs e )
            {
            //donateToolStripMenuItem.ForeColor = Color.FromArgb( 0x25, 0x25, 0x15 );
            //donateToolStripMenuItem.BackColor = Color.FromArgb( 0xff, 0xa5, 0x00 );

            GGDisk disk = new GGDisk( );

            if (File.Exists( disk.filedonordll ))
                {

                Assembly assembly = Assembly.LoadFrom( disk.filedonordll );
                foreach (Type type in assembly.GetTypes( ))
                    {
                    // Pick up a class
                    if (type.IsClass == true)
                        {
                        // If it does not implement the IBase Interface, skip it
                        if (type.GetInterface( "Gator.IPlugin" ) == null)
                            {
                            continue;
                            }

                        // If however, it does implement the IBase Interface,
                        // create an instance of the object
                        object ibaseObject = Activator.CreateInstance( type );

                        object result = type.InvokeMember( "Display",
                                                         BindingFlags.Default | BindingFlags.InvokeMethod,
                                                         null,
                                                         ibaseObject,
                                                         null );
                        }
                    }
                }
            }

       

        private void bgwCreateList_DoWork ( object sender, DoWorkEventArgs e )
            {
            BackgroundWorker worker = sender as BackgroundWorker;

            worker.ReportProgress( 1 );
            if (chkSkipFirstPage.Checked == true)
                mylist.SkipFirstPage( );
            worker.ReportProgress( 2 );

            bool bin = false;
            bool bon = false;
            bool boff = false;
            if (chkIngame.Checked == true)
                bin = true;
            if (chkOnline.Checked == true)
                bon = true;
            if (chkOffLine.Checked == true)
                boff = true;
            worker.ReportProgress( 3 );
            mylist.GatherTypeAdjust( bin, bon, boff );
            worker.ReportProgress( 4 );

            worker.ReportProgress( 5 );
            if (chkSkipDupes.Checked == true)
                mylist.SkipDupes( );
            worker.ReportProgress( 6 );

            //MakeSkipDupeVisible( );

            worker.ReportProgress( 7 );
            if (chkSkipBlackListed.Checked == true)
                mylist.SkipBlackListed( togroupnew );
            worker.ReportProgress( 8 );


            worker.ReportProgress( 9 );
            if (chkRandomizeList.Checked == true)
                {
                //pbarRandomize.Visible = true;
                RandomizeList( mylist.listtmp );
                }
            worker.ReportProgress( 10 );
            worker.ReportProgress( 100 );
            //pbCGLrandmize.Visible = true;
            }
        private void RandomizeList ( StringBuilder listtmp )
            {

            int iterations = 5000;
          
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
               
                ;
                }
            listtmp.Length = 0;
            listtmp.Append( list1 );
            listtmp.Append( list2 );
           
            
            
            
            }
        private void bgwCreateList_ProgressChanged ( object sender, ProgressChangedEventArgs e )
            {
            if (e.ProgressPercentage == 1)
                {
                if (chkSkipFirstPage.Checked == true)
                    {
                    pbCGLfirstpage.Visible = true;
                    pbCGLfirstpage.Image = Gator25.Properties.Resources.busy1;
                    }
                }
            if (e.ProgressPercentage == 2)
                {
                if (chkSkipFirstPage.Checked == true)
                    {
                    pbCGLfirstpage.Visible = true;
                    pbCGLfirstpage.Image = Gator25.Properties.Resources.gtick;
                    }
                }

            if (e.ProgressPercentage == 4)
                {
                if ( chkIngame.Checked == true )
                    {
                    pbCGLingame.Visible = true;
                    pbCGLingame.Image = Gator25.Properties.Resources.gtick;
                    }
                if (chkOffLine.Checked == true)
                    {
                    pbCGLoffline.Visible = true;
                    pbCGLoffline.Image = Gator25.Properties.Resources.gtick;
                    }
                if (chkOnline.Checked == true)
                    {
                    pbCGLonline.Visible = true;
                    pbCGLonline.Image = Gator25.Properties.Resources.gtick;
                    }

                }
            if (e.ProgressPercentage == 5)
                {

                if (chkSkipDupes.Checked == true)
                    {
                    pbCGLdupes.Visible = true;
                    pbCGLdupes.Image = Gator25.Properties.Resources.busy1;
                    }
                }
            if (e.ProgressPercentage == 6)
                {
                if (chkSkipDupes.Checked == true)
                    {
                    pbCGLdupes.Visible = true;
                    pbCGLdupes.Image = Gator25.Properties.Resources.gtick;
                    }
                }

            if (e.ProgressPercentage == 7)
                {

                if (chkSkipBlackListed.Checked == true)
                    {
                    pbCGLblack.Visible = true;
                    pbCGLblack.Image = Gator25.Properties.Resources.busy1;
                    }
                }
            if (e.ProgressPercentage == 8)
                {
                if (chkSkipBlackListed.Checked == true)
                    {
                    pbCGLblack.Visible = true;
                    pbCGLblack.Image = Gator25.Properties.Resources.gtick;
                    }
                }

            if (e.ProgressPercentage ==9)
                {

                if (chkRandomizeList.Checked == true)
                    {
                    Log( "randomizing the list.." );
                    pbCGLrandmize.Visible = true;
                    pbCGLrandmize.Image = Gator25.Properties.Resources.busy1;
                    }
                }
            if (e.ProgressPercentage == 10)
                {
                if (chkRandomizeList.Checked == true)
                    {
                    Log( "gather list randomizing complete!", Constants.LogMsgType.Success, true );
                    pbCGLrandmize.Visible = true;
                    pbCGLrandmize.Image = Gator25.Properties.Resources.gtick;
                    }
                }

           
           

            }

        private void bgwCreateList_RunWorkerCompleted ( object sender, RunWorkerCompletedEventArgs e )
            {
            mylist.CreateList( );  ///////////////////////
            Log( "the gather list is created and ready!", Constants.LogMsgType.Success, true );
            btnGatherStart.Enabled = true;
            btnGatherStop.Enabled = true;
            btnCreate.Enabled = true;
            pbAjax.Visible = false;
            
            }

        private int nmaxgrps = 400;
        private int nmingrpmem = 5000;

        private void btnAutoFillFromGrps_Click ( object sender, EventArgs e )
            {
            btnAutoFillFromGrps.Enabled = false;
            pbAutoFillBusy.Visible = true;
            try
                {
                nmaxgrps = Convert.ToInt32( txtMaxFromGrps.Text );
                }
            catch (Exception ex)
                {
                nmaxgrps = 400;
                }
            try
                {
                nmingrpmem = Convert.ToInt32( txtMinGrpMemberCount.Text );
                }
            catch (Exception ex)
                {
                nmingrpmem = 5000;
                }
            if ((nmaxgrps > 400) || (nmaxgrps < 1))
                {
                MessageBox.Show( "The maximum group count must be a value between 1 and 400", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
                }

           
            
            GGDisk disk = new GGDisk();

            if (File.Exists( disk.filefromgrps ))
                {
                try
                    {
                    File.Delete( disk.filefromgrps );
                    }
                catch (Exception ex)
                    {
                    ;
                    }
                }
            if (cboFromGroup.Items.Count > 0)
                cboFromGroup.Items.Clear( );
            bgwAutoFill.RunWorkerAsync( );
            }

        private int curgrplistpage = 1;
        private void bgwAutoFill_DoWork ( object sender, DoWorkEventArgs e )
            {
            BackgroundWorker worker = sender as BackgroundWorker;

            int i = curgrplistpage;
            int j = curgrplistpage;
            bool workover = false;
            int ncount = 0;
            GGDisk disk = new GGDisk( );
            disk.ClearFromGroups( );
            while (workover == false)
                {

                //http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=all
                //http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=public&p=3&filter=public&sortby=SortByMembers
                string head = "http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=public&p=";
                string tail = "&filter=public&sortby=SortByMembers";
                string reqpage = head + i.ToString( ) + tail;

                try
                    {
                    MyWeb.MyWeb web = new MyWeb.MyWeb( );
                    string res = string.Empty;
                    while (res == string.Empty)
                        {
                        res = web.GetWebPage( reqpage, 0, 0 );
                        }

                    int beg;
                    beg = res.IndexOf( "<div class=\"groupBlockMedium\">" );
                    if (beg != -1)
                        {
                        res = res.Substring( beg );
                        MyUtils.MyUtils utils = new MyUtils.MyUtils( );
                        ArrayList tok = new ArrayList( );
                        tok.Clear( );
                        tok = utils.GetTokensBetween( res, "<a class=\"linkStandard\" href=\"http://steamcommunity.com/groups/", "/members" );
                        
                        for (int k = 0; k < tok.Count; k++)
                            {
                            disk.AddFromGroup( tok[k].ToString( ) );
                            ncount++;
                            if (ncount > nmaxgrps)
                                {
                                workover = true;
                                curgrplistpage = i+1;
                                break;
                                }
                            if (k == tok.Count - 1)
                                {
                                Group grp = new Group( tok[k].ToString( ), Constants.GroupInitType.groupurl );
                                if (grp.total < nmingrpmem)
                                    {
                                    workover = true;
                                    curgrplistpage = i + 1;
                                    break;
                                    }
                                }
                            }
                        }


                    i++;


                    }
                catch (Exception ex)
                    {
                    ;
                    }
                   

                System.Threading.Thread.Sleep( 1000 );
                //http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=all
                //http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=public&p=3&filter=public&sortby=SortByMembers
                head = "http://steamcommunity.com/actions/GroupList?sortby=SortByMembers&filter=private&p=";
                tail = "&filter=private&sortby=SortByMembers";
                reqpage = head + i.ToString( ) + tail;

                try
                    {
                    MyWeb.MyWeb web = new MyWeb.MyWeb( );
                    string res = string.Empty;
                    while (res == string.Empty)
                        {
                        res = web.GetWebPage( reqpage, 0, 0 );
                        }

                    int beg;
                    beg = res.IndexOf( "<div class=\"groupBlockMedium\">" );
                    if (beg != -1)
                        {
                        res = res.Substring( beg );
                        MyUtils.MyUtils utils = new MyUtils.MyUtils( );
                        ArrayList tok = new ArrayList( );
                        tok.Clear( );
                        tok = utils.GetTokensBetween( res, "<a class=\"linkStandard\" href=\"http://steamcommunity.com/groups/", "/members" );
                        for (int k = 0; k < tok.Count; k++)
                            {
                            disk.AddFromGroup( tok[k].ToString( ) );
                            ncount++;
                            if (ncount > nmaxgrps)
                                {
                                workover = true;
                                curgrplistpage = j+1;
                                break;
                                }
                            if (k == tok.Count - 1)
                                {
                                Group grp = new Group( tok[k].ToString( ), Constants.GroupInitType.groupurl );
                                if (grp.total < nmingrpmem)
                                    {
                                    workover = true;
                                    curgrplistpage = i + 1;
                                    break;
                                    }
                                }
                            
                            }
                        }


                    j++;


                    }
                catch (Exception ex)
                    {
                    ;
                    }
                    
                }
            worker.ReportProgress( 100 );
            }

        private void cboFromGroup_SelectedIndexChanged ( object sender, EventArgs e )
            {

            }

        private void copyrightInfoToolStripMenuItem_Click ( object sender, EventArgs e )
            {
            Gator2.About abt = new Gator2.About( cversion );
            abt.ShowDialog( );
            }

        private void disclaimerToolStripMenuItem1_Click_1 ( object sender, EventArgs e )
            {
            frmDisclamer discl = new frmDisclamer( );
            Constants.DislcaimerResponse resp =  discl.Display( );
            if (resp != Constants.DislcaimerResponse.Agree)
                Environment.Exit( 0 );
            }

        private void checkForUpdatesToolStripMenuItem1_Click_1 ( object sender, EventArgs e )
            {
            upd.HandleUpdate( Constants.UpdateType.Manual, cversion );

            }


        private void MenuHighlight ( object sender )
            {
            ToolStripMenuItem tsmitem = sender as ToolStripMenuItem;

            tsmitem.BackColor = Color.FromArgb( 0xff, 0xa5, 0x00 );
            tsmitem.ForeColor = Color.FromArgb( 0x25, 0x25, 0x25 );
            }

        private void MenuLolight ( object sender )
            {
            ToolStripMenuItem tsmitem = sender as ToolStripMenuItem;
            if (tsmitem.Selected == false)
                {
                tsmitem.BackColor = Color.Transparent;
                tsmitem.ForeColor = Color.FromArgb( 255, 255, 255 );
                }
            }

        private void toolStripMenuItem54_MouseEnter ( object sender, EventArgs e )
            {
            MenuHighlight( sender );
            }

        private void toolStripMenuItem61_MouseEnter ( object sender, EventArgs e )
            {
            MenuHighlight( sender );
            }

        private void donateToolStripMenuItem_MouseEnter ( object sender, EventArgs e )
            {
            MenuHighlight( sender );
            }

        private void pluginsToolStripMenuItem1_MouseEnter ( object sender, EventArgs e )
            {
            MenuHighlight( sender );
            }

        private void toolStripMenuItem54_MouseLeave ( object sender, EventArgs e )
            {
            MenuLolight( sender );
            }

        private void toolStripMenuItem61_MouseLeave ( object sender, EventArgs e )
            {
            MenuLolight( sender );
            }

        private void donateToolStripMenuItem_MouseLeave ( object sender, EventArgs e )
            {
            MenuLolight( sender );
            }

        private void pluginsToolStripMenuItem1_MouseLeave ( object sender, EventArgs e )
            {
            MenuLolight( sender );
            }

        private void toolStripMenuItem97_MouseEnter ( object sender, EventArgs e )
            {
            MenuHighlight( sender );

            }

        private void toolStripMenuItem97_MouseLeave ( object sender, EventArgs e )
            {
            MenuLolight( sender );
            }

        private void toolStripMenuItem97_MouseHover ( object sender, EventArgs e )
            {
            MenuHighlight( sender );
            }

        private void toolStripMenuItem97_MouseMove ( object sender, MouseEventArgs e )
            {
            MenuHighlight( sender );
            }

        private bool mousein = false;
        private void mnuMain_MouseEnter ( object sender, EventArgs e )
            {
            mousein = true;
            }

        private void mnuMain_MouseLeave ( object sender, EventArgs e )
            {
            mousein = false;
            }

        private void btnGatherTabClear_Click ( object sender, EventArgs e )
            {

            txtMinGrpMemberCount.Text = "0";
            txtMaxFromGrps.Text = "100";

            GGDisk disk = new GGDisk( );

            if (File.Exists( disk.filefromgrps ))
                {
                try
                    {
                    File.Delete( disk.filefromgrps );
                    }
                catch (Exception ex)
                    {
                    ;
                    }
                }
            if (cboFromGroup.Items.Count > 0)
                cboFromGroup.Items.Clear( );
            if (chkGatherAllGroups.Checked == true)
                tmrGatherAllGroups.Enabled = false;
            chkGatherAllGroups.Checked = false;

            //if (mylist != null)
            //    {
            //    mylist.GatherStop( );
            //    while (mylist.IsGathering == true)
            //        System.Threading.Thread.Sleep( 1000 );
            //    mylist.ClearGather( );
            //    }
            lblStatusGather.Text = "IDLE";
            lblStatusGather.ForeColor = Color.Red;
            //lblGatherTotal.Text = "0";

            ClearGatherClick( );
            }

        private void bgwAutoFill_RunWorkerCompleted ( object sender, RunWorkerCompletedEventArgs e )
            {
            btnAutoFillFromGrps.Enabled = true;
            pbAutoFillBusy.Visible = false;
            }

        private void button1_Click_1 ( object sender, EventArgs e )
            {

            try
                {
                System.Diagnostics.Process.Start( wbSteamHistory.Url.ToString());
                }
            catch (Exception ex)
                {
                ;
                }
            }

        private void btgGGLogOff_Click ( object sender, EventArgs e )
            {
            Log( GetTime() +  "Logged off from GroupGATOR network" );
            if (auth != null)
                {
                if (auth.status == Constants.SignInStatus.LoggedIn)
                    {
                    SteamLoginClick( );
                    }
                }
            tabControl1.SendToBack( );
            panMain.BringToFront( );
            panMain.Visible = true;
            tabControl1.Visible = false;
            TabDecorate( Constants.TabSituation.DonorLogOff );

            }

       
        }
    }

/////////////////////////////////////////////// PLUGIN INTERFACE
//public MenuItem mnupl = new MenuItem();
//private int mnucnt = 0;
//private void button1_Click(object sender, EventArgs e)
//    {
//    GGDisk disk = new GGDisk();
//    Assembly asm = Assembly.LoadFile(disk.dirplugin + "\\ap.dll");
//    Type type = asm.GetType("AdminPanel.AdminPanel");
//    AdminPanel.AdminPanel apan = (AdminPanel.AdminPanel)asm.CreateInstance(type.Name.ToString());            
//    MethodInfo mi = type.GetMethod("GetPluginName");
//    string s = (string)mi.Invoke(null, null);
//    MessageBox.Show(s);       


//    }


//private void MenuItemClickHandler(object sender, EventArgs e)
//    {
//    ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
//    // Take some action based on the data in clickedItem
//    MessageBox.Show(clickedItem.Name);
//    }

///////////////////////////////////////////// OLD - ANOTHER
//ToolStripMenuItem[] items = new ToolStripMenuItem[5]; // You would obviously calculate this value at runtime
//            for (int i = 0; i < items.Length; i++)
//                {
//                items[i] = new ToolStripMenuItem();
//                items[i].Name = "dynamicItem" + i.ToString();
//                items[i].Tag = "specialDataHere";
//                items[i].Text = "Visible Menu Text Here";
//                items[i].Click += new EventHandler(MenuItemClickHandler);
//                }
//            pluginsToolStripMenuItem.DropDownItems.AddRange(items);