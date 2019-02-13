using Discord;
using Discord.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Discord_Marubot_Window
{
    class Robot
    {
        DiscordClient discord;
        data DataBase;
        List<string> list;
        List<string> user;
        List<string> server;
        Form1 form;
        Thread worker;
        Winform wf;
        string token;

        bool tgl_crawl = false;
        


        int loading = 0;
        int maximum = 10000;

        public data getdatabase()
        {
            return this.DataBase;
        }

        public DiscordClient getdiscord()
        {
            return this.discord;
        }

        private void crawler2()
        {
            
            Crawl_HAP crawl = new Crawl_HAP();
            List<string> old_list = DataBase.getoldlist();

            try
            {
                crawl.maru_crawl(old_list);
            }
            catch
            {
                wf.WinformLog("@크롤실패");
                return;
            }
            List<maru> maru = crawl.getlist();
            if (!(maru.Count > 0))
                return;
            wf.WinformLog("@크롤");


            string new_title = maru[0].gettitle();
            string new_domain = maru[0].getdomain();
            
            string old_title = DataBase.getold()[0];
            string old_domain = DataBase.getold()[1];
            
            

            ulong tmp_server;
            ulong tmp_user;

            for (int i = 0; i < maru.Count; i++)
            {
                if(maru[i].gettitle() == old_title)
                    break;

                for (int j = 0; j < list.Count(); j++)
                {
                    if (maru[i].gettitle().Contains(list[j]))
                    {
                        tmp_server = Convert.ToUInt64(server[j]);
                        tmp_user = Convert.ToUInt64(user[j]);
                        try
                        {
                            if (discord.GetServer(tmp_server) != null &&
                                discord.GetServer(tmp_server).GetUser(tmp_user) != null)
                            {
                                discord
                                    .GetServer(tmp_server)
                                    .GetUser(tmp_user)
                                    .SendMessage(maru[i].getdomain());
                            }


                        }
                        catch
                        {
                            wf.WinformLog("@메세지 전송 실패");
                            return;
                        }
                    }
                }
            }

            ////////////크롤종료후

            DataBase.setold(new_title, new_domain);

            DataBase.setoldlist(crawl.getalllist());

            wf.WinformOld(new_title, new_domain);
            try
            {
                discord.SetGame(new_title);
            }
            catch
            {

            }
        }
        /*
        private void crawler()
        {
            Crawl_HAP crawl = new Crawl_HAP();

            try
            {
                crawl.maru_crawl();
            }
            catch
            {
                wf.WinformLog("@크롤실패");
                return;
            }
            
            string[] crawled = crawl.getcrawled();
            List<maru> maru = crawl.getlist();

            string new_title = crawled[0];
            string new_domain = crawled[1];
            string writer = crawled[2];

            string[] olded = DataBase.getold();
            string old_title = olded[0];
            string old_domain = olded[1];

            ulong tmp_server;
            ulong tmp_user;

            bool equal = old_domain == new_domain;

            try
            {
                discord.SetGame(new_title);
            }
            catch
            {

            }

            if (!equal)
                for (int i = 0; i < list.Count(); i++)
                    if (new_title.Contains(list[i]))
                    {
                        tmp_server = Convert.ToUInt64(server[i]);
                        tmp_user = Convert.ToUInt64(user[i]);

                        if (discord.GetServer(tmp_server) != null)
                            if (discord.GetServer(tmp_server).GetUser(tmp_user) != null)
                            {
                                try
                                {
                                    discord
                                        .GetServer(tmp_server)
                                        .GetUser(tmp_user)
                                        .SendMessage(new_domain);
                                }
                                catch
                                {
                                    wf.WinformLog("@메세지 전송 실패");
                                    return;
                                }
                            }
                    }

            DataBase.setold(new_title, new_domain);
            wf.WinformOld(new_title, new_domain);

            loading++;
            if (loading % 10 == 0)
            {
                wf.WinformProg(DataBase);
                if (loading > maximum)
                    loading = 0;
            }

        }
        */

        private void crawling()
        {
            while (true)
            {
                if (tgl_crawl)
                {
                    Thread working = new Thread(new ThreadStart(crawler2));
                    working.Start();
                    working.Join(100000);

                    loading++;
                    if (loading % 10 == 0)
                    {
                        wf.WinformProg(DataBase);
                        if (loading > maximum)
                            loading = 0;
                    }
                }
                Thread.Sleep(5000);
            }
        }
        public Robot(string Token, Form1 form)
        {
            this.token = Token;
            this.form = form;
            wf = form.getWF();
            worker = new Thread(new ThreadStart(crawling));
            worker.Start();

            DataBase = form.getDB();
            dbload();
            wf.WinformOld(DataBase.getold()[0], DataBase.getold()[1]);


            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });
            var commands = discord.GetService<CommandService>();

            ////////////////////////////////////////////////////////////////////
            /*
            commands.CreateCommand("여기").Do(async (e) =>
            {
                DataBase.setchan(e.Channel.Id,e.User.Id.ToString());
                string imsi = File.ReadAllText(DataBase.getdirection() + @"\" + e.User.Id.ToString() + ".txt");
                Channel here = discord.GetChannel(Convert.ToUInt64(imsi));
                await here.SendMessage(
                    here.ToString() + "여기 있으면 됨?");
            });
            */

            commands.CreateCommand("크롤").Do(async (e) =>
            {
                if (wf.checkPerm(e.User.Id.ToString()))
                    if (tgl_crawl)
                    {
                        //Console.WriteLine("@크롤" + crawlonline);
                        tgl_crawl = false;
                        wf.WinformLog("@크롤" + tgl_crawl);
                        discord.SetGame(null);
                        await e.Channel.SendMessage("크롤 종료");
                    }
                    else
                    {
                        //Console.WriteLine("@크롤"+crawlonline);
                        tgl_crawl = true;
                        wf.WinformLog("@크롤" + tgl_crawl);
                        await e.Channel.SendMessage("크롤 시작");
                    }
                else
                {
                    await e.Channel.SendMessage("s");
                }
            });

            commands.CreateCommand("짭비님")
                .Alias(new string[] { "짭비", "님아" })
                .Do(async (e) =>
                {
                    //Console.WriteLine("@짭비님");
                    wf.WinformLog("@짭비님");

                    await e.Channel.SendMessage($"{e.User.Name}님 왜요");
                    await e.Channel.SendMessage($"{e.User.Mention}");
                    await e.Channel.SendMessage("");
                });

            commands.CreateCommand("뭐함")
                .Alias(new string[] { "뭐해", "뭐해요","뭐해여","머해","머해요","머해여","머함" })
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage($"{discord.CurrentGame.Name} 이거 봄");
                });

            commands.CreateCommand("추가")
                .Parameter("this", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string msg = e.GetArg("this");
                    if (msg != null)
                    {
                        await e.Channel.SendMessage("검색어 추가요?");
                        string[] tmp = msg.Split('\"');
                        string tmp2 = "";
                        for (int i = 0; i < tmp.Length; i++)
                            if (tmp[i] != "\r\n" && tmp[i] != "\n" && tmp[i] != "" && tmp[i].Length < 25)
                                if (!tmp[i].Contains("<@"))
                                {
                                    DataBase.listadd(tmp[i], e.User.Id.ToString(), e.Server.Id.ToString());
                                    tmp2 += tmp[i] + "\r\n";
                                }
                        await e.Channel.SendMessage("============" + "\r\n" + tmp2 + "============" + "\r\n" + "됨");
                        dbload();
                    }
                    else
                        await e.Channel.SendMessage("뭐요");
                });
            commands.CreateCommand("제거")
                .Alias(new string[] { "삭제" })
                .Parameter("this", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string msg = e.GetArg("this");
                    if (msg != null)
                    {
                        await e.Channel.SendMessage(msg + "요?");
                        if (DataBase.listfind(msg) || msg == "전체")
                        {
                            if (DataBase.listdel(msg, e.User.Id.ToString(), e.Server.Id.ToString()))
                            {
                                await e.Channel.SendMessage(msg + "지움");
                                dbload();
                            }
                            else
                                await e.Channel.SendMessage("님한테 그런거 없음;");
                        }
                        else
                            await e.Channel.SendMessage("그런거 없음;");
                    }
                    else
                        await e.Channel.SendMessage("뭐요");
                });
            commands.CreateCommand("저장")
                .Alias(new string[] { "세이브" })
                .Do(async (e) =>
                {
                    if (wf.checkPerm(e.User.Id.ToString()))
                    {
                        await e.Channel.SendMessage("db 저장하면됨?" + "\n" + "알씀");
                        DataBase.filesave();
                    }
                    else
                    {
                        await e.Channel.SendMessage("싫음");
                    }
                });
            commands.CreateCommand("백업")
                .Do(async (e) =>
                {
                    if (wf.checkPerm(e.User.Id.ToString()))
                    {
                        await e.Channel.SendMessage("db 백업하면됨?" + "\n" + "알씀");
                        DataBase.backup();
                    }
                    else
                    {
                        await e.Channel.SendMessage("귀찮음");
                    }
                });
            commands.CreateCommand("불러오기")
                .Alias(new string[] { "로드" })
                .Do(async (e) =>
                {
                    if (wf.checkPerm(e.User.Id.ToString()))
                    {
                        await e.Channel.SendMessage("db 불러오면됨?" + "\n" + "알씀");
                        DataBase.fileload();
                        dbload();
                        await e.Channel.SendMessage("됐다");
                    }
                    else
                    {
                        await e.Channel.SendMessage("ㄴ");
                    }
                });
            commands.CreateCommand("리스트")
                .Alias(new string[] { "목록", "검색어" })
                .Do(async (e) =>
                {
                    await e.Channel
                    .SendMessage("검색어요?"
                    + "\n" + "알씀");


                    await e.Channel.SendMessage(makelist(e.User.Id.ToString()));
                    await e.Channel.SendMessage("됐다");
                });
            commands.CreateCommand("퍼미션")
                .Alias(new string[] { "목록", "검색어" })
                .Do(async (e) =>
                {
                    await e.Channel
                    .SendMessage("퍼미션요?"
                    + "\n" + "알씀");
                    List<string> perm = wf.getpermlist();
                    string imsi = "";

                    int size = perm.Count();
                    for (int i = 0; i < size; i++)
                    {
                        imsi += "\"" + perm[i] + "\"" + "\r\n";
                    }
                    imsi = "============"
                        + "\r\n" + imsi
                        + "============";
                    await e.Channel.SendMessage(imsi);
                    await e.Channel.SendMessage("됐다");
                });
            commands.CreateCommand("퍼미션추가")
                .Parameter("this", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string msg = e.GetArg("this");
                    if (wf.checkPerm(e.User.Id.ToString()))
                    {
                        wf.WinformPermadd(msg);
                        await e.Channel.SendMessage(msg + " 추가함");
                    }
                    else
                    {
                        await e.Channel.SendMessage("ㅈㅅ");
                    }
                });
            commands.CreateCommand("퍼미션제거")
                .Parameter("this", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string msg = e.GetArg("this");
                    if (wf.checkPerm(e.User.Id.ToString()))
                    {
                        if (wf.WinformPermfind(msg) != -1)
                        {
                            wf.WinformPermremove(msg);
                            await e.Channel.SendMessage(msg + " 제거");
                        }
                        else
                        {
                            await e.Channel.SendMessage("그런거없음;");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("ㅈㅅ");
                    }
                });
            commands.CreateCommand("유저제거")
                .Parameter("this", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string msg = e.GetArg("this");

                    if (msg != null)
                    {
                        await e.Channel.SendMessage(
                            e.Channel.GetUser(Convert.ToUInt64(msg)) + "거요?");
                        if (wf.checkPerm(e.User.Id.ToString()))
                        {
                            if (DataBase.listdel("전체", msg, e.Server.Id.ToString()))
                            {
                                await e.Channel.SendMessage(
                                    e.Channel.GetUser(Convert.ToUInt64(msg)) + "거지움");
                                dbload();
                            }
                            else
                                await e.Channel.SendMessage("ㄴㄴㄴ");
                        }
                        else
                            await e.Channel.SendMessage("안됨;");
                    }
                    else
                        await e.Channel.SendMessage("뭐요");
                });
            commands.CreateCommand("서버제거")
                .Parameter("this", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string msg = e.GetArg("this");

                    if (msg != null)
                    {
                        await e.Channel.SendMessage(
                            discord.GetServer(Convert.ToUInt64(msg)) + "여기요?");
                        if (wf.checkPerm(e.User.Id.ToString()))
                        {
                            if (DataBase.listdel("전체", "전체", msg))
                            {
                                await e.Channel.SendMessage(
                                    discord.GetServer(Convert.ToUInt64(msg)) + "여기지움");
                                dbload();
                            }
                            else
                                await e.Channel.SendMessage("ㄴㄴㄴ");
                        }
                        else
                            await e.Channel.SendMessage("안됨;");
                    }
                    else
                        await e.Channel.SendMessage("뭐요");
                });
            commands.CreateCommand("엿보기")
                .Parameter("this", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    string msg = e.GetArg("this");
                    IEnumerable<User> UserList = e.Channel.FindUsers(msg);
                    
                    if (UserList.Count() == 0)
                    {
                        await e.Channel
                    .SendMessage(e.Channel.GetUser(Convert.ToUInt64(msg)).Name + "님 검색어요?");
                        await e.Channel.SendMessage(makelist(msg));
                    }
                    else
                    {
                        await e.Channel
                    .SendMessage(UserList.First().Name + "님 검색어요?");
                        await e.Channel.SendMessage(makelist(UserList.First().Id.ToString()));
                    }

                    await e.Channel.SendMessage("됐다");

                });

            commands.CreateCommand("서버목록")
                .Do(async (e) =>
                {
                    IEnumerator<Server> enumer = discord.Servers.GetEnumerator();
                    string tmp = "============\r\n";
                    while (enumer.MoveNext())
                        tmp += "\"[" + enumer.Current + "|" + enumer.Current.Id + "]\"" + "\r\n";
                    await e.Channel.SendMessage(tmp + "============");
                });


            /*
            commands.CreateCommand("일어나")
                .Do(async (e) =>
                {
                    if (wf.checkPerm(e.User.Id.ToString()))
                    {
                        await discord.Disconnect();
                        await discord.Connect(token, TokenType.Bot);
                    }
                 
                });
                */

            ///////////////////////////////////////////////////////////////////

            /////인사하기
            /*
            discord.UserUpdated += async (s, e) => {
                Channel channel = e.Server.FindChannels("lounge", ChannelType.Text).SingleOrDefault();
                if (e.Before.Status == UserStatus.Offline)
                    if (e.After.Status == UserStatus.Online)
                        await channel.SendMessage(e.After.Name + "님 ㅎㅇ");
                if (e.Before.Status == UserStatus.Idle)
                    if (e.After.Status == UserStatus.Online)
                        await channel.SendMessage(e.After.Name + "님 ㅎㅇ");
           };*/

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(
                Token, TokenType.Bot);
                wf.WinformLog("@bot online");
            });

        }


        private void Log(object sender, LogMessageEventArgs e)
        {
            string time = DateTime.Now.ToString("yyyyMMddHHmm");
            string date = DateTime.Now.ToString("yyyyMMdd");
            string tmp;

            tmp = DataBase.getdirection()
                + @"\" + date;
            DirectoryInfo di = new DirectoryInfo(tmp);
            if (!di.Exists)
                di.Create();

            string msg = time + $"[{ e.Severity}|{e.Source}]:{e.Message}" + "\r\n";

            File.AppendAllText(tmp + @"\log.txt", msg);

            wf.WinformLog(msg);

            if (e.Message == "Disconnected")
            {
                wf.WinformLog("@disconnect catch");
                discord.Disconnect();
                discord.Connect(token, TokenType.Bot);
                wf.WinformLog("@reconnect by bot");
            }
        }


        string makelist(string user)
        {
            string imsi = "";
            /*
            foreach (string title in list)
                imsi += "\"" + title + "\"" + "\r\n";
                */
            int size = this.user.Count();

            List<string> tmp = new List<string> { };
            for (int i = 0; i < size; i++)
            {
                if (this.user[i] == user)
                {
                    tmp.Add(list[i]);
                }
            }
            tmp.Sort();

            for (int i = 0; i < tmp.Count; i++)
                imsi += "\"" + tmp[i] + "\"" + "\r\n";
            imsi = "============"
                + "\r\n" + imsi
                + "============";

            return imsi;
        }

        public void dbload()
        {
            list = DataBase.getlist();
            user = DataBase.getuser();
            server = DataBase.getserver();
        }


        //////////////////////////////////////




    }
}
