using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Gator25
    {
    public partial class ListRandomizer : Form
        {
        private int iterations = 20000;
        public StringBuilder glist;


        public ListRandomizer()
            {
            InitializeComponent();
            
            }
        public ListRandomizer ( StringBuilder alist )
            {
            glist = new StringBuilder( );
            glist.Length = 0;
            glist.Append( alist.ToString( ) );
            }

        private void ListRandomizer_Load(object sender, EventArgs e)
            {
            StartRandomizeClick( ); 
            }
        public StringBuilder Display()
            {

            
            this.Show();
            RandProc( );
            return glist;
            }


        private int stepbig = 0;
        private int stepsmall = 0;

        private double curw = 0.0;

        Gator.GGDisk disk = new Gator.GGDisk();
        private void RandProc()            
            {

            panFore.Visible = true;
            panBack.Visible = true;
            //panFore.BringToFront();

            stepsmall = 10;
            int totalstepsmall = panBack.Width / 10;
            stepbig = iterations / totalstepsmall;

            string list1 = string.Empty;
            string list2 = string.Empty;
            int middle = glist.ToString().Length / 2;
            if (middle < 160)
                return;
            string checkstrend = "</steamID64>";
            string checkstrbeg = "<steamID64>";
            int beg = glist.ToString().IndexOf(checkstrend, middle);
            if (beg == -1)
                {
                checkstrend = "</UID>";
                checkstrbeg = "<UID>";
                beg = glist.ToString().IndexOf(checkstrend, middle);
                if (beg == -1)
                    return;
                }

            beg += checkstrend.Length;
            list1 = glist.ToString().Substring(0, beg);
            list2 = glist.ToString().Substring(beg);

            int start1, stop1, start2, stop2, len1, len2;
            start1 = stop1 = start2 = stop2 = -1;
            len1 = list1.Length;
            len2 = list2.Length;

            string str1, str2, tmp;
            str1 = str2 = tmp = string.Empty;

            int tick1 = Environment.TickCount;


            
            for (int i = 1; i <= iterations; i++)
                {
                Random rand = new Random(Environment.TickCount);
                start1 = rand.Next(0, len1 - 40);
                start1 = list1.IndexOf(checkstrbeg, start1);
                if (start1 == -1)
                    goto label;
                stop1 = list1.IndexOf(checkstrend, start1);
                if (stop1 == -1)
                    goto label;                
                stop1 += checkstrend.Length;
                str1 = list1.Substring(start1, stop1 - start1);

                rand = new Random(Environment.TickCount);
                start2 = rand.Next(0, len2 - 40);
                start2 = list2.IndexOf(checkstrbeg, start2);
                if (start2 == -1)
                    goto label;                
                stop2 = list2.IndexOf(checkstrend, start2);
                if (stop2 == -1)
                    goto label;                
                stop2 += checkstrend.Length;
                str2 = list2.Substring(start2, stop2 - start2);

                list1 = list1.Replace(str1, str2);
                list2 = list2.Replace(str2, str1);

            label:
                //double dstep1 = ( i  /  iterations )  * 100;                
                ////double dstep2 =  dstep1 / 100;
                //int iitmp = Convert.ToInt32(dstep1);
                //if ( iitmp == 0 )
                //    iitmp = 1;

                //progressBar1.Value = iitmp;
                //progressBar1.PerformStep();
                //progressBar1.Refresh();                
                //panFore.Width = panBack.Width * iitmp / 100;
                ////System.Threading.Thread.Sleep(5);
                curw = curw + 1.0;
                if (curw >= stepbig)
                    {
                    curw = 0.0;
                    panFore.Width = panFore.Width + stepsmall;
                    }
                panFore.Refresh();                
                }
            panFore.Width = panBack.Width;
            //panBack.Visible = false;
            glist.Length = 0;
            glist.Append(list1);
            glist.Append(list2);
            int tick2 = Environment.TickCount;
            int tim = ( tick2 - tick1 ) / 1000;
            label1.Text = "Randomize ends. Total time took = " + tim.ToString() + " seconds!";
            this.Text = "Randomize complete in " + tim.ToString() + " seconds";
            
            notstarted = false;
            System.Threading.Thread.Sleep( 2000 );
            this.Close( );
            
            //gBrush.Dispose();
            //gGraphics.Dispose();
            
            }

        private bool notstarted = true;
        private void button1_Click(object sender, EventArgs e)
            {
            //label1.Text = "Randomizing the gatherlist..";
            //this.Text = "List Randomizer";
            //panFore.Width = 10;
            //panFore.Visible = false;
            //panBack.Visible = false;
            //this.Refresh();
            //this.Hide();
            StartRandomizeClick( );
            }
        private void StartRandomizeClick ( )
            {
            //if (notstarted == true)
            //    {
            //    button1.Enabled = false;
            //    ThreadStart thRandStart = new ThreadStart( RandProc );
            //    Thread thRand = new Thread( thRandStart );
            //    thRand.IsBackground = true;
            //    thRand.Start( );
            //    }
            //else
            //    {
            //    this.Close( );
            //    }
            //RandProc( );
            }
        private void button2_Click(object sender, EventArgs e)
            {
            
            }

        private void label1_Click ( object sender, EventArgs e )
            {

            }
        }
    }