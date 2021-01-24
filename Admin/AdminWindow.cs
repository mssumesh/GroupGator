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
    public partial class AdminWindow : Form
        {
        private string capt;
        Price price;

        public AdminWindow(string caption)
            {
            capt = caption;
            price = null;
            InitializeComponent();
            //this.Size = new Size(923, 437);
            }

        //648, 437    //////////
        //1036, 437   ////////////

        private void AdminWindow_Load(object sender, EventArgs e)
            {
            CheckForIllegalCrossThreadCalls = false;
            this.Text = capt;
                this.Size = new Size(612, 437);
            panAdminLI.Visible = false;
            panAdminLO.Visible = true;
            //real
            //this.Size = new Size(612, 437);
            //new
            //this.Size = new Size(923, 437);
            int x = 0;
            int y = 0;
            x = Screen.PrimaryScreen.WorkingArea.Width - 612;
            y = Screen.PrimaryScreen.WorkingArea.Height - 437;
            this.Left = x/2;
            this.Top = y/2;
            
            }


        private void AdminLoginClick()
            {            
            WebService serv = new WebService();
            //Log(GetTime() + "Attempting to log in as admin.. ", Constants.LogMsgType.Success, false);
            Constants.ServerResponse res = serv.AdminCheck(txtAdminName.Text, txtAdminPass.Text);
            if (res == Constants.ServerResponse.AcheckSuccess)
                {
                panAdminLI.Visible = true;
                panAdminLO.Visible = false;
                //Log("Success!", Constants.LogMsgType.Success, true);
                //tmrGGStats.Enabled = true;
                if ( price == null )
                    price = new Price(txtAdminName.Text, txtAdminPass.Text);
                UpdateAdminStats();
                if (cboPriceEdit.Items.Count > 0)
                    cboPriceEdit.SelectedIndex = 0;
                }
            else
                {
                panAdminLI.Visible = false;
                panAdminLO.Visible = true;
                MessageBox.Show("Your attempt to login as administrator failed because your account is not valid", "Login Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Log("Failed! Your administrator account is not valid", Constants.LogMsgType.Error, true);
                }
            }

        private void label53_Click(object sender, EventArgs e)
            {

            }

        private void btnAdminLogin_Click_1(object sender, EventArgs e)
            {
            AdminLoginClick();
            }

        private void btnAdminGetUserTime_Click(object sender, EventArgs e)
            {
            GetTimeRemainingClick(txtAcGetTime.Text);
            }
        private void GetTimeRemainingClick(string email)
            {
            label57.Visible = true;
            lblTimeLeft.Visible = true;
            WebService serv = new WebService();
            //Log(GetTime() + "checking remaining time for account - " + email + " ", Constants.LogMsgType.Success, false);
            string tremain = string.Empty;
            Constants.ServerResponse res = serv.GetTimeRemaining(txtAcGetTime.Text, ref tremain);
            if (res == Constants.ServerResponse.Success)
                {
                //Log("Success!", Constants.LogMsgType.Success, true);
                lblTimeLeft.Text = FormatTime(tremain, Constants.FormatTimeType.Dot);
                }
            else if (res == Constants.ServerResponse.Failed)
                {
                MessageBox.Show("Checking remaining time for account failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Log("Failed!", Constants.LogMsgType.Error, true);
                }
            }
        private string FormatTime(string time, Constants.FormatTimeType type)
            {
            double dbltime = 0.0;
            try
                {
                dbltime = Convert.ToDouble(time);
                }
            catch (Exception ex)
                {
                //Log(GetTime() + "Conversion exception in FormatTime " + ex.Message, Constants.LogMsgType.Error, true);
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
        public string GetTime()
            {
            DateTime dt = DateTime.Now;
            string ret = "[" + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "] ";
            return ret;
            }

        private void btnAdminSetTime_Click(object sender, EventArgs e)
            {
            SetTimeClick();
            }

        private void SetTimeClick()
            {
            WebService serv = new WebService();
            //Log(GetTime() + "Attempting to set " + txtDaysToSet.Text + "days to " + txtAcSetTime.Text, Constants.LogMsgType.Success, false);
            Constants.ServerResponse res = serv.SetTime(txtAcSetTime.Text, txtAdminName.Text, txtAdminPass.Text, txtDaysToSet.Text);
            if (res == Constants.ServerResponse.UnlockSuccess)
                {
                //Log("Success!", Constants.LogMsgType.Success, true);
                MessageBox.Show("Account (" + txtAcSetTime.Text + ") set with " + txtDaysToSet.Text + " days", "Success", MessageBoxButtons.OK);
                }
            else if (res == Constants.ServerResponse.UnlockFailNotAdmin)
                {
                //Log("Failed! You must be logged in as administrator to set days", Constants.LogMsgType.Error, true);
                MessageBox.Show("Failed! You must have administrator privileges to set days", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else if (res == Constants.ServerResponse.UnlockFailWrongAccount)
                {
                //Log("Failed! The provided account does not exist", Constants.LogMsgType.Error, true);
                MessageBox.Show("Failed! The provided account does not exist", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void btnAdminAddDays_Click(object sender, EventArgs e)
            {
            AddTimeClick();
            }
        private void AddTimeClick()
            {
            WebService serv = new WebService();
            //Log(GetTime() + "Attempting to add " + txtDaysToAdd.Text + "days to " + txtAcAddTime.Text, Constants.LogMsgType.Success, false);
            Constants.ServerResponse res = serv.AddTime(txtAcAddTime.Text, txtAdminName.Text, txtAdminPass.Text, txtDaysToAdd.Text);
            if (res == Constants.ServerResponse.UnlockSuccess)
                {
                //Log("Success!", Constants.LogMsgType.Success, true);
                MessageBox.Show("Account (" + txtAcAddTime.Text + ") added with " + txtDaysToAdd.Text + " days", "Success", MessageBoxButtons.OK);
                }
            else if (res == Constants.ServerResponse.UnlockFailNotAdmin)
                {
                //Log("Failed! You must be logged in as administrator to add days", Constants.LogMsgType.Error, true);
                MessageBox.Show("Failed! You must have administrator privileges to add days", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else if (res == Constants.ServerResponse.UnlockFailWrongAccount)
                {
                //Log("Failed! The provided account does not exist", Constants.LogMsgType.Error, true);
                MessageBox.Show("Failed! The provided account does not exist", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void btnAdminUnlock_Click(object sender, EventArgs e)
            {
            UnlockAccountClick();
            }
        private void UnlockAccountClick()
            {
            WebService serv = new WebService();
            //Log(GetTime() + "Attempting to unlock account - " + txtAcUnlock.Text + " ", Constants.LogMsgType.Success, false);
            Constants.ServerResponse res = serv.Unlock(txtAcUnlock.Text, txtAdminName.Text, txtAdminPass.Text);
            if (res == Constants.ServerResponse.UnlockSuccess)
                {
                //Log("Success!", Constants.LogMsgType.Success, true);
                MessageBox.Show("Account (" + txtAcUnlock.Text + ") unlocked successfully", "Success", MessageBoxButtons.OK);
                //if (txtAcUnlock.Text == profile.gatoremail)
                //    {
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
                //    Environment.Exit(0);
                //    }
                }
            else if (res == Constants.ServerResponse.UnlockFailNotAdmin)
                {
                //Log("Failed! You must be logged in as administrator to unlock", Constants.LogMsgType.Error, true);
                MessageBox.Show("Failed! You must have administrator privileges to unlock", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else if (res == Constants.ServerResponse.UnlockFailWrongAccount)
                {
                //Log("Failed! The provided account does not exist", Constants.LogMsgType.Error, true);
                MessageBox.Show("Failed! The provided account does not exist", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void btnPriceEdit_Click(object sender, EventArgs e)
            {
            this.Size = new Size(923, 437);
            int x = ( Screen.PrimaryScreen.WorkingArea.Width - 923 ) / 2;
            int y = ( Screen.PrimaryScreen.WorkingArea.Height - 437 ) / 2;
            this.Left = x;
            this.Top = y;
            LoadForEdit();
            }

        private void button2_Click(object sender, EventArgs e)
            {
            this.Size = new Size(612, 437);
            int x = Screen.PrimaryScreen.WorkingArea.Width - 612;
            int y = Screen.PrimaryScreen.WorkingArea.Height - 437;
            this.Left = x / 2;
            this.Top = y / 2;
            }

        private void btnAdminLogin_Click(object sender, EventArgs e)
            {
            AdminLoginClick();
            }

        private void timer1_Tick(object sender, EventArgs e)
            {
            ThreadStart ts = new ThreadStart(UpdateAdminStats);
            Thread t = new Thread(ts);
            t.Start();
            //UpdateAdminStats();
            }

        static string paid = string.Empty;
        static string free = string.Empty;
        static string totalinv = string.Empty;
        private void UpdateAdminStats()
            {
            WebService ws = new WebService();
            ws.GetAdminStats(ref paid, ref free, ref totalinv);
            lblPaidOnline.Text = paid;
            lblFreeOnline.Text = free;
            lblGlobalInvites.Text = totalinv;
            ///////////downlaod and set payment values as well
            if ( price == null )
                price = new Price(txtAdminName.Text, txtAdminPass.Text);
            if (price != null)
                {
                price.GetAll();
                if (price.Count > 0)
                    {
                    if( cboPriceEdit.Items.Count > 0 )
                        cboPriceEdit.Items.Clear();
                    for (int i = 0; i < price.Count; i++)
                        {
                        if ( price.pricelist[i].savings == "0" )
                            cboPriceEdit.Items.Add( "$" + price.pricelist[i].amount + " OR " + price.pricelist[i].gp + " Points = " + price.pricelist[i].days + " Days (" + price.pricelist[i].savings + "Trial Price!)" );
                        else
                            cboPriceEdit.Items.Add("$" + price.pricelist[i].amount + " OR " + price.pricelist[i].gp + " Points = " + price.pricelist[i].days + " Days (" + price.pricelist[i].savings + "% Savings!)");
                        }
                    }
                
                }
            }

        private void btnBan_Click(object sender, EventArgs e)
            {
            WebService ws = new WebService();
            bool ret =  ws.Ban(txtAdminName.Text, txtAdminPass.Text, txtBan.Text);
            if (ret == true)
                MessageBox.Show("User " + txtBan.Text + " banned!");
            else
                MessageBox.Show("Ban operation failed, please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        private void btnUnban_Click(object sender, EventArgs e)
            {
            WebService ws = new WebService();
            bool ret =  ws.UnBan(txtAdminName.Text, txtAdminPass.Text, txtUnban.Text);
            if (ret == true)
                MessageBox.Show("User " + txtUnban.Text + " un-banned!");
            else
                MessageBox.Show("UnBan operation failed, please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        private void btnSet_Click(object sender, EventArgs e)
            {
            ////UpdatePrice();
            if (price != null)
                {
                if (txtSetPayPal.Text == string.Empty)
                    {
                    MessageBox.Show("Paypal Code can not be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }
                Gator.Price.PRICE prc = new Gator.Price.PRICE();
                prc.amount = txtSetPrice.Text;
                prc.days = txtSetDays.Text;
                prc.gp = txtSetGP.Text;
                prc.paypal = txtSetPayPal.Text;
                if (optYes.Checked == true)
                    prc.promo = "1";
                else
                    prc.promo = "0";
                prc.savings = txtSetSavings.Text;
                if (price.Update(txtSetPayPal.Text, prc) == true)
                    MessageBox.Show("price succesfully updated!");
                else
                    MessageBox.Show("Attempt to update the price failed! Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
            }

        private void LoadForEdit()
            {
            if ( price != null )
                {
                
                string buff = string.Empty;
                buff = cboPriceEdit.Text;
                if (buff != string.Empty)
                    {
                    //$100.00 OR 1000 Points = 400 Days (75% Savings!)
                    int beg, end;
                    beg = end = -1;
                    beg = 1;
                    end = buff.IndexOf(" ");
                    txtSetPrice.Text = buff.Substring(beg, end - beg);
                    beg = end + " OR ".Length;
                    end = buff.IndexOf(" ", beg);
                    txtSetGP.Text = buff.Substring(beg, end - beg);
                    beg = end + " Points = ".Length;
                    end = buff.IndexOf(" ", beg);
                    txtSetDays.Text = buff.Substring(beg, end - beg);
                    beg = end + " Days (".Length;
                    end = buff.IndexOf("%", beg);
                    txtSetSavings.Text = buff.Substring(beg, end - beg);
                    string tmp = string.Empty;
                    GGDisk disk = new GGDisk();
                    tmp = disk.Read(price.pricelocal);
                    if (tmp == string.Empty)
                        {
                        price.GetAll();
                        tmp = disk.Read( price.pricelocal );
                        }
                    if (tmp != string.Empty)
                        {
                        //"{PRICETAG}{AMOUNT}{/AMOUNT}){GP}{/GP}{PROMO}{/PROMO}{DAYS}{/DAYS}{PAYPAL}{/PAYPAL}{SAVE}{/SAVE}{/PRICETAG}
                        MyUtils.MyUtils utils = new MyUtils.MyUtils();
                        //string ppal = utils.GetStrBetween(tmp, "{/PROMO}{DAYS}" + txtSetDays.Text + "{/DAYS}{PAYPAL}", "{/PAYPAL}");
                        //txtSetPayPal.Text = price.GetPaypalCode( txtSetDays.Text );//, txtSetDays.Text);
                        txtSetPayPal.Text = utils.GetStrBetween( tmp, "{DAYS}" + txtSetDays.Text + "{/DAYS}{PAYPAL}", "{/PAYPAL}" );
                        string promo = utils.GetStrBetween(tmp, "{PROMO}", "{/PROMO}{DAYS}" + txtSetDays.Text + "{/DAYS}{PAYPAL}");
                        if (promo == "0 ")
                            optNo.Checked = true;
                        else
                            optYes.Checked = true;
                        }
                    string txt1 = "$" + txtSetPrice.Text + " = " + txtSetDays.Text + " Days (" + txtSetSavings.Text + "% Savings!)";
                    if ( txtSetSavings.Text == "0" )
                        txt1 = "$" + txtSetPrice.Text + " = " + txtSetDays.Text + " Days (" + txtSetSavings.Text + "Trail Price)";
                    string txt2 = txtSetGP.Text + "GP = " + txtSetDays.Text + " Days";
                    txtPaypalEdit.Text = txt1;
                    txtGPEdit.Text = txt2;
                    //lblPayment1.Text = txt1;
                    //lblPayment2.Text = txt2;

                    }
                }
            
            
            }


        private void btnNew_Click(object sender, EventArgs e)
            {
            //AddPrice();
            if (price != null)
                {
                if (txtSetPayPal.Text == string.Empty)
                    {
                    MessageBox.Show("Paypal Code can not be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }
                Gator.Price.PRICE prc = new Gator.Price.PRICE();
                prc.amount = txtSetPrice.Text;
                prc.days = txtSetDays.Text;
                prc.gp = txtSetGP.Text;
                prc.paypal = txtSetPayPal.Text;
                if (optYes.Checked == true)
                    prc.promo = "1";
                else
                    prc.promo = "0";
                prc.savings = txtSetSavings.Text;
                if (price.Add(prc) == true)
                    MessageBox.Show("New price succesfully added!");
                else
                    MessageBox.Show("Attempt to add new price failed! Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void btnDelete_Click(object sender, EventArgs e)
            {
            if (price != null)
                {
                if (txtSetPayPal.Text == string.Empty)
                    {
                    MessageBox.Show("Paypal Code can not be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }
                if (price.Delete(txtSetPayPal.Text) == true)
                    MessageBox.Show("price succesfully deleted!");
                else
                    MessageBox.Show("Attempt to delete the price failed! Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void cboPriceEdit_SelectedIndexChanged(object sender, EventArgs e)
            {
            LoadForEdit();
            }

        private void txtSetPrice_TextChanged(object sender, EventArgs e)
            {
            string txt1 = "$" + txtSetPrice.Text + " = " + txtSetDays.Text + " Days (" + txtSetSavings.Text + "% Savings!) and";
            string txt2 = txtSetGP.Text + "GP = " + txtSetDays.Text + " Days";
            txtPaypalEdit.Text = txt1;
            txtGPEdit.Text = txt2;
            }

        private void txtSetGP_TextChanged(object sender, EventArgs e)
            {
            string txt1 = "$" + txtSetPrice.Text + " = " + txtSetDays.Text + " Days (" + txtSetSavings.Text + "% Savings!) and";
            string txt2 = txtSetGP.Text + "GP = " + txtSetDays.Text + " Days";
            txtPaypalEdit.Text = txt1;
            txtGPEdit.Text = txt2;
            }

        private void txtSetDays_TextChanged(object sender, EventArgs e)
            {
            string txt1 = "$" + txtSetPrice.Text + " = " + txtSetDays.Text + " Days (" + txtSetSavings.Text + "% Savings!) and";
            string txt2 = txtSetGP.Text + "GP = " + txtSetDays.Text + " Days";
            txtPaypalEdit.Text = txt1;
            txtGPEdit.Text = txt2;
            }

        private void txtSetSavings_TextChanged(object sender, EventArgs e)
            {
            string txt1 = "$" + txtSetPrice.Text + " = " + txtSetDays.Text + " Days (" + txtSetSavings.Text + "% Savings!) and";
            string txt2 = txtSetGP.Text + "GP = " + txtSetDays.Text + " Days";
            txtPaypalEdit.Text = txt1;
            txtGPEdit.Text = txt2;
            }

       
        }
    }