using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Reflection;

namespace Sharponi.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;
        private readonly IConfiguration config;
        private readonly IServiceProvider provider;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            IConfiguration config,
            IServiceProvider provider)
        {
            this.discord = discord;
            this.commands = commands;
            this.config = config;
            this.provider = provider;

            this.discord.MessageReceived += OnMessageReceivedAsync;
        }

        public async Task Init() {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;     // Ensure the message is from a user/bot
            if (msg == null) return;
            if (msg.Author.Id == discord.CurrentUser.Id) return;     // Ignore self when checking commands

            var context = new SocketCommandContext(discord, msg);     // Create the command context

            int argPos = 0;     // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(config["prefix"], ref argPos) || msg.HasMentionPrefix(discord.CurrentUser, ref argPos))
            {
                var result = await commands.ExecuteAsync(context, argPos, provider);     // Execute the command

                if (!result.IsSuccess)     // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}