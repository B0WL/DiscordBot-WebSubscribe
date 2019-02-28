using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discord_Marubot_Window
{

    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static botmain bot;
        public unsafe static data DB;
        static Thread botthread;
        public static Winform wf;
        public static Form1 form;

        static List<string> list;
        static List<string> user;
        static List<string> server;

        static DiscordSocketClient discordclient;

        static Thread worker;
        unsafe public static bool tgl_crawl;
        static int loading;
        


        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            bot = new botmain();
            bot.Init();//봇 생성과 초기화


            unsafe
            {
                fixed (bool* fixedtgl = &tgl_crawl)
                    bot.ch.tgl = fixedtgl;
            }


            form = new Form1();
            wf = new Winform();
            DB = new data();

            form.GetDB(DB);

            wf.WinformLog("@이니셜라이징끝");
            bot.ch.wf = wf;
            bot.ch.db = DB;

            discordclient = bot.ch.getdiscord();
            botthread = new Thread(new ThreadStart(working));
            botthread.IsBackground = true;
            botthread.Start();



            dbload();

            tgl_crawl = false;
            loading = 0;

            //worker = new Thread(new ThreadStart(crawling));
            worker = new Thread(new ThreadStart(crawler3));
            worker.Start();

            wf.WinformLog("@여기까지");




            Application.Run(form);
        }


        unsafe static void working()
        {
            bot.StartAsync().GetAwaiter().GetResult();

        }


        static void progress()
        {
            loading++;
            wf.WinformProg(DB);
            if (loading % 10 == 0)
            {
                wf.WinformProg(DB);
                if (loading > 10000)
                {
                    loading = 0;
                }
            }
        }

        static Crawl_HAP crawl = new Crawl_HAP();

        static void crawler3()
        {
            while (true)
            {

                progress();
                Thread.Sleep(60000);

                if (tgl_crawl)
                {

                    #region crawler2

                    List<string> old_list = DB.getoldlist();

                    string old_title = DB.getold()[0];
                    string old_domain = DB.getold()[1];

                    try
                    {
                        crawl.maru_crawl(old_list);
                    }
                    catch
                    {
                        wf.WinformLog("@크롤링 실패");
                        continue;
                    }


                    List<maru> maru = crawl.getlist();

                    if (!(maru.Count > 0))
                    {

                        wf.WinformOld(old_title, old_domain);

                        try
                        {
                            discordclient.SetGameAsync(old_title);
                        }
                        catch
                        {
                            wf.WinformLog("@타이틀 배치 실패");
                        }
                        
                        continue;
                    }


                    string new_title=maru[0].gettitle();
                    string new_domain= maru[0].getdomain();


                    ulong tmp_server;
                    ulong tmp_user;

                    try
                    {
                        discordclient.SetGameAsync(new_title);
                    }
                    catch
                    {
                        wf.WinformLog("@타이틀 배치 실패");
                    }






                    maru.Reverse();
                    for (int i = 0; i < maru.Count; i++)
                    {
                        if (maru[i].gettitle() == old_title)
                            break;

                        new_title = maru[i].gettitle();
                        new_domain = maru[i].getdomain();

                        for (int j = 0; j < list.Count(); j++)
                        {
                            if (new_title.Contains(list[j]))
                            {
                                tmp_server = Convert.ToUInt64(server[j]);
                                tmp_user = Convert.ToUInt64(user[j]);


                                
                                if (discordclient.GetChannel(tmp_server) != null &&
                                    discordclient.GetChannel(tmp_server).GetUser(tmp_user) != null)
                                {
                                    int fail=0;//성공
                                    do
                                    {
                                        try
                                        {

                                            SendDM(discordclient
                                                .GetChannel(tmp_server)
                                                .GetUser(tmp_user),
                                                    new_title, Properties.Settings.Default.URL + new_domain);


                                            wf.WinformLog("@"+ discordclient.GetUser(tmp_user)+"가 "+ new_title + "를 받음\n");
                                        }
                                        catch {
                                            wf.WinformLog("@메세지 전송 실패\n");
                                            fail++;
                                        }
                                        finally
                                        {
                                            fail = 0;
                                        }


                                    } while (0 < fail && fail < 10);//1~최대10회 실패동안 반복
                                    
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                    }
                    ////////////크롤종료후

                    wf.WinformOld(new_title, Properties.Settings.Default.URL + new_domain);
                    DB.setold(new_title, new_domain);
                    DB.setoldlist(crawl.getalllist());
                    DB.filesave();

                    #endregion


                }
            }
        }

        static async void SendDM(SocketUser u, string title, string domain)
        {
            try
            {
                var builder = new EmbedBuilder();
                builder.WithTitle(title);
                builder.WithDescription(domain);

                await (await u.GetOrCreateDMChannelAsync())
                    .SendMessageAsync("",false,builder);
            }
            catch
            {
                wf.WinformLog("@DM 전송실패");
            }
        }
        static void dbload()
        {
            list = DB.getlist();
            user = DB.getuser();
            server = DB.getserver();
            wf.WinformLog("디비 다옮김ㅋㅋ");
        }


    }
}