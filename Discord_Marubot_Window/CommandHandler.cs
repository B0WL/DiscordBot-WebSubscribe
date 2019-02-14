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

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;     // Ensure the message is from a user/bot
            if (msg == null) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return;     // Ignore self when checking commands

            var context = new SocketCommandContext(_discord, msg);     // Create the command context

            int argPos = 0;     // Check if the message has a valid command prefix

            if (msg.Content.Contains("<@545600905823780894>"))
            {
                var text = msg.Content.ToString();

                text = text.Replace("<@545600905823780894> ", "");

                var author = msg.Author.Id.ToString();
                var server = msg.Channel.Id.ToString();

                switch (text)
                {
                    case "츠구":
                        await context.Channel.SendMessageAsync("츠구에요");
                        break;

                    default:
                        await context.Channel.SendMessageAsync("하토바 츠구에요");
                        break;
                }

            }
        }
    }
}