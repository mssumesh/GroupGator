using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;


namespace MyWeb
    {
    class MyWeb 
        {
        //private Gator2Main gmain;
        public MyWeb()
            {
            //gmain = null;
            }
        //public MyWeb(Gator2Main gt)
        //    {
        //    gmain = gt;
        //    }
        
        public string GetWebPage(string reqpage, int nretry, int waitretry)
            {
            WebClient wc;

            string post = string.Empty;
            int retry = 0;
            

        GoHere:
            wc = new WebClient();
            wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            try
                {
                retry++;
                post = wc.DownloadString(reqpage);
                }
            catch (Exception ex)
                {
                if (ex is WebException)
                    {
                    WebException wex = (WebException)ex;
                    if ((wex.Status == WebExceptionStatus.ConnectFailure) ||
                        (wex.Status == WebExceptionStatus.NameResolutionFailure) ||
                        (wex.Status == WebExceptionStatus.Timeout) ||
                        (wex.Status == WebExceptionStatus.ConnectionClosed))
                        {
                        //if (gmain != null)
                        //    gmain.Log("Network Error (" + wex.Message + ") - retry# : " + retry.ToString(), 0, true);
                        wc.Dispose();
                        System.Threading.Thread.Sleep(waitretry);
                        if (retry < nretry)
                            goto GoHere;
                        }
                    }
                else
                    {
                    ;
                    }               
                };
            wc.Dispose();

            return post;
            }
        public string GetWebPage(string reqpage, int nretry, int waitretry, ref string cuki)
            {
            WebClient wc;

            string post = string.Empty;
            int retry = 0;

        GoHere:
            wc = new WebClient();        

            wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            try
                {
                //wc.Headers.Add(HttpRequestHeader.Cookie, cuki);
                wc.Headers.Add("Cookie", cuki);
                retry++;
                post = wc.DownloadString(reqpage);
                cuki = wc.ResponseHeaders["Set-Cookie"];
                }
            catch (Exception ex)
                {
                if (ex is WebException)
                    {
                    WebException wex = (WebException)ex;
                    if ((wex.Status == WebExceptionStatus.ConnectFailure) ||
                        (wex.Status == WebExceptionStatus.NameResolutionFailure) ||
                        (wex.Status == WebExceptionStatus.Timeout) ||
                        (wex.Status == WebExceptionStatus.ConnectionClosed))
                        {
                        //if (gmain != null)
                        //    gmain.Log("Network Error (" + wex.Message + ") - retry# : " + retry.ToString(), 0, true);
                        wc.Dispose();
                        System.Threading.Thread.Sleep(waitretry);
                        if (retry < nretry)
                            goto GoHere;
                        }
                    }
                else
                    {
                    ;
                    }
                };
            //cuki = wc.Headers[HttpResponseHeader.SetCookie];
            
            wc.Dispose();

            return post;
            }

        public string PostPage(string target, NameValueCollection nvm, int nretry, int waitretry)
            {
            WebClient wc;

            string post = string.Empty;
            int retry = 0;

        GoHere:
            wc = new WebClient();
            wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            try
                {
                retry++;
                byte[] responseBytes = wc.UploadValues(target, "POST", nvm);
                post = Encoding.UTF8.GetString(responseBytes);
                }
            catch (Exception ex)
                {
                if (ex is WebException)
                    {
                    WebException wex = (WebException)ex;
                    if ((wex.Status == WebExceptionStatus.ConnectFailure) ||
                        (wex.Status == WebExceptionStatus.NameResolutionFailure) ||
                        (wex.Status == WebExceptionStatus.Timeout) ||
                        (wex.Status == WebExceptionStatus.ConnectionClosed))
                        {
                        //if (gmain != null)
                        //    gmain.Log("Network Error (" + wex.Message + ") - retry# : " + retry.ToString(), 0, true);
                        wc.Dispose();
                        System.Threading.Thread.Sleep(waitretry);
                        if (retry < nretry)
                            goto GoHere;
                        }
                    }
                else
                    {
                    ;
                    }
                };
            wc.Dispose();

            return post;
            }
        public string PostPage(string target, NameValueCollection nvm, int nretry, int waitretry, ref string cuki)
            {
            WebClient wc;

            string post = string.Empty;
            int retry = 0;

        GoHere:
            wc = new WebClient();

            wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            try
                {
                //wc.Headers.Add(HttpRequestHeader.Cookie, cuki);
                wc.Headers.Add("Cookie", cuki);
                retry++;
                byte[] responseBytes = wc.UploadValues(target, "POST", nvm);
                post = Encoding.UTF8.GetString(responseBytes);
                cuki = wc.ResponseHeaders["Set-Cookie"];
                }
            catch (Exception ex)
                {
                if ( ex is WebException )
                    {
                    WebException wex = (WebException)ex;
                    if ((wex.Status == WebExceptionStatus.ConnectFailure) ||
                        (wex.Status == WebExceptionStatus.NameResolutionFailure) ||
                        (wex.Status == WebExceptionStatus.Timeout) ||
                        (wex.Status == WebExceptionStatus.ConnectionClosed))
                        {
                        //if (gmain != null)
                        //    gmain.Log("Network Error (" + wex.Message + ") - retry# : " + retry.ToString(), 0, true);
                        wc.Dispose();
                        System.Threading.Thread.Sleep(waitretry);
                        if (retry < nretry)
                            goto GoHere;
                        }
                    }
                else
                    {
                    ;
                    }
                };
            //cuki = wc.Headers[HttpResponseHeader.SetCookie];
            
            wc.Dispose();

            return post;
            }

        public string PostPage(string target, NameValueCollection nvm, int nretry, int waitretry, string cuki )
            {
            WebClient wc;

            string post = string.Empty;
            int retry = 0;

        GoHere:
            wc = new WebClient();

            wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            try
                {
                //wc.Headers.Add(HttpRequestHeader.Cookie, cuki);
                wc.Headers.Add("Cookie", cuki);
                retry++;
                byte[] responseBytes = wc.UploadValues(target, "POST", nvm);
                post = Encoding.UTF8.GetString(responseBytes);
                }
            catch (Exception ex)
                {
                if (ex is WebException)
                    {
                    WebException wex = (WebException)ex;
                    if ((wex.Status == WebExceptionStatus.ConnectFailure) ||
                        (wex.Status == WebExceptionStatus.NameResolutionFailure) ||
                        (wex.Status == WebExceptionStatus.Timeout) ||
                        (wex.Status == WebExceptionStatus.ConnectionClosed))
                        {
                        //if (gmain != null)
                        //    gmain.Log("Network Error (" + wex.Message + ") - retry# : " + retry.ToString(), 0, true);
                        wc.Dispose();
                        System.Threading.Thread.Sleep(waitretry);
                        if (retry < nretry)
                            goto GoHere;
                        }
                    }
                else
                    {

                    ;
                    }
                };
            //cuki = wc.Headers[HttpResponseHeader.SetCookie];
            //cuki = wc.ResponseHeaders["Set-Cookie"];
            wc.Dispose();

            return post;
            }

        public void DownloadFile(string remotefile, string localfile, int nretry, int waitretry)
            {
            WebClient wc;
            int retry = 0;

        GoHere:
            wc = new WebClient();
            wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            try
                {
                retry++;
                wc.DownloadFile(remotefile, localfile);
                }
            catch (Exception ex)
                {
                if (ex is WebException)
                    {
                    WebException wex = (WebException)ex;
                    if ((wex.Status == WebExceptionStatus.ConnectFailure) ||
                        (wex.Status == WebExceptionStatus.NameResolutionFailure) ||
                        (wex.Status == WebExceptionStatus.Timeout) ||
                        (wex.Status == WebExceptionStatus.ConnectionClosed))
                        {
                        //if (gmain != null)
                        //    gmain.Log("Network Error (" + wex.Message + ") - retry# : " + retry.ToString(), 0, true);
                        wc.Dispose();
                        System.Threading.Thread.Sleep(waitretry);
                        if (retry < nretry)
                            goto GoHere;
                        }
                    }
                else
                    {
                    ;
                    }

                };
            wc.Dispose();

            
            }
        }
    }
