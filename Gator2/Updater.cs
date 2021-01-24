using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.IO.Compression;
using System.Diagnostics;
using Gator2;

namespace Gator
    {
    class Updater
        {
        private string lncv = string.Empty;
        private string lncf = string.Empty;
        private string lcv = string.Empty;
        private string lcf = string.Empty;
        private string cv = string.Empty;
        public bool checking = false;

        public Constants.UpdateType type;

        public void HandleUpdate( Constants.UpdateType updtype, string curversion )
            {
            if (checking == false)
                {
                type = updtype;
                cv = curversion;
                checking = true;
                Thread thUpdate;
                ThreadStart thUpdateStart;
                thUpdateStart = new ThreadStart(UpdaterProc);
                thUpdate = new Thread(thUpdateStart);             
                thUpdate.IsBackground = true;
                thUpdate.Start();
                }
            else
                {
                if (updtype != Constants.UpdateType.Auto)
                    {
                    UpdateMessageDlg updmsgdlg = new UpdateMessageDlg("Another update in progress. Please wait till its complete!", Constants.UpdateMessageType.Info);
                    //Log("Another update in progress. Please wait till its complete!", Constants.LogMsgType.Error, true);
                    updmsgdlg.ShowDialog();
                    updmsgdlg.Dispose();
                    }
                }
            }

        private void UpdaterProc()
            {
            string remote = string.Empty;          
            Constants.AvailableUpdate status = IsUpdateAvailable(ref remote);
            UpdateMessageDlg updmsgdlg;
            Constants.UpdateResponse dlgupdresp;
            if (status == Constants.AvailableUpdate.NoUpdate)
                {
                if (type == Constants.UpdateType.Manual)
                    {
                    updmsgdlg = new UpdateMessageDlg("Your version of Group Gator is latest! No new updates available", Constants.UpdateMessageType.Info);
                    updmsgdlg.Display();
                    updmsgdlg.Dispose();
                    }
                checking = false;
                return;
                }
            else if (status == Constants.AvailableUpdate.NonCritical)
                {
                updmsgdlg = new UpdateMessageDlg("A new version of GroupGator is available. Do you want to download this update?", Constants.UpdateMessageType.Question);
                dlgupdresp = updmsgdlg.Display();
                updmsgdlg.Dispose();
                if (dlgupdresp == Constants.UpdateResponse.Yes  )
                    {
                    GGDisk disk = new GGDisk();
                    try
                        {                                               
                        if (File.Exists(disk.fileupd))
                            File.Delete(disk.fileupd);

                        DownloadUpdate(remote, disk.fileupd);

                        updmsgdlg = new UpdateMessageDlg("Update downloaded! Would you like to exit GroupGator now and install this new update?", Constants.UpdateMessageType.Question);
                        dlgupdresp = updmsgdlg.Display();
                        updmsgdlg.Dispose();
                        if (dlgupdresp == Constants.UpdateResponse.Yes)
                            {
                            System.Diagnostics.Process.Start(disk.fileupd);
                            //todo freeversion update
                            Environment.Exit(0);
                            }
                        }
                    catch (Exception ex)
                        {
                        ;// log.Show(ex.Message.ToString(), 0, true);
                        }
                    finally
                        {
                        checking = false;
                        }
                    }
                }
            else if (status == Constants.AvailableUpdate.Critical)
                {
                GGDisk disk = new GGDisk();
                try
                    {                   
                    if (File.Exists(disk.fileupd))
                        File.Delete(disk.fileupd);

                    DownloadUpdate(remote, disk.fileupd);
                    updmsgdlg = new UpdateMessageDlg("CRITICAL UPDATE: GroupGATOR will now exit and apply the update.", Constants.UpdateMessageType.Info);
                    updmsgdlg.Display();
                    updmsgdlg.Dispose();

                    System.Diagnostics.Process.Start(disk.fileupd);
                  //todo freeversion update
                    Environment.Exit(0);
                    }
                catch (Exception ex)
                    {
                    ;
                    }
                finally
                    {
                    checking = false;
                    }
                }

            checking = false;

            }
        public Constants.AvailableUpdate IsUpdateAvailable(ref string loc)
            {            
            string updfile = string.Empty;
            MyUtils.MyUtils util = new MyUtils.MyUtils();
            MyWeb.MyWeb myweb = new MyWeb.MyWeb();
            WebService wserv = new WebService();
            while (updfile == string.Empty)
                {
                updfile = myweb.GetWebPage(wserv.urlupdchk, 0, 0);
                System.Threading.Thread.Sleep(3000);
                }     
            lcv = util.GetStrBetween (updfile, "<TYPEC>", "</TYPEC>");
            lcf = util.GetStrBetween(updfile, "<CFILE>", "</CFILE>");
            lncv = util.GetStrBetween(updfile, "<TYPENC>", "</TYPENC>");
            lncf = util.GetStrBetween(updfile, "<NCFILE>", "</NCFILE>");
            cv = cv.Replace(".", string.Empty);
            lncv = lncv.Replace(".", string.Empty);
            lcv = lcv.Replace(".", string.Empty);

            int icv = 0;
            int ilncv = 0;
            int ilcv = 0;

            try
                {
                icv = Convert.ToInt32(cv);
                ilncv = Convert.ToInt32(lncv);
                ilcv = Convert.ToInt32(lcv);
                }
            catch (Exception ex)
                {
                icv = 0;
                ilncv = 0;
                ilcv = 0;
                }

            Constants.AvailableUpdate ret = Constants.AvailableUpdate.NoUpdate;

            if (icv > ilcv)
                {
                ret = Constants.AvailableUpdate.NoUpdate;
                if (ilncv > icv)
                    {
                    ret = Constants.AvailableUpdate.NonCritical;
                    loc = lncf;
                    }
                }
            else if (icv == ilcv)
                {
                if (ilncv > icv)
                    {
                    loc = lncf;
                    ret = Constants.AvailableUpdate.NonCritical;
                    }
                }
            else if (icv < ilcv)
                {
                ret = Constants.AvailableUpdate.Critical;
                if (ilncv >= ilcv)
                    loc = lncf;
                else
                    loc = lcf;
                }

            return ret;
            }

        private int updateret = 0;
        public void UpdateReturn(int ret)
            {
            updateret = ret;
            }

        public void DownloadUpdate(string remote, string local)
            {
            MyWeb.MyWeb web = new MyWeb.MyWeb();
            web.DownloadFile(remote, local, 5, 5000);
            }

        }
    }