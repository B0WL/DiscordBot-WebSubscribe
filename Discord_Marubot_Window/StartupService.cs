using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Discord_Marubot_Window
{

    public class StartupService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
        public StartupService(
            DiscordSocketClient discord,
            CommandService commands
           )
        {
            _discord = discord;
            _commands = commands;
        }

        public async Task StartAsync()
        {

            Properties.Settings settings = Properties.Settings.Default;
            string botToken = settings.BotToken;
            string testToken = settings.TestToken;
            bool isTest = settings.isTest;

            string discordToken = botToken;
            if (isTest)
                discordToken = testToken;


            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `_configuration.json` file found in the applications root directory.");

            await _discord.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
            await _discord.StartAsync();                                // Connect to the websocket

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());     // Load commands and modules into the command service
        }
    }
}