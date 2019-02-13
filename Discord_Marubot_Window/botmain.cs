using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Marubot_Window
{
    class botmain
    {
        public CommandHandler ch;

        IServiceProvider provider;

        public void Init()
        {
            var services = new ServiceCollection()      // Begin building the service provider
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig     // Add the discord client to the service provider
                {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000     // Tell Discord.Net to cache 1000 messages per channel
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig     // Add the command service to the service provider
                {
                    DefaultRunMode = RunMode.Async,     // Force all commands to run async
                    LogLevel = LogSeverity.Verbose
                }))
                .AddSingleton<CommandHandler>()     // Add remaining services to the provider
                .AddSingleton<LoggingService>()
                .AddSingleton<StartupService>()
                .AddSingleton<Random>();             // You get better random with a single instance than by creating a new one every time you need it


            provider = services.BuildServiceProvider();     // Create the service provider

            provider.GetRequiredService<LoggingService>();      // Initialize the logging service, startup service, and command handler
            ch = provider.GetRequiredService<CommandHandler>();

        }

        public async Task StartAsync()
        {
            await provider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);     // Prevent the application from closing
        }


    }
}