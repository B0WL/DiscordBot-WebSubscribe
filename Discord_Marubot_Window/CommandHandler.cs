using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

using Discord_Marubot_Window;

namespace Discord_Marubot_Window
{

    public class CommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        unsafe public bool* tgl;

        public data db;
        public Winform wf;


        public DiscordSocketClient getdiscord()
        {
            return _discord;
        }

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }

        



        unsafe bool gettgl()
        {
            return *tgl;
        }
        unsafe void settgl(bool b)
        {
            *tgl = b;
        }

        string makelist(string user)
        {
            string imsi = "";

            int size = db.list.Count();

            List<string> tmp = new List<string> { };

            int ii = 0;

            for (int i = 0; i < size; i++)
            {
                if (db.user[i] == user)
                {
                    tmp.Add(db.list[i]);
                    ii++;
                }
            }
            tmp.Sort();

            int cut = 1;

            for (int i = 0; i < tmp.Count; i++)
            {
                int length = Encoding.Default.GetByteCount(imsi);
                if ((int)(length / 1800) == cut)
                {
                    imsi += "|";
                    cut++;
                }

                imsi += "\"" + tmp[i] + "\"" + "\r\n";
            }
            imsi = "============"
                + "\r\n" + imsi
                + "============" + "\r\n총 검색어 " + ii.ToString() + "개";

            return imsi;
        }


        /*
        async void CommandList(string result, SocketUserMessage msg, SocketCommandContext context)
        {
            var result2 = result.Split(' ');
            var command = result2[0].Substring(1);
            
            var result3 = "";
            for (int i = 1; i < result2.Length; i++)
            {
                result3 += result2[i];
                if (i != result2.Length - 1)
                    result3 += " ";
            }
            var innerText = result3;

            var author = msg.Author.Id.ToString();
            var server = msg.Channel.Id.ToString();

            string list_txt;
            string[] cut_txt;

            switch (command)
            {
                case "님아":
                    await context.Channel.SendMessageAsync("왜요");
                    break;
                case "크롤":
                    #region 크롤
                    await context.Channel.SendMessageAsync("크롤");
                    if (wf.checkPerm(author))
                    {
                        if (gettgl())
                        {
                            await context.Channel.SendMessageAsync(" 종료");
                            settgl(false);
                        }
                        else if (!gettgl())
                        {
                            await context.Channel.SendMessageAsync(" 시작");
                            settgl(true);
                        }
                    }
                    else
                    {
                        await context.Channel.SendMessageAsync("ㄴ");
                    }
                    break;
                #endregion
                case "추가":
                    #region 추가
                    if (result3 != "" && !result3.Contains("|"))
                    {
                        await context.Channel.SendMessageAsync(
                            "검색어 추가요?\r\n============\r\n");
                        //
                        string[] tmp = result3.Split('\"');
                        string tmp2 = "";
                        for (int i = 0; i < tmp.Length; i++)
                            if (tmp[i] != "\r\n" && tmp[i] != "\n" && tmp[i] != "" && tmp[i].Length < 40)
                                if (!tmp[i].Contains("<@"))
                                {
                                    db.listadd(tmp[i], author, server);
                                    tmp2 += tmp[i] + "\r\n";
                                }
                        await context.Channel.SendMessageAsync(tmp2 +
                            "============\r\n됨");
                        db.filesave();
                        await context.Channel.SendMessageAsync("저장함");

                    }
                    #endregion
                    break;
                case "제거":
                    #region 제거
                    if (result3 != "")
                    {
                        string[] tmp = result3.Split('\"');
                        string tmp2 = "";
                        for (int i = 0; i < tmp.Length; i++)
                            if (tmp[i] != "\r\n" && tmp[i] != "\n" && tmp[i] != "" && tmp[i].Length < 40)
                                if (!tmp[i].Contains("<@"))
                                {
                                    if (db.listfind(tmp[i]) || tmp[i] == "전체")
                                    {
                                        if (db.listdel(tmp[i], author, server))
                                        {
                                            tmp2 += tmp[i] + "\r\n";
                                        }
                                        else
                                            tmp2 += tmp[i] + " 이건 님한테 없음\r\n";
                                    }
                                    else
                                        tmp2 += tmp[i] + "이런거 없음\r\n";

                                }
                        await context.Channel.SendMessageAsync(
                            "검색어 제거요?\r\n============\r\n"
                            + tmp2 +
                            "============\r\n됨");
                        if (result3 != "전체")
                        {
                            db.filesave();
                            await context.Channel.SendMessageAsync("저장함");
                        }
                        else
                        {
                            await context.Channel.SendMessageAsync("저장은 안했음");
                        }
                    }
                    else
                        await context.Channel.SendMessageAsync("뭐요");
                    #endregion
                    break;
                case "강제제거":
                    #region 강제제거

                    if (wf.checkPerm(author))
                        if (result3 != "")
                        {
                            if (db.listfind(result3) || result3 == "전체")
                            {
                                if (db.listdel(result3, "강제", server))
                                {
                                    await context.Channel.SendMessageAsync(result3 + " 지움");

                                }
                                else
                                    await context.Channel.SendMessageAsync("이님한테 그런거 없음;");
                            }
                            else
                                await context.Channel.SendMessageAsync("그런거 없음;");
                        }
                        else
                            await context.Channel.SendMessageAsync("뭐요");
                    #endregion
                    break;
                case "리스트":
                    #region 리스트

                    list_txt = makelist(author);
                    cut_txt = list_txt.Split('|');

                    for (int i = 0; i < cut_txt.Length; i++)
                        await context.Channel.SendMessageAsync(cut_txt[i]);
                    #endregion
                    break;
                case "체크":
                    #region 체크

                    if (result3 != "")
                    {
                        await context.Channel.SendMessageAsync("체크요?");
                        var list = db.list;
                        string sum = "";
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (db.user[i] == author)
                                if (result3.Contains(list[i]))
                                {
                                    sum += "\"" + list[i] + "\"" + "\r\n";
                                }
                        }

                        sum = "============"
                            + "\r\n" + sum
                            + "============" + "\r\n이것들 걸림";

                        await context.Channel.SendMessageAsync(sum);
                    }
                    #endregion
                    break;
                case "검색":
                    #region 검색
                    if (result3 != "")
                    {
                        await context.Channel.SendMessageAsync("검색요?");
                        var list = db.list;
                        string sum = "";
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (db.user[i] == author)
                                if (list[i].Contains(result3))
                                {
                                    sum += "\"" + list[i] + "\"" + "\r\n";
                                }
                        }

                        sum = "============"
                            + "\r\n" + sum
                            + "============" + "\r\n이렇게 있네요";

                        await context.Channel.SendMessageAsync(sum);
                    }
                    #endregion
                    break;
                case "엿보기":
                    #region 엿보기
                    if (result3 != "")
                    {

                        list_txt = makelist(result3);
                        cut_txt = list_txt.Split('|');

                        for (int i = 0; i < cut_txt.Length; i++)
                            await context.Channel.SendMessageAsync(cut_txt[i]);
                    }
                    #endregion
                    break;
                case "저장":
                    #region 저장
                    if (wf.checkPerm(author))
                    {
                        db.filesave();
                        await context.Channel.SendMessageAsync("저장 완료");
                    }
                    else
                    {
                        await context.Channel.SendMessageAsync("ㄴ");
                    }
                    #endregion
                    break;
                case "뭐봄":
                case "머봄":
                case "뭐함":
                case "머함":
                case "모함":
                case "머해":
                case "뭐해":
                case "모해":
                    #region
                    var domain = Program.form.domainbox.Text;

                    var dir = Directory.GetCurrentDirectory() + @"\data";
                    Crawl_HAP crawler = new Crawl_HAP();
                    WebClient client = new WebClient();
                    
                    string txt = "";
                    txt += wf.WinformGetOld();

                    await context.Channel.SendMessageAsync(txt);
                    #endregion
                    break;
            }
        }
        */


        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;     // Ensure the message is from a user/bot
            if (msg == null) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return;     // Ignore self when checking commands

            var context = new SocketCommandContext(_discord, msg);     // Create the command context

            int argPos = 0;     // Check if the message has a valid command prefix
            if (msg.HasStringPrefix("!", ref argPos)
                || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = msg.Content.ToString();




                var result2 = result.Split(' ');
                var command = result2[0].Substring(1);

                var result3 = "";
                for (int i = 1; i < result2.Length; i++)
                {
                    result3 += result2[i];
                    if (i != result2.Length - 1)
                        result3 += " ";
                }
                var innerText = result3;

                var author = msg.Author.Id.ToString();
                var server = msg.Channel.Id.ToString();

                string list_txt;
                string[] cut_txt;
                

                switch (command)
                {
                    case "님아":
                        await context.Channel.SendMessageAsync("왜요");
                        break;
                    case "크롤":
                        #region 크롤
                        await context.Channel.SendMessageAsync("크롤");
                        if (wf.checkPerm(author))
                        {
                            if (gettgl())
                            {
                                await context.Channel.SendMessageAsync(" 종료");
                                settgl(false);
                            }
                            else if (!gettgl())
                            {
                                await context.Channel.SendMessageAsync(" 시작");
                                settgl(true);
                            }
                        }
                        else
                        {
                            await context.Channel.SendMessageAsync("ㄴ");
                        }
                        break;
                    #endregion
                    case "추가":
                        #region 추가
                        if (result3 != "" && !result3.Contains("|"))
                        {
                            await context.Channel.SendMessageAsync(
                                "검색어 추가요?\r\n============\r\n");
                            //
                            string[] tmp = result3.Split('\"');
                            string tmp2 = "";
                            for (int i = 0; i < tmp.Length; i++)
                                if (tmp[i] != "\r\n" && tmp[i] != "\n" && tmp[i] != "" && tmp[i].Length < 40)
                                    if (!tmp[i].Contains("<@"))
                                    {
                                        db.listadd(tmp[i], author, server);
                                        tmp2 += tmp[i] + "\r\n";
                                    }
                            await context.Channel.SendMessageAsync(tmp2 +
                                "============\r\n됨");
                            db.filesave();
                            await context.Channel.SendMessageAsync("저장함");

                        }
                        #endregion
                        break;
                    case "제거":
                        #region 제거
                        if (result3 != "")
                        {
                            string[] tmp = result3.Split('\"');
                            string tmp2 = "";
                            for (int i = 0; i < tmp.Length; i++)
                                if (tmp[i] != "\r\n" && tmp[i] != "\n" && tmp[i] != "" && tmp[i].Length < 40)
                                    if (!tmp[i].Contains("<@"))
                                    {
                                        if (db.listfind(tmp[i]) || tmp[i] == "전체")
                                        {
                                            if (db.listdel(tmp[i], author, server))
                                            {
                                                tmp2 += tmp[i] + "\r\n";
                                            }
                                            else
                                                tmp2 += tmp[i] + " 이건 님한테 없음\r\n";
                                        }
                                        else
                                            tmp2 += tmp[i] + "이런거 없음\r\n";

                                    }
                            await context.Channel.SendMessageAsync(
                                "검색어 제거요?\r\n============\r\n"
                                + tmp2 +
                                "============\r\n됨");
                            if (result3 != "전체")
                            {
                                db.filesave();
                                await context.Channel.SendMessageAsync("저장함");
                            }
                            else
                            {
                                await context.Channel.SendMessageAsync("저장은 안했음");
                            }
                        }
                        else
                            await context.Channel.SendMessageAsync("뭐요");
                        #endregion
                        break;
                    case "강제제거":
                        #region 강제제거

                        if (wf.checkPerm(author))
                            if (result3 != "")
                            {
                                if (db.listfind(result3) || result3 == "전체")
                                {
                                    if (db.listdel(result3, "강제", server))
                                    {
                                        await context.Channel.SendMessageAsync(result3 + " 지움");

                                    }
                                    else
                                        await context.Channel.SendMessageAsync("이님한테 그런거 없음;");
                                }
                                else
                                    await context.Channel.SendMessageAsync("그런거 없음;");
                            }
                            else
                                await context.Channel.SendMessageAsync("뭐요");
                        #endregion
                        break;
                    case "리스트":
                        #region 리스트

                        list_txt = makelist(author);
                        cut_txt = list_txt.Split('|');

                        for (int i = 0; i < cut_txt.Length; i++)
                            await context.Channel.SendMessageAsync(cut_txt[i]);
                        #endregion
                        break;
                    case "체크":
                        #region 체크

                        if (result3 != "")
                        {
                            await context.Channel.SendMessageAsync("체크요?");
                            var list = db.list;
                            string sum = "";
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (db.user[i] == author)
                                    if (result3.Contains(list[i]))
                                    {
                                        sum += "\"" + list[i] + "\"" + "\r\n";
                                    }
                            }

                            sum = "============"
                                + "\r\n" + sum
                                + "============" + "\r\n이것들 걸림";

                            await context.Channel.SendMessageAsync(sum);
                        }
                        #endregion
                        break;
                    case "검색":
                        #region 검색
                        if (result3 != "")
                        {
                            await context.Channel.SendMessageAsync("검색요?");
                            var list = db.list;
                            string sum = "";
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (db.user[i] == author)
                                    if (list[i].Contains(result3))
                                    {
                                        sum += "\"" + list[i] + "\"" + "\r\n";
                                    }
                            }

                            sum = "============"
                                + "\r\n" + sum
                                + "============" + "\r\n이렇게 있네요";

                            await context.Channel.SendMessageAsync(sum);
                        }
                        #endregion
                        break;
                    case "엿보기":
                        #region 엿보기
                        if (result3 != "")
                        {

                            list_txt = makelist(result3);
                            cut_txt = list_txt.Split('|');

                            for (int i = 0; i < cut_txt.Length; i++)
                                await context.Channel.SendMessageAsync(cut_txt[i]);
                        }
                        #endregion
                        break;
                    case "저장":
                        #region 저장
                        if (wf.checkPerm(author))
                        {
                            db.filesave();
                            await context.Channel.SendMessageAsync("저장 완료");
                        }
                        else
                        {
                            await context.Channel.SendMessageAsync("ㄴ");
                        }
                        #endregion
                        break;
                    case "뭐봄":
                    case "머봄":
                    case "뭐함":
                    case "머함":
                    case "모함":
                    case "머해":
                    case "뭐해":
                    case "모해":
                        #region
                        var domain = Program.form.domainbox.Text;

                        var dir = Directory.GetCurrentDirectory() + @"\data";
                        Crawl_HAP crawler = new Crawl_HAP();
                        WebClient client = new WebClient();

                        string txt = "";
                        txt += wf.WinformGetOld();

                        await context.Channel.SendMessageAsync(txt);
                        #endregion
                        break;
                }

            }
        }
    }
}