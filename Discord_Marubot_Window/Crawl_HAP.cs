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
            showmeurl = settings.URL;// Config 파일에서 가져온 url
            var htmlDoc = htmlWeb.Load(showmeurl+ updateurl);

            HtmlNodeCollection nodeCol = htmlDoc.DocumentNode.SelectNodes("//div[@class='data-container']");
            //페이지안에잇는거 다가져오기

            alllist.Clear();//이전 한페이지 리스트
            marumaru.Clear();

            foreach (HtmlNode node in nodeCol)//한마리씩 뜯어봄
            {
                var title_tmp = node.SelectNodes(".//div[@class='subject']");
                title = title_tmp[0].InnerText.Replace("\n", "|").Split('|')[1].Trim();
                //제목가져오는애

                var domain_tmp = node.SelectNodes(".//a");
                domain = domain_tmp[0].Attributes["href"].Value;
                //주소가져오는애


                alllist.Add(new maru(title, domain));
                //일단 갖다넣음

                if (!old_list.Contains(domain))//크롤 이전 리스트에 이도메인 없으면 넣음
                    marumaru.Add(new maru(title, domain));

            }
        }
        

        public void maru_img(string url)//머함했을때 이미지 갖고오는건데 요즘안씀
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
