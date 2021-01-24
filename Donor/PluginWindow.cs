using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Gator
    {
    public partial class DonorWindow : Form
        {
        private string capt;
        public DonorWindow( string caption)
            {
            InitializeComponent();
            capt = caption;
            }
        public string usr;
        public string pass;
        Price price;
        public Constants.SoftLicense lic;
        Gator25.AdAPI adapi;
   
        private void PluginWindow_Load(object sender, EventArgs e)
            {
            this.Text = capt;
            //label1.Text = capt;

            //"<GGID>" + txtDBName.Text + "</GGID><GGPASS>" + txtDBPass.Text + "</GGPASS>";
            //towrite = towrite + "<LIC>PAID</LIC>";
            //disk.Write(towrite, disk.filedbdetails);

            GGDisk disk = new GGDisk();
            MyUtils.MyUtils utils = new MyUtils.MyUtils();
            string buff = disk.Read ( disk.filedbdetails );
            usr = utils.GetStrBetween ( buff, "<GGID>", "</GGID>");
            pass = utils.GetStrBetween ( buff, "<GGPASS>", "</GGPASS>");
            if (buff.Contains("<LIC>DEMO</LIC>"))
                {
                lic = Constants.SoftLicense.Demo;
                usr = "FREEUSER@groupgatorcommunity.net";
                }
            else
                {

                FingerPrint fp = new FingerPrint();
                string mid = fp.GetComuputerID();

                WebService serv = new WebService();
                WebService.ServerData sdata = new WebService.ServerData();
                sdata.email = usr;
                sdata.pass = pass;
                sdata.hid = mid;
                sdata.lockcomp = "false";
                Constants.ServerResponse resp = serv.Login(sdata);
                lic = Constants.SoftLicense.Demo;

                if (resp == Constants.ServerResponse.Valid)
                    lic = Constants.SoftLicense.Paid;
                else
                    lic = Constants.SoftLicense.Demo;
                }

            if (lic == Constants.SoftLicense.Demo)
                {
                lblUserMail1.Text = "FREEUSER@groupgatorcommunity.net";
                }
            else
                lblUserMail1.Text = usr;


            ThreadStart tsSK = new ThreadStart( SideKick );
            Thread tSK = new Thread( tsSK );
            tSK.IsBackground = true;
            tSK.Start( );
            
            }

        private void SideKick ( )
            {
            price = new Price( usr, pass );
            adapi = new Gator25.AdAPI( usr );

            DonorTimerTick( );
            btnDonate11.Enabled = true;
            btnGP11.Enabled = true;
            btnSurvey1.Enabled = true;
            btnGoogleDonate1.Enabled = true;
            }
       

        private void btnDonorLogOff1_Click(object sender, EventArgs e)
            {
            //if (lic == Constants.SoftLicense.Demo)
            //    Log(GetTime() + "FREEUSER Logged out of GroupGATOR", Constants.LogMsgType.Success, true);
            //else
            //    Log(GetTime() + profile.gatoremail + " Logged out of GroupGATOR", Constants.LogMsgType.Success, true);
            //DonorLogOffClick();
            this.Close();
            }

        private void btnUnlock1_Click(object sender, EventArgs e)
            {
            UnlockNormalClick();
            }
        //public void DonorLogOffClick()
        //    {
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
        //        {
        //        if (mylist.IsInviting == true)
        //            mylist.InviteStop();
        //        if (mylist.IsGathering == true)
        //            mylist.GatherStop();
        //        System.Threading.Thread.Sleep(2000);
        //        UnSavedListCheck();
        //        }
        //    profile.SaveConfig();

        //    }
        //public void DonorLoggedInMakeup(string loggeduser)
        //    {
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
        //        {
        //        dbltime = Convert.ToDouble(trem);
        //        }
        //    catch (Exception ex)
        //        {
        //        Log(GetTime() + "Conversion exception in LoggedInMakeup " + ex.Message, Constants.LogMsgType.Error, true);
        //        }
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


        //    }
        ////private void StartFreeVersionClick()
        ////    {
        ////    btnStartFreeVersion.Enabled = false;
        ////    panAdminLI.Visible = false;
        ////    panAdminLO.Visible = false;
        ////    FingerPrint fp = new FingerPrint();
        ////    string mid = fp.GetComuputerID();
        ////    lic = Constants.SoftLicense.Demo;
        ////    WebService serv = new WebService();
        ////    serv.mid = mid;
        ////    serv.Enter(Constants.SoftLicense.Demo);
        ////    TabDecorate(Constants.TabSituation.FreeLogin);
        ////    tabControl1.Visible = true;
        ////    panMain.Visible = false;

        ////    lblRemDays.Visible = false;
        ////    lblRemHours.Visible = false;
        ////    lblRemMinutes.Visible = false;
        ////    lblRemSeconds.Visible = false;
        ////    label70.Visible = false;
        ////    label71.Visible = false;
        ////    label72.Visible = false;
        ////    label73.Visible = false;

        ////    WebService.ServerData sd = serv.CheckUsage();
        ////    lblUserMail.Text = "FREEUSER";
        ////    Log("Thank you for trying free version of GroupGATOR " + cversion, Constants.LogMsgType.Error, true);
        ////    Log("You have " + sd.iremain.ToString() + " invites and " + sd.gremain.ToString() + " gathers remaining", Constants.LogMsgType.Error, true);

        ////    lblFreeRem.Visible = true;
        ////    lblFreeRem.Text = sd.iremain.ToString() + " INVITES and " + sd.gremain.ToString() + " GATHERS";

        ////    btnStartFreeVersion.Enabled = true;
        ////    }
        private void UnlockNormalClick()
            {

            DialogResult dlgres = MessageBox.Show( "You will lose a day from your subscription period as a fee, everytime you unlock the software license. Are you sure you want to unlock?", "Confirmation Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            if (dlgres == System.Windows.Forms.DialogResult.Yes)
                {
                WebService serv = new WebService( );

                string timerem = "0";
                
                Constants.ServerResponse res = serv.GetTimeRemaining( usr, ref timerem );
                double dbltime = 0.0;
                try
                    {
                    dbltime = Convert.ToDouble( timerem );
                    }
                catch (Exception ex)
                    {
                    dbltime = 0.0;
                    }
                TimeSpan ts = TimeSpan.FromSeconds( dbltime );
                int cur = ts.Days;
                double dblcur = Convert.ToDouble( cur );

                if (dblcur < 1.0)
                    {
                    MessageBox.Show( "You do not have sufficient days left in your account perform the unlocking. Please buy more time for continuing unlocking", "Insufficient Time", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                    }
                //Log(GetTime() + "Attempting to unlock account - " + usr + " ", Constants.LogMsgType.Success, false);
                res = serv.UnlockNormal( usr, pass );
                if (res == Constants.ServerResponse.UnlockSuccess)
                    {
                    //Log("Success!", Constants.LogMsgType.Success, true);
                    //MessageBox.Show("Account (" + profile.gatoremail + ") unlocked successfully", "Success", MessageBoxButtons.OK);
                    MessageBox.Show( "The account (" + usr + ") has been unlocked. Please exit and restart GroupGATOR on the machine of your choice", "Unlocked!", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    //DonorLogOffClick();
                    Environment.Exit( 0 );
                    }
                else if (res == Constants.ServerResponse.UnlockFailWrongAccount)
                    {
                    //Log("Failed! The provided account is not valid for unlocking", Constants.LogMsgType.Error, true);
                    MessageBox.Show( "Failed! The provided account is not valid for unlocking", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    }
                }
            }
        private void DonateClicked(Constants.DonateType dtype, bool ispaypal)
            {
            
            Gator25.PayPalWindow paypal = new Gator25.PayPalWindow();
            if (ispaypal == true)
                {
                
                string pcode = string.Empty;
                if ( cboPaypalRates1.SelectedIndex != -1 )
                    {
                    string tmp = cboPaypalRates1.SelectedItem.ToString();
                    MyUtils.MyUtils utils = new MyUtils.MyUtils();
                   
                    //$100.00 OR 1000 Points = 400 Days (75% Savings!)  

                    string amnt = utils.GetStrBetween(tmp, "$", " ");
                    string days = utils.GetStrBetween(tmp, " = ", " ");
                    pcode = price.GetPaypalCode ( amnt, days );
                    paypal.DisplayPaypal(pcode);
                    }
                }
            else
                paypal.DisplayGoogle(dtype);

            //string reqfile = string.Empty;
            //GGDisk disk = new GGDisk();
            //switch (dtype)
            //    {
            //    case Constants.DonateType.Donate1:
            //        reqfile = disk.filedonator1;
            //        break;
            //    case Constants.DonateType.Donate2:
            //        reqfile = disk.filedonator2;
            //        break;
            //    case Constants.DonateType.Donate3:
            //        reqfile = disk.filedonator3;
            //        break;
            //    case Constants.DonateType.Donate4:
            //        reqfile = disk.filedonator4;
            //        break;
            //    case Constants.DonateType.Donate5:
            //        reqfile = disk.filedonator5;
            //        break;
            //    case Constants.DonateType.Donate6:
            //        reqfile = disk.filedonator6;
            //        break;
            //    }
            //if (File.Exists(reqfile))
            //    {
            //    try
            //        {
            //        System.Diagnostics.Process.Start(reqfile);
            //        }
            //    catch (Exception ex)
            //        {
            //        Log("Exception in donate : " + ex.Message.ToString(), Constants.LogMsgType.Error, true);
            //        }
            //    }

            }
        string dremain;
        string hremain;
        string mremain;
        string sremain;
        private void DonorGiftClick()
            {
            DialogResult dlgres = MessageBox.Show("This will deduct " + txtDonDaysToGift1.Text + "days from your subscription and will add the amount to " + txtDonAcToGiftTo1.Text + ". An additional 3 days will be deducted from your subscription as the fee for gifting. Are you sure you want to continue?", "Confirmation Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            WebService serv = new WebService( );
            string timerem = "0";
            double reqd = 0.0;
            try
                {
                reqd = Convert.ToDouble( txtDonDaysToGift1.Text );
                }
            catch (Exception ex)
                {
                reqd = 0;
                }

            Constants.ServerResponse res = serv.GetTimeRemaining( usr, ref timerem );
            double dbltime = 0.0;
            try
                {
                dbltime = Convert.ToDouble( timerem );
                }
            catch (Exception ex)
                {
                dbltime = 0.0;
                }
            TimeSpan ts = TimeSpan.FromSeconds ( dbltime );
            int cur = ts.Days;
            double dblcur = Convert.ToDouble ( cur );
   
            if ( dblcur < reqd )
                {
                MessageBox.Show ( "You do not have sufficient days left in your account to send this gift. Try reducing the number of days to gift, or buy more time for continuing with this gift", "Insufficient Time", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
                }
            
            if (dlgres == DialogResult.Yes)
                {
                
                //Log(GetTime() + "Attempting to gift " + txtDonDaysToGift.Text + "days to " + txtDonAcToGiftTo.Text + "...", Constants.LogMsgType.Success, false);
                res = serv.GiftDays(txtDonAcToGiftTo1.Text, usr, pass, txtDonDaysToGift1.Text);
                if (res == Constants.ServerResponse.GiftSuccess)
                    {
                    //Log("Success!", Constants.LogMsgType.Success, true);
                    MessageBox.Show("Account (" + txtDonAcToGiftTo1.Text + ") gifted with " + txtDonDaysToGift1.Text + " days", "Success", MessageBoxButtons.OK);
                   

                    }
                else if (res == Constants.ServerResponse.GiftFailNoDonor)
                    {
                    //Log("Failed! You are not a valid donor to gift days", Constants.LogMsgType.Error, true);
                    MessageBox.Show("Failed! You are not a valid donor to gift days", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                else if (res == Constants.ServerResponse.GiftFailNoReceiver)
                    {
                    //Log("Failed! The provided receiver account does not exist", Constants.LogMsgType.Error, true);
                    MessageBox.Show("Failed! The provided receiver account does not exist", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                else if (res == Constants.ServerResponse.GiftFailNotEnough)
                    {
                    //Log("Failed! You dont have sufficient time left in your account to gift", Constants.LogMsgType.Error, true);
                    MessageBox.Show("Failed! You dont have sufficient time left in your account to gift", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        private void btnDonate11_Click(object sender, EventArgs e)
            {
            switch (cboPaypalRates1.SelectedIndex)
                {
                case 0:
                    DonateClicked(Constants.DonateType.Donate1, true);
                    break;
                case 1:
                    DonateClicked(Constants.DonateType.Donate2, true);
                    break;
                case 2:
                    DonateClicked(Constants.DonateType.Donate3, true);
                    break;
                case 3:
                    DonateClicked(Constants.DonateType.Donate4, true);
                    break;
                case 4:
                    DonateClicked(Constants.DonateType.Donate5, true);
                    break;
                case 5:
                    DonateClicked(Constants.DonateType.Donate6, true);
                    break;
                }
            
            }

        private void btnGoogleDonate1_Click(object sender, EventArgs e)
            {
            switch (cboGoogleRates1.SelectedIndex)
                {
                case 0:
                    DonateClicked(Constants.DonateType.Donate1, false);
                    break;
                case 1:
                    DonateClicked(Constants.DonateType.Donate2, false);
                    break;
                case 2:
                    DonateClicked(Constants.DonateType.Donate3, false);
                    break;
                case 3:
                    DonateClicked(Constants.DonateType.Donate4, false);
                    break;
                case 4:
                    DonateClicked(Constants.DonateType.Donate5, false);
                    break;
                case 5:
                    DonateClicked(Constants.DonateType.Donate6, false);
                    break;
                }
            }

        private void btnGift1_Click(object sender, EventArgs e)
            {
            DonorGiftClick();
            }

        private void btnSurvey1_Click(object sender, EventArgs e)
            {
            Gator25.frmAds adDlg = new Gator25.frmAds(usr);
            adDlg.ShowDialog();
            }


        private void tmrDonorSettings_Tick(object sender, EventArgs e)
            {

            DonorTimerTick();
            }
        private void DonorTimerTick()
            {
            price.GetAll();
            if (price.Count > 0)
                {
                if (cboPaypalRates1.Items.Count > 0)
                    cboPaypalRates1.Items.Clear();
                for (int i = 0; i < price.Count; i++)
                    {
                    if ( price.pricelist[i].savings == "0" )
                        cboPaypalRates1.Items.Add( "$" + price.pricelist[i].amount + " OR " + price.pricelist[i].gp + " Points = " + price.pricelist[i].days + " Days (" + price.pricelist[i].savings + " Trial Price" );
                    else
                        cboPaypalRates1.Items.Add("$" + price.pricelist[i].amount + " OR " + price.pricelist[i].gp + " Points = " + price.pricelist[i].days + " Days (" + price.pricelist[i].savings + "% Savings!");
                    }
                }

            ///////format remaining time
            WebService serv = new WebService();    
            string timerem = string.Empty;
            Constants.ServerResponse tremres;            
            tremres = serv.GetTimeRemaining(usr, ref timerem);
            double dbltime = 0.0;
            if (timerem != string.Empty)
                {
                try
                    {
                    dbltime = Convert.ToDouble(timerem);
                    }
                catch (Exception ex)
                    {
                    ;
                    }
                TimeSpan t = TimeSpan.FromSeconds(dbltime);

                rtfRemaining.SelectionStart = 0;
                rtfRemaining.SelectionLength = 0;
                rtfRemaining.SelectionColor = Color.Red;
                rtfRemaining.SelectedText = t.Days.ToString();
                rtfRemaining.SelectionStart = rtfRemaining.Text.ToCharArray().Length;
                rtfRemaining.SelectionLength = 0;
                rtfRemaining.SelectionColor = Color.Black;
                rtfRemaining.SelectedText = " days ";

                rtfRemaining.SelectionStart = rtfRemaining.Text.ToCharArray().Length; ;
                rtfRemaining.SelectionLength = 0;
                rtfRemaining.SelectionColor = Color.Red;
                rtfRemaining.SelectedText = t.Hours.ToString();
                rtfRemaining.SelectionStart = rtfRemaining.Text.ToCharArray().Length;
                rtfRemaining.SelectionLength = 0;
                rtfRemaining.SelectionColor = Color.Black;
                rtfRemaining.SelectedText = " hours ";

                rtfRemaining.SelectionStart = rtfRemaining.Text.ToCharArray().Length; ;
                rtfRemaining.SelectionLength = 0;
                rtfRemaining.SelectionColor = Color.Red;
                rtfRemaining.SelectedText = t.Minutes.ToString();
                rtfRemaining.SelectionStart = rtfRemaining.Text.ToCharArray().Length;
                rtfRemaining.SelectionLength = 0;
                rtfRemaining.SelectionColor = Color.Black;
                rtfRemaining.SelectedText = " minutes ";

                rtfRemaining.SelectionStart = rtfRemaining.Text.ToCharArray().Length; ;
                rtfRemaining.SelectionLength = 0;
                rtfRemaining.SelectionColor = Color.Red;
                rtfRemaining.SelectedText = t.Seconds.ToString();
                rtfRemaining.SelectionStart = rtfRemaining.Text.ToCharArray().Length;
                rtfRemaining.SelectionLength = 0;
                rtfRemaining.SelectionColor = Color.Black;
                rtfRemaining.SelectedText = " seconds ";

                int rev = adapi.GetUserPoints();
                int used = serv.GetGatorPointsUsed( usr );
                rev = rev - used;
                if (rev < 0)
                    rev = 0;
                lblGatorPoints.Text = rev.ToString();
                }
            }
        
        private void btnGP11_Click(object sender, EventArgs e)
            {
            if (cboGPRates1.Text == string.Empty)
                return;
            if (usr == "FREEUSER@groupgatorcommunity.net")
                return;

            string tmp = cboGPRates1.Text;
            int tag = tmp.IndexOf( " " );
            if (tag != -1)
                {
                string price = tmp.Substring( 0, tag );
                int cash = 0;
                try
                    {
                    cash = Convert.ToInt32( price );
                    }
                catch (Exception ex)
                    {
                    cash = 0;
                    }
                WebService ws = new WebService( );
                int rev = adapi.GetUserBalance( );
                Constants.ServerResponse sresp = ws.BuyWithGP (usr, cash.ToString( ), rev );
                if (sresp == Constants.ServerResponse.GPBought)
                    MessageBox.Show( "Your purchase was successful!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information );
                else if (sresp == Constants.ServerResponse.GPNoAccount)
                    MessageBox.Show( "Your purchase failed because no existing account found with this email", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                else if ( sresp == Constants.ServerResponse.GPNotEnough )
                    MessageBox.Show( "Your purchase failed because the current user does not have enough credit left to make the purchase", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
            }

        private void cboGPRates1_SelectedIndexChanged ( object sender, EventArgs e )
            {

            }
        }
    }