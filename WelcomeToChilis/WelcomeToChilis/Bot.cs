using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.VoiceNext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WelcomeToChilis.Commands;

namespace WelcomeToChilis
{
    public class Bot
    {
        public DiscordClient Client { get; set; }
        public CommandsNextExtension Commands { get; set; }
        public VoiceNextExtension Voice { get; set; }

        public async Task RunAsync()
        {
            var configJson = ConfigManager.GetConfig();

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };


            var endpoint = new ConnectionEndpoint
            {
                Hostname = "127.0.0.1", // From your server configuration.
                Port = 2333 // From your server configuration
            };

            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = "youshallnotpass", // From your server configuration.
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };

          
            Client = new DiscordClient(config);
            var lavalink = Client.UseLavalink();
            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new List<string>() { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false

            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<FunCommands>();
            Voice = Client.UseVoiceNext();
            await Client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            await Task.Delay(-1, new System.Threading.CancellationToken());
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            Console.WriteLine("WelcomeToChilis is online...");
            return Task.CompletedTask;
        }


    }
}
