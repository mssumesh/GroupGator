using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gator25
    {
    public partial class frmAds : Form
        {
        private string ggemail;
        Gator25.AdAPI ads;
        Gator.GGDisk disk;
        MyWeb.MyWeb web;
        MyUtils.MyUtils utils;
        public frmAds( string ggmail)
            {
            ggemail = ggmail;
            InitializeComponent();
            }

        private void button1_Click(object sender, EventArgs e)
            {
            this.Close();
            }

       

        private void frmAds_Load(object sender, EventArgs e)
            {
            //////disk = new Gator.GGDisk();
            web = new MyWeb.MyWeb();
            utils = new MyUtils.MyUtils();
            disk = new Gator.GGDisk();
            curpage = 0;
            ads = new AdAPI(ggemail);

            bgwAds.RunWorkerAsync( );

            
            }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
            {
            if (listBox1.SelectedIndex != -1)
                {
                System.Diagnostics.Process.Start(ads.MyOffers[listBox1.SelectedIndex].url);
                }
            }

        private void button2_Click(object sender, EventArgs e)
            {
            AdListRefresh(sender);
            }
        private int curpage = 0;
        private void DisplayOffers(object sender)
            {
            BackgroundWorker worker = bgwAds;

            lbl1Ad1.Visible = false;
            lbl2Ad1.Visible = false;
            btnAd1.Visible = false;
            pbAd1.Visible = false;

            lbl1Ad2.Visible = false;
            lbl2Ad2.Visible = false;
            btnAd2.Visible = false;
            pbAd2.Visible = false;

            lbl1Ad3.Visible = false;
            lbl2Ad3.Visible = false;
            btnAd3.Visible = false;
            pbAd3.Visible = false;

            lbl1Ad4.Visible = false;
            lbl2Ad4.Visible = false;
            btnAd4.Visible = false;
            pbAd4.Visible = false;

            lbl1Ad5.Visible = false;
            lbl2Ad5.Visible = false;
            btnAd5.Visible = false;
            pbAd5.Visible = false;

            lbl1Ad6.Visible = false;
            lbl2Ad6.Visible = false;
            btnAd6.Visible = false;
            pbAd6.Visible = false;

            lbl1Ad7.Visible = false;
            lbl2Ad7.Visible = false;
            btnAd7.Visible = false;
            pbAd7.Visible = false;

            lbl1Ad8.Visible = false;
            lbl2Ad8.Visible = false;
            btnAd8.Visible = false;
            pbAd8.Visible = false;


            int limit = ads.count;
            if (ads.count  == 0)
                return;
            else
                {
                limit = ads.count % 8;
                if (limit == 0)
                    limit = 8;
                }

            int itmp = curpage + 1;
            lblCurAdPage.Text = itmp.ToString( );
            
            string creative = disk.filedefcreat;
            for (int i = 0; i < limit; i++)
                {                
                if (ads.MyOffers[i + curpage * 8].creative != "default.gif" )
                    {
                    if (!File.Exists(disk.dirimgs + "\\" + ads.MyOffers[i + curpage * 8].creative))
                        {
                        web.DownloadFile("http://adscendmedia.com/creative.php?id=" + ads.MyOffers[i + curpage * 8].creative, disk.dirimgs + "\\" + ads.MyOffers[i + curpage * 8].creative + ".gif", 5, 2000);
                        }
                    }
                }
             try
                {
                string t2c = string.Empty;
                string gain = string.Empty;
                if (ads.count > 0)
                    {
                    if (limit >= 0)
                        {
                        //LoadImageIntoPictureBox(disk.dirimgs + "\\" + ads.MyOffers[0 + curpage * 8].creative + ".gif", pbAd1);
                        //lbl1Ad1.Text = ads.MyOffers[0 + curpage * 8].desc;
                        //t2c = ads.MyOffers[0 + curpage * 8].time2credit;
                        //if (t2c == "0")
                        //    lbl2Ad1.Text = "Instant GP Credit";
                        //else
                        //    lbl2Ad1.Text = "GP credited in " + t2c + " days";
                        //gain = ads.MyOffers[0 + curpage * 8].payout;
                        //int pt = ads.ConvertMoneyToGP(gain);
                        //btnAd1.Text = "Get " + pt.ToString() + " Gator Points!";
                        //lbl1Ad1.Visible = true;
                        //lbl2Ad1.Visible = true;
                        //btnAd1.Visible = true;
                        //pbAd1.Visible = true;
                        worker.ReportProgress( 41 );
                        }
                    if (limit >= 1)
                        {
                        //LoadImageIntoPictureBox(disk.dirimgs + "\\" + ads.MyOffers[1 + curpage * 8].creative + ".gif", pbAd2);
                        //lbl1Ad2.Text = ads.MyOffers[1 + curpage * 8].desc;
                        //t2c = ads.MyOffers[1 + curpage * 8].time2credit;
                        //if (t2c == "0")
                        //    lbl2Ad2.Text = "Instant GP Credit";
                        //else
                        //    lbl2Ad2.Text = "GP credited in " + t2c + " days";
                        //gain = ads.MyOffers[1 + curpage * 8].payout;
                        //int pt = ads.ConvertMoneyToGP(gain);
                        //btnAd2.Text = "Get " + pt.ToString() + "Gator Points!";
                        //lbl1Ad2.Visible = true;
                        //lbl2Ad2.Visible = true;
                        //btnAd2.Visible = true;
                        //pbAd2.Visible = true;
                        worker.ReportProgress( 42 );
                        }
                    if (limit >= 2)
                        {
                        //LoadImageIntoPictureBox(disk.dirimgs + "\\" + ads.MyOffers[2 + curpage * 8].creative + ".gif", pbAd3);
                        //lbl1Ad3.Text = ads.MyOffers[2 + curpage * 8].desc;
                        //t2c = ads.MyOffers[2 + curpage * 8].time2credit;
                        //if (t2c == "0")
                        //    lbl2Ad3.Text = "Instant GP Credit";
                        //else
                        //    lbl2Ad3.Text = "GP credited in " + t2c + " days";
                        //gain = ads.MyOffers[2 + curpage * 8].payout;
                        //int pt = ads.ConvertMoneyToGP(gain);
                        //btnAd3.Text = "Get " + pt.ToString() + "Gator Points!";
                        //lbl1Ad3.Visible = true;
                        //lbl2Ad3.Visible = true;
                        //btnAd3.Visible = true;
                        //pbAd3.Visible = true;
                        worker.ReportProgress( 43 );
                        }
                    if (limit >= 3)
                        {
                        //LoadImageIntoPictureBox(disk.dirimgs + "\\" + ads.MyOffers[3 + curpage * 8].creative + ".gif", pbAd4);
                        //lbl1Ad4.Text = ads.MyOffers[3 + curpage * 8].desc;
                        //t2c = ads.MyOffers[3 + curpage * 8].time2credit;
                        //if (t2c == "0")
                        //    lbl2Ad4.Text = "Instant GP Credit";
                        //else
                        //    lbl2Ad4.Text = "GP credited in " + t2c + " days";
                        //gain = ads.MyOffers[3 + curpage * 8].payout;
                        //int pt = ads.ConvertMoneyToGP(gain);
                        //btnAd4.Text = "Get " + pt.ToString() + "Gator Points!";
                        //lbl1Ad4.Visible = true;
                        //lbl2Ad4.Visible = true;
                        //btnAd4.Visible = true;
                        //pbAd4.Visible = true;
                        worker.ReportProgress( 44 );
                        }
                    if (limit >= 4)
                        {
                        //LoadImageIntoPictureBox(disk.dirimgs + "\\" + ads.MyOffers[4 + curpage * 8].creative + ".gif", pbAd5);
                        //lbl1Ad5.Text = ads.MyOffers[4 + curpage * 8].desc;
                        //t2c = ads.MyOffers[4 + curpage * 8].time2credit;
                        //if (t2c == "0")
                        //    lbl2Ad5.Text = "Instant GP Credit";
                        //else
                        //    lbl2Ad5.Text = "GP credited in " + t2c + " days";
                        //gain = ads.MyOffers[4 + curpage * 8].payout;
                        //int pt = ads.ConvertMoneyToGP(gain);
                        //btnAd5.Text = "Get " + pt.ToString() + "Gator Points!";
                        //lbl1Ad5.Visible = true;
                        //lbl2Ad5.Visible = true;
                        //btnAd5.Visible = true;
                        //pbAd5.Visible = true;
                        worker.ReportProgress( 45 );
                        }
                    if (limit >= 5)
                        {
                        //LoadImageIntoPictureBox(disk.dirimgs + "\\" + ads.MyOffers[5 + curpage * 8].creative + ".gif", pbAd6);
                        //lbl1Ad6.Text = ads.MyOffers[5 + curpage * 8].desc;
                        //t2c = ads.MyOffers[5 + curpage * 8].time2credit;
                        //if (t2c == "0")
                        //    lbl2Ad6.Text = "Instant GP Credit";
                        //else
                        //    lbl2Ad6.Text = "GP credited in " + t2c + " days";
                        //gain = ads.MyOffers[5 + curpage * 8].payout;
                        //int pt = ads.ConvertMoneyToGP(gain);
                        //btnAd6.Text = "Get " + pt.ToString() + "Gator Points!";
                        //lbl1Ad6.Visible = true;
                        //lbl2Ad6.Visible = true;
                        //btnAd6.Visible = true;
                        //pbAd6.Visible = true;
                        worker.ReportProgress( 46 );
                        }
                    if (limit >= 6)
                        {
                        //LoadImageIntoPictureBox(disk.dirimgs + "\\" + ads.MyOffers[6 + curpage * 8].creative + ".gif", pbAd7);
                        //lbl1Ad7.Text = ads.MyOffers[6 + curpage * 8].desc;
                        //t2c = ads.MyOffers[6 + curpage * 8].time2credit;
                        //if (t2c == "0")
                        //    lbl2Ad7.Text = "Instant GP Credit";
                        //else
                        //    lbl2Ad7.Text = "GP credited in " + t2c + " days";
                        //gain = ads.MyOffers[6 + curpage * 8].payout;
                        //int pt = ads.ConvertMoneyToGP(gain);
                        //btnAd7.Text = "Get " + pt.ToString() + "Gator Points!";
                        //lbl1Ad7.Visible = true;
                        //lbl2Ad7.Visible = true;
                        //btnAd7.Visible = true;
                        //pbAd7.Visible = true;
                        worker.ReportProgress( 47 );
                        }
                    if (limit >= 7)
                        {
                        //LoadImageIntoPictureBox(disk.dirimgs + "\\" + ads.MyOffers[7 + curpage * 8].creative + ".gif", pbAd8);
                        //lbl1Ad8.Text = ads.MyOffers[7 + curpage * 8].desc;
                        //t2c = ads.MyOffers[7 + curpage * 8].time2credit;
                        //if (t2c == "0")
                        //    lbl2Ad8.Text = "Instant GP Credit";
                        //else
                        //    lbl2Ad8.Text = "GP credited in " + t2c + " days";
                        //gain = ads.MyOffers[7 + curpage * 8].payout;
                        //int pt = ads.ConvertMoneyToGP(gain);
                        //btnAd8.Text = "Get " + pt.ToString() + "Gator Points!";
                        //lbl1Ad8.Visible = true;
                        //lbl2Ad8.Visible = true;
                        //btnAd8.Visible = true;
                        //pbAd8.Visible = true;
                        worker.ReportProgress( 48 );
                        }
                    }
                }
            catch (Exception ex)
                {
                ;
                }
            }
        private void LoadImageIntoPictureBox(string imgloc, System.Windows.Forms.PictureBox pbox)
            {
            FileStream fileStream = File.OpenRead(imgloc);
            MemoryStream memStream = new MemoryStream();
            memStream.SetLength(fileStream.Length);
            fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
            pbox.Image = Image.FromStream(memStream);
            pbox.Refresh();
            fileStream.Dispose();
            memStream.Dispose();
            }
        private void AdListRefresh(object sender)
            {
            BackgroundWorker worker = sender as BackgroundWorker;

            worker.ReportProgress( 10 );
            ads.GetAllOffers();
            worker.ReportProgress( 20 );
            label1.Text = "total offers currently availabe : " + ads.count.ToString();
            worker.ReportProgress( 30 );
            DisplayOffers( sender);
            worker.ReportProgress( 50 );
            worker.ReportProgress( 100 );
            }

        private void btnNext_Click(object sender, EventArgs e)
            {
            int totalpages = ads.count / 8;
            if (curpage >= totalpages - 1)
                return;
            ++curpage;           
            int itmp = curpage + 1;
            lblCurAdPage.Text = itmp.ToString( );
            DisplayOffers( sender );

            }

        private void btnPrev_Click(object sender, EventArgs e)
            {
            if (curpage <= 0)
                return;
            --curpage;
            int itmp = curpage + 1;
            lblCurAdPage.Text = itmp.ToString( );
            DisplayOffers( sender );

            }

        private void bgwAds_DoWork ( object sender, DoWorkEventArgs e )
            {
            AdListRefresh( sender );
            }

        private void bgwAds_ProgressChanged ( object sender, ProgressChangedEventArgs e )
            {
            string t2c = string.Empty;
            string gain = string.Empty;

            if (e.ProgressPercentage >= 41)
                {
                LoadImageIntoPictureBox( disk.dirimgs + "\\" + ads.MyOffers[0 + curpage * 8].creative + ".gif", pbAd1 );
                lbl1Ad1.Text = ads.MyOffers[0 + curpage * 8].desc;
                t2c = ads.MyOffers[0 + curpage * 8].time2credit;
                if (t2c == "0")
                    lbl2Ad1.Text = "Instant GP Credit";
                else
                    lbl2Ad1.Text = "GP credited in " + t2c + " days";
                gain = ads.MyOffers[0 + curpage * 8].payout;
                int pt = ads.ConvertMoneyToGP( gain );
                btnAd1.Text = "Get " + pt.ToString( ) + " Gator Points!";
                lbl1Ad1.Visible = true;
                lbl2Ad1.Visible = true;
                btnAd1.Visible = true;
                pbAd1.Visible = true;
                }
            if (e.ProgressPercentage >= 42)
                {
                LoadImageIntoPictureBox( disk.dirimgs + "\\" + ads.MyOffers[1 + curpage * 8].creative + ".gif", pbAd2 );
                lbl1Ad2.Text = ads.MyOffers[1 + curpage * 8].desc;
                t2c = ads.MyOffers[1 + curpage * 8].time2credit;
                if (t2c == "0")
                    lbl2Ad2.Text = "Instant GP Credit";
                else
                    lbl2Ad2.Text = "GP credited in " + t2c + " days";
                gain = ads.MyOffers[1 + curpage * 8].payout;
                int pt = ads.ConvertMoneyToGP( gain );
                btnAd2.Text = "Get " + pt.ToString( ) + "Gator Points!";
                lbl1Ad2.Visible = true;
                lbl2Ad2.Visible = true;
                btnAd2.Visible = true;
                pbAd2.Visible = true;
                }
            if (e.ProgressPercentage >= 43)
                {
                LoadImageIntoPictureBox( disk.dirimgs + "\\" + ads.MyOffers[2 + curpage * 8].creative + ".gif", pbAd3 );
                lbl1Ad3.Text = ads.MyOffers[2 + curpage * 8].desc;
                t2c = ads.MyOffers[2 + curpage * 8].time2credit;
                if (t2c == "0")
                    lbl2Ad3.Text = "Instant GP Credit";
                else
                    lbl2Ad3.Text = "GP credited in " + t2c + " days";
                gain = ads.MyOffers[2 + curpage * 8].payout;
                int pt = ads.ConvertMoneyToGP( gain );
                btnAd3.Text = "Get " + pt.ToString( ) + "Gator Points!";
                lbl1Ad3.Visible = true;
                lbl2Ad3.Visible = true;
                btnAd3.Visible = true;
                pbAd3.Visible = true;
                }
            if (e.ProgressPercentage >= 44)
                {
                LoadImageIntoPictureBox( disk.dirimgs + "\\" + ads.MyOffers[3 + curpage * 8].creative + ".gif", pbAd4 );
                lbl1Ad4.Text = ads.MyOffers[3 + curpage * 8].desc;
                t2c = ads.MyOffers[3 + curpage * 8].time2credit;
                if (t2c == "0")
                    lbl2Ad4.Text = "Instant GP Credit";
                else
                    lbl2Ad4.Text = "GP credited in " + t2c + " days";
                gain = ads.MyOffers[3 + curpage * 8].payout;
                int pt = ads.ConvertMoneyToGP( gain );
                btnAd4.Text = "Get " + pt.ToString( ) + "Gator Points!";
                lbl1Ad4.Visible = true;
                lbl2Ad4.Visible = true;
                btnAd4.Visible = true;
                pbAd4.Visible = true;
                }
            if (e.ProgressPercentage >= 45)
                {
                LoadImageIntoPictureBox( disk.dirimgs + "\\" + ads.MyOffers[4 + curpage * 8].creative + ".gif", pbAd5 );
                lbl1Ad5.Text = ads.MyOffers[4 + curpage * 8].desc;
                t2c = ads.MyOffers[4 + curpage * 8].time2credit;
                if (t2c == "0")
                    lbl2Ad5.Text = "Instant GP Credit";
                else
                    lbl2Ad5.Text = "GP credited in " + t2c + " days";
                gain = ads.MyOffers[4 + curpage * 8].payout;
                int pt = ads.ConvertMoneyToGP( gain );
                btnAd5.Text = "Get " + pt.ToString( ) + "Gator Points!";
                lbl1Ad5.Visible = true;
                lbl2Ad5.Visible = true;
                btnAd5.Visible = true;
                pbAd5.Visible = true;
                }
            if (e.ProgressPercentage >= 46)
                {
                LoadImageIntoPictureBox( disk.dirimgs + "\\" + ads.MyOffers[5 + curpage * 8].creative + ".gif", pbAd6 );
                lbl1Ad6.Text = ads.MyOffers[5 + curpage * 8].desc;
                t2c = ads.MyOffers[5 + curpage * 8].time2credit;
                if (t2c == "0")
                    lbl2Ad6.Text = "Instant GP Credit";
                else
                    lbl2Ad6.Text = "GP credited in " + t2c + " days";
                gain = ads.MyOffers[5 + curpage * 8].payout;
                int pt = ads.ConvertMoneyToGP( gain );
                btnAd6.Text = "Get " + pt.ToString( ) + "Gator Points!";
                lbl1Ad6.Visible = true;
                lbl2Ad6.Visible = true;
                btnAd6.Visible = true;
                pbAd6.Visible = true;
                }
            if (e.ProgressPercentage >= 47)
                {
                LoadImageIntoPictureBox( disk.dirimgs + "\\" + ads.MyOffers[6 + curpage * 8].creative + ".gif", pbAd7 );
                lbl1Ad7.Text = ads.MyOffers[6 + curpage * 8].desc;
                t2c = ads.MyOffers[6 + curpage * 8].time2credit;
                if (t2c == "0")
                    lbl2Ad7.Text = "Instant GP Credit";
                else
                    lbl2Ad7.Text = "GP credited in " + t2c + " days";
                gain = ads.MyOffers[6 + curpage * 8].payout;
                int pt = ads.ConvertMoneyToGP( gain );
                btnAd7.Text = "Get " + pt.ToString( ) + "Gator Points!";
                lbl1Ad7.Visible = true;
                lbl2Ad7.Visible = true;
                btnAd7.Visible = true;
                pbAd7.Visible = true;
                }
            if (e.ProgressPercentage >= 48)
                {
                LoadImageIntoPictureBox( disk.dirimgs + "\\" + ads.MyOffers[7 + curpage * 8].creative + ".gif", pbAd8 );
                lbl1Ad8.Text = ads.MyOffers[7 + curpage * 8].desc;
                t2c = ads.MyOffers[7 + curpage * 8].time2credit;
                if (t2c == "0")
                    lbl2Ad8.Text = "Instant GP Credit";
                else
                    lbl2Ad8.Text = "GP credited in " + t2c + " days";
                gain = ads.MyOffers[7 + curpage * 8].payout;
                int pt = ads.ConvertMoneyToGP( gain );
                btnAd8.Text = "Get " + pt.ToString( ) + "Gator Points!";
                lbl1Ad8.Visible = true;
                lbl2Ad8.Visible = true;
                btnAd8.Visible = true;
                pbAd8.Visible = true;
                }

            }

     
        }
    }