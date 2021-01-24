using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web;

namespace Gator25
    {
    public partial class PayPalWindow : Form
        {
        private string url = string.Empty;
        public PayPalWindow()
            {
            InitializeComponent();
            }

            
        private void PayPalWindow_Load(object sender, EventArgs e)
            {
            }
        private bool modepaypal = true;
        public void DisplayPaypal( string pcode )
            {
            modepaypal = true;
            url = "https://www.paypal.com/cgi-bin/webscr"; 
            this.WindowState = FormWindowState.Normal;
            this.Text = "Loading paypal. Please wait..";
            this.Cursor = Cursors.WaitCursor;
            wbPaypal.DocumentText = "<html><head><title>Loading paypal. Please wait..</title><body><center>Loading paypal. Please wait..</center></body></html>";
            this.Show();
            //String postdata = HttpUtility.UrlEncode ("cmd=_s-xclick&hosted_button_id=S7CTH5L6HQTGJ");
            String postdata = "cmd=_s-xclick&hosted_button_id=" + pcode; ;
            //switch (dtype)
            //    {
            //    case Gator.Constants.DonateType.Donate1:
            //        postdata = "cmd=_s-xclick&hosted_button_id=" + pcode;//S7CTH5L6HQTGJ";
            //        break;
            //    case Gator.Constants.DonateType.Donate2:
            //        postdata = "cmd=_s-xclick&hosted_button_id=BDPSWU257XP4A";
            //        break;
            //    case Gator.Constants.DonateType.Donate3:
            //        postdata = "cmd=_s-xclick&hosted_button_id=M7QCGYY4959F6";
            //        break;
            //    case Gator.Constants.DonateType.Donate4:
            //        postdata = "cmd=_s-xclick&hosted_button_id=HXUR8PKBB693W";
            //        break;
            //    case Gator.Constants.DonateType.Donate5:
            //        postdata = "cmd=_s-xclick&hosted_button_id=7RK3QHLXM2FWY";
            //        break;
            //    case Gator.Constants.DonateType.Donate6:
            //        postdata = "cmd=_s-xclick&hosted_button_id=BHP7ZTRKJC8PG";
            //        break;
            //    }
            System.Text.Encoding eutf8= System.Text.Encoding.UTF8;
            byte[] byPostData = eutf8.GetBytes(postdata);
            wbPaypal.Navigate(url, "_top", byPostData, "Content-Type: application/x-www-form-urlencoded\r\n");            
            }

        private void wbPaypal_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
            //if (wbPaypal.ReadyState != WebBrowserReadyState.Complete)
            //    return;
            if ( modepaypal == true )
                this.Text = "Donate securely via PayPal!";
            else
                this.Text = "Donate securely via Google!";
            //this.WindowState = FormWindowState.Maximized;
            this.Cursor = Cursors.Default;
            }
        public void DisplayGoogle(Gator.Constants.DonateType dtype)
            {
            modepaypal = false;
            url = "https://checkout.google.com/api/checkout/v2/checkoutForm/Merchant/947907143595969";
            this.WindowState = FormWindowState.Normal;
            this.Text = "Loading google. Please wait..";
            this.Cursor = Cursors.WaitCursor;
            wbPaypal.DocumentText = "<html><head><title>Loading google. Please wait..</title><body><center>Loading paypal. Please wait..</center></body></html>";
            this.Show();
            //String postdata = HttpUtility.UrlEncode ("cmd=_s-xclick&hosted_button_id=S7CTH5L6HQTGJ");
            String postdata = "&item_option_name_1=5+Days+%28Trial+Price%21%29&item_option_price_1=5.0&item_option_description_1=GroupGATOR&item_option_quantity_1=1&item_option_currency_1=USD&shopping-cart.item-options.items.item-1.digital-content.description=You+will+need+to+notify+our+staff+immediately+after+making+a+donation+so+time+can+be+assigned.+Thanks%21&shopping-cart.item-options.items.item-1.digital-content.url=http%3A%2F%2Fwww.groupgatorcommunity.net%2FGroupGATOR2%2FGroupGATOR2.zip&item_option_name_2=14+Days+%2825%25+Savings%21%29&item_option_price_2=10.0&item_option_description_2=GroupGATOR&item_option_quantity_2=1&item_option_currency_2=USD&shopping-cart.item-options.items.item-2.digital-content.description=You+will+need+to+notify+our+staff+immediately+after+making+a+donation+so+time+can+be+assigned.+Thanks%21&shopping-cart.item-options.items.item-2.digital-content.url=http%3A%2F%2Fwww.groupgatorcommunity.net%2FGroupGATOR2%2FGroupGATOR2.zip&item_option_name_3=30+Days+%2833%25+Savings%21&item_option_price_3=20.0&item_option_description_3=GroupGATOR&item_option_quantity_3=1&item_option_currency_3=USD&shopping-cart.item-options.items.item-3.digital-content.description=You+will+need+to+notify+our+staff+immediately+after+making+a+donation+so+time+can+be+assigned.+Thanks%21&shopping-cart.item-options.items.item-3.digital-content.url=http%3A%2F%2Fwww.groupgatorcommunity.net%2FGroupGATOR2%2FGroupGATOR2.zip&item_option_name_4=60+Days+%2850%25+Savings%21%29&item_option_price_4=30.0&item_option_description_4=GroupGATOR&item_option_quantity_4=1&item_option_currency_4=USD&shopping-cart.item-options.items.item-4.digital-content.description=You+will+need+to+notify+our+staff+immediately+after+making+a+donation+so+time+can+be+assigned.+Thanks%21&shopping-cart.item-options.items.item-4.digital-content.url=http%3A%2F%2Fwww.groupgatorcommunity.net%2FGroupGATOR2%2FGroupGATOR2.zip&item_option_name_5=150+Days+%2866%25+Savings%21%29&item_option_price_5=50.0&item_option_description_5=GroupGATOR&item_option_quantity_5=1&item_option_currency_5=USD&shopping-cart.item-options.items.item-5.digital-content.description=You+will+need+to+notify+our+staff+immediately+after+making+a+donation+so+time+can+be+assigned.+Thanks%21&shopping-cart.item-options.items.item-5.digital-content.url=http%3A%2F%2Fwww.groupgatorcommunity.net%2FGroupGATOR2%2FGroupGATOR2.zip&item_option_name_6=400+Days+%2875%25+Savings%21%29&item_option_price_6=100.0&item_option_description_6=GroupGATOR&item_option_quantity_6=1&item_option_currency_6=USD&shopping-cart.item-options.items.item-6.digital-content.description=You+will+need+to+notify+our+staff+immediately+after+making+a+donation+so+time+can+be+assigned.+Thanks%21&shopping-cart.item-options.items.item-6.digital-content.url=http%3A%2F%2Fwww.groupgatorcommunity.net%2FGroupGATOR2%2FGroupGATOR2.zip&x=35&y=22";
            String headdata = string.Empty;
            switch (dtype)
                {
                case Gator.Constants.DonateType.Donate1:
                    headdata = "item_selection_1=6";
                    break;
                case Gator.Constants.DonateType.Donate2:
                    headdata = "item_selection_1=5";
                    break;
                case Gator.Constants.DonateType.Donate3:
                    headdata = "item_selection_1=4";
                    break;
                case Gator.Constants.DonateType.Donate4:
                    headdata = "item_selection_1=3";
                    break;
                case Gator.Constants.DonateType.Donate5:
                    headdata = "item_selection_1=2";
                    break;
                case Gator.Constants.DonateType.Donate6:
                    headdata = "item_selection_1=1";
                    break;
                }
            postdata = headdata + postdata;
            System.Text.Encoding eutf8 = System.Text.Encoding.UTF8;
            byte[] byPostData = eutf8.GetBytes(postdata);
            wbPaypal.Navigate(url, "_top", byPostData, "Content-Type: application/x-www-form-urlencoded\r\n");
            }
        }
    }