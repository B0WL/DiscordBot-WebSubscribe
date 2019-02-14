using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord_Marubot_Window
{
    class Crawl_HAP
    {
        string title;
        string domain;

        HtmlWeb htmlWeb = new HtmlWeb();

        List<maru> marumaru = new List<maru>();
        List<maru> alllist = new List<maru>();
        
        public Crawl_HAP()
        {
            ServicePointManager.Expect100Continue = true;
            foreach (SecurityProtocolType protocol
                in SecurityProtocolType.GetValues(typeof(SecurityProtocolType)))
            {
                switch (protocol)
                {
                    case SecurityProtocolType.Ssl3:
                    case SecurityProtocolType.Tls:
                    case SecurityProtocolType.Tls11:
                        break;
                    default:
                        ServicePointManager.SecurityProtocol |= protocol;
                        break;
                }
            }


            htmlWeb.PreRequest += request =>
            {
                request.CookieContainer = new CookieContainer();


                return true;
            };
        }

        
        string showmeurl = "https://mangashow.me";
        string updateurl = "/bbs/board.php?bo_table=msm_manga";
        Properties.Settings settings = Properties.Settings.Default;

        public void maru_crawl(List<string> old_list)
        {
            showmeurl = settings.URL;
            var htmlDoc = htmlWeb.Load(showmeurl+ updateurl);

            HtmlNodeCollection nodeCol = htmlDoc.DocumentNode.SelectNodes("//div[@class='data-container']");
            foreach (HtmlNode node in nodeCol)
            {
                var title_tmp = node.SelectNodes(".//div[@class='subject']");
                title = title_tmp[0].InnerText.Replace("\n", "|").Split('|')[1].Trim();
                var domain_tmp = node.SelectNodes(".//a");
                domain = showmeurl + domain_tmp[0].Attributes["href"].Value;


                alllist.Add(new maru(title, domain));
                if (!old_list.Contains(domain))
                    marumaru.Add(new maru(title, domain));
            }
        }
        

        public void maru_img(string url)
        {
            var htmlDoc = htmlWeb.Load(url);
            HtmlNodeCollection nodeCol = htmlDoc.DocumentNode.SelectNodes("//div[@class='ctt_box']//img[@src]");


            foreach (HtmlNode node in nodeCol)
            {
                string tmp = node.Attributes["src"].Value;
                var dir = Directory.GetCurrentDirectory() + @"\data";

                WebClient client = new WebClient();
               
                try
                {
                    client.Headers.Add("User-Agent: Other");   
                    client.DownloadFile(new Uri(tmp), dir + "\\tmp.png");
                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }


        public string[] getcrawled()
        {
            string[] crawled = new string[2];

            crawled[0] = title;
            crawled[1] = domain;
            return crawled;
        }
        public List<maru> getlist()
        {
            return this.marumaru;
        }
        public List<maru> getalllist()
        {
            return this.alllist;
        }
        



    }
}
