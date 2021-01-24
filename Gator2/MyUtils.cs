using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MyUtils
    {
    class MyUtils
        {
        public string GetStrBetween(string mainstr, string tag1, string tag2)
            {
            int start, stop, len;
            start = stop = len = -1;
            string tmp = string.Empty;
            if (mainstr != null)
                {
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
                }
            return tmp;
            }
        public int CountString(string buff, string substr)
            {
            int ret = 0;
            int beg = -1;
            int lastpos = 0;
            do
                {
                beg = buff.IndexOf(substr, lastpos);
                if (beg != -1)
                    {
                    ret++;
                    lastpos = beg + substr.Length;
                    }
                if (lastpos >= buff.Length -1)
                    break;
                }while (beg != -1 );
            
            return ret;
            }
        public ArrayList GetTokensBetween(string mainstr, string begTag, string endTag)
            {
            ArrayList ret = new ArrayList();
            ret.Clear();

            int beg, end;
            beg = end = -1;
            string tmp = string.Empty;
            if (mainstr != null)
                {
                beg = mainstr.IndexOf(begTag);
                while (beg != -1)
                    {
                    beg = beg + begTag.Length;
                    end = mainstr.IndexOf(endTag, beg);
                    if ((end != -1) && ((end - beg) >= 0))
                        {
                        tmp = mainstr.Substring(beg, end - beg);
                        ret.Add(tmp);
                        end = end + endTag.Length;
                        }
                    beg = mainstr.IndexOf(begTag, end);
                    };
                }
            return ret;
            }
        public ArrayList GetTokensBetween(string mainstr, string startTag1, string startTag2, string begTag, string endTag )
            {
            ArrayList ret = new ArrayList();
            ret.Clear();

            int beg, end;
            beg = end = -1;
            if (mainstr != null)
                {
                beg = mainstr.IndexOf(startTag1);
                string tmp = string.Empty;
                if (beg != -1)
                    {
                    if (startTag2 != string.Empty)
                        beg = mainstr.IndexOf(startTag2, beg);

                    beg = mainstr.IndexOf(begTag, beg);
                    while (beg != -1)
                        {
                        beg = beg + begTag.Length;
                        end = mainstr.IndexOf(endTag, beg);
                        if ((end != -1) && ((end - beg) >= 0))
                            {
                            tmp = mainstr.Substring(beg, end - beg);
                            ret.Add(tmp);
                            end = end + endTag.Length;
                            }
                        beg = mainstr.IndexOf(begTag, end);
                        }
                    }
                }
            return ret;
            }
        public ArrayList GetTokensBetween(string mainstr, string startTag1, string startTag2, string begTag, string endTag, string padHead, string padTail )
            {
            ArrayList ret = new ArrayList();
            ret.Clear();

            int beg, end;
            beg = end = -1;
            if (mainstr != null)
                {
                beg = mainstr.IndexOf(startTag1);
                string tmp = string.Empty;
                if (beg != -1)
                    {
                    if (startTag2 != string.Empty)
                        beg = mainstr.IndexOf(startTag2, beg);

                    beg = mainstr.IndexOf(begTag, beg);
                    while (beg != -1)
                        {
                        beg = beg + begTag.Length;
                        end = mainstr.IndexOf(endTag, beg);
                        if ((end != -1) && ((end - beg) >= 0))
                            {
                            tmp = mainstr.Substring(beg, end - beg);
                            tmp = padHead + tmp + padTail;
                            ret.Add(tmp);
                            end = end + endTag.Length;
                            }
                        beg = mainstr.IndexOf(begTag, end);
                        }
                    }
                }
            return ret;
            }

        
        }
    }
