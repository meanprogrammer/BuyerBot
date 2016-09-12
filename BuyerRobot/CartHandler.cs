using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            string content = wc.DownloadString(url);
            
           
            var doc = new HtmlDocument();   
            doc.LoadHtml(content);

            HtmlNode pdp_selectedSKU = doc.GetElementbyId("pdp_selectedSKU");
            var realSkuValue = pdp_selectedSKU.GetAttributeValue("value", string.Empty);
            HtmlNode pdp_model = doc.GetElementbyId("pdp_model");
            var realModelValue = pdp_model.GetAttributeValue("value", string.Empty);
            HtmlNode requestKey = doc.GetElementbyId("requestKey");
            var realRequestKeyValue = requestKey.GetAttributeValue("value", string.Empty);

            Console.WriteLine(content);
        }

        public string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }
    }
}
