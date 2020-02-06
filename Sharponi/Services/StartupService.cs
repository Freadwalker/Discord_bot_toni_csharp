using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sharponi.Services
{
    public class StartupService
    {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;
        private readonly IConfigurationRoot config;

        // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config)
        {
            this.provider = provider;
            this.config = config;
            this.discord = discord;
            this.commands = commands;
        }

        public async Task StartAsync()
        {
            string discordToken = config["tokens:discord"];     // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `_configuration.json` file found in the applications root directory.");

            await discord.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
            await discord.StartAsync();                                // Connect to the websocket

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);     // Load commands and modules into the command service
        }
    }
}
