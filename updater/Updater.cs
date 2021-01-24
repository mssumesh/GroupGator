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

namespace Gator2
    {
    class Updater
        {

        private static string curver = string.Empty;
        private static string latestver = string.Empty;
        private static string updateurl = string.Empty;
        private string app_path;

        public Updater(string cver, string updurl, string path)
            {
            curver = cver;
            
            updateurl = updurl;
            app_path = path;
            }

        public bool IsUpdateAvailable( ref string loc)
            {         
            
            WebClient wc;
            string updfile = string.Empty;

        CheckUpdAgain:
            wc = new WebClient();
            try
                {
                wc = new WebClient();
                updfile = wc.DownloadString(updateurl);
                }
            catch (WebException wex)
                {
                if ((wex.Status == WebExceptionStatus.ConnectFailure) ||
                    (wex.Status == WebExceptionStatus.NameResolutionFailure) ||
                    (wex.Status == WebExceptionStatus.Timeout) ||
                    (wex.Status == WebExceptionStatus.ConnectionClosed))
                    {
                    //log.Show(gh.GetTime() + "unable to connect to update server. Retrying..", 0, true);
                    wc.Dispose();
                    System.Threading.Thread.Sleep(1000);
                    goto CheckUpdAgain;
                    }
                }
            wc.Dispose();

            latestver = GetSubSting(updfile, "<LVER>", "</LVER>");
            loc = GetSubSting(updfile, "<FILE>", "</FILE>");
            latestver = latestver.Replace(".", string.Empty);
            curver = curver.Replace(".", string.Empty);

            bool ret = false;
            try
                {
                if ((Convert.ToInt32(latestver)) > (Convert.ToInt32(curver)))
                    ret = true;
                else
                    ret = false;
                }
            catch (Exception ex)
                {
                //MessageBox.Show(ex.Message.ToString());
                }

            return ret;
  
            

            }

        public void DownloadUpdate( string remote, string local )
            {
            string buff = string.Empty;
            //int beg;
            WebClient wc;
            //beg = -1;
            
            DownloadUpdatedFileAgain:
            wc = new WebClient();
            try
                {
                //MessageBox.Show(remote);
                //MessageBox.Show(local);

                wc.DownloadFile(remote, local);
                
                }
            catch (WebException wex)
                {
                if ((wex.Status == WebExceptionStatus.ConnectFailure) ||
                    (wex.Status == WebExceptionStatus.NameResolutionFailure) ||
                    (wex.Status == WebExceptionStatus.Timeout) ||
                    (wex.Status == WebExceptionStatus.ConnectionClosed))
                    {
                    wc.Dispose();
                    System.Threading.Thread.Sleep(1000);
                    goto DownloadUpdatedFileAgain;
                    }
                }
            wc.Dispose();

            MessageBox.Show("Download of new GroupGator update complete!", "Update Complete!");
            }

        public string GetSubSting(string mainstr, string tag1, string tag2)
            {
            int start, stop, len;
            start = stop = len = -1;
            string tmp = string.Empty;
            start = mainstr.IndexOf(tag1);
            if (start != -1)
                {
                start += tag1.Length;
                stop = mainstr.IndexOf(tag2, start);
                len = stop - start;
                if ((stop != -1) && (len > 0))
                    {
                    tmp = mainstr.Substring(start, len);
                    }
                }
            return tmp;

            }
        }
    }
