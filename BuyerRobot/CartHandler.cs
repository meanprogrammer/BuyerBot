using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BuyerRobot
{
    public class CartHandler
    {
        public void AddToCart()
        {
            string size = "9.0";
            string url = "http://www.footlocker.com/product/model:234227/sku:05329600/jordan-retro-1-low-og-mens/white/red/?cm=";
            GZipWebClient wc = new GZipWebClient();
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT x.y; rv:10.0) Gecko/20100101 Firefox/10.0");//"Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:46.0) Gecko/20100101 Firefox/46.0");
            wc.Headers.Add("DNT","1");
            wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            wc.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            wc.Headers.Add("Accept-Language", "en-US,en;q=0.8,da;q=0.6");
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            var responseUrl = string.Empty;
            string content = HttpGetRequest(url, out responseUrl); //wc.DownloadString(url);
            
           
            var doc = new HtmlDocument();   
            doc.LoadHtml(content);

            HtmlNode pdp_selectedSKU = doc.GetElementbyId("pdp_selectedSKU");
            var realSkuValue = pdp_selectedSKU.GetAttributeValue("value", string.Empty);
            HtmlNode pdp_model = doc.GetElementbyId("pdp_model");
            var realModelValue = pdp_model.GetAttributeValue("value", string.Empty);
            HtmlNode requestKey = doc.GetElementbyId("requestKey");
            var realRequestKeyValue = requestKey.GetAttributeValue("value", string.Empty);

            Dictionary<string, string> payload = new Dictionary<string, string>();
            payload.Add("BV_TrackingTag_QA_Display_Sort", string.Empty);
            payload.Add("BV_TrackingTag_Review_Display_Sort",string.Format("http://footlocker.ugc.bazaarvoice.com/8001/{0}/reviews.djs?format=embeddedhtml", realSkuValue));
            payload.Add("coreMetricsCategory", "blank");
            payload.Add("fulfillmentType", "SHIP_TO_HOME");
            payload.Add("hasXYPromo", "false");
            payload.Add("inlineAddToCart", "0,1");
            payload.Add("qty", "1");
            payload.Add("rdo_deliveryMethod", "shiptohome");
            payload.Add("requestKey", realRequestKeyValue);
            payload.Add("size", "10.0");
            payload.Add("sku", realSkuValue);
            payload.Add("storeCostOfGoods", "0.00");
            payload.Add("storeNumber", "00000");
            payload.Add("the_model_nbr", realModelValue);


            //wc.Headers.Clear();
            //wc.ResponseHeaders.Add("Accept", "*/*");
            /*
            wc.ResponseHeaders.Add("Origin", "http://www.footlocker.com");
            wc.ResponseHeaders.Add("X-Requested-With", "XMLHttpRequest");
            wc.ResponseHeaders.Add("Referer", wc.BaseAddress);
            wc.ResponseHeaders.Add("Accept-Encoding", "gzip, deflate");
            */

            //byte[] response = wc.UploadValues("http://www.footlocker.com/catalog/miniAddToCart.cfm?secure=0&", "POST", payload);
            //string result = System.Text.Encoding.UTF8.GetString(response);

            string result = HttpPostRequest("https://www.footlocker.com/catalog/miniAddToCart.cfm?secure=0&", payload, responseUrl);


            Console.WriteLine(content);
        }


        private string HttpPostRequest(string url, Dictionary<string, string> postParameters, string referer)
        {
            string postData = "";

            foreach (string key in postParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(postParameters[key]) + "&";
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


            byte[] data = Encoding.ASCII.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Accept = "*/*";
            request.Headers.Add("Origin", "http://www.footlocker.com");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Referer = referer;
            request.Headers.Add("Accept-Encoding", "gzip, deflate");


            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)request.GetResponse();

            Stream responseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            string pageContent = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();

            myHttpWebResponse.Close();

            return pageContent;
        }


        private string HttpGetRequest(string url, out string responseUrl)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT x.y; rv:10.0) Gecko/20100101 Firefox/10.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add("DNT", "1");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8,da;q=0.6");
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            responseUrl = request.Address.AbsoluteUri;
            Stream responseStream = response.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            string pageContent = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            responseStream.Close();

            response.Close();

            return pageContent;
        }

        /*
         *  Dictionary<string, string> payload = new Dictionary<string, string>();
            payload.Add("BV_TrackingTag_QA_Display_Sort", string.Empty);
            payload.Add("BV_TrackingTag_Review_Display_Sort",string.Format("http://footlocker.ugc.bazaarvoice.com/8001/{0}/reviews.djs?format=embeddedhtml", realSkuValue));
            payload.Add("coreMetricsCategory", "blank");
            payload.Add("fulfillmentType", "SHIP_TO_HOME");
            payload.Add("hasXYPromo", "false");
            payload.Add("inlineAddToCart", "0,1");
            payload.Add("qty", "1");
            payload.Add("rdo_deliveryMethod", "shiptohome");
            payload.Add("requestKey", realRequestKeyValue);
            payload.Add("size", "10.0");
            payload.Add("sku", realSkuValue);
            payload.Add("storeCostOfGoods", "0.00");
            payload.Add("storeNumber", "00000");
            payload.Add("the_model_nbr", realModelValue);
         * */
    }
}
