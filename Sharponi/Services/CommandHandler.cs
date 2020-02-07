using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Sharponi.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;
        private readonly HiddenCommandHandler hiddenCommandHandler;
        private readonly IConfiguration config;
        private readonly IServiceProvider provider;
        private readonly ILogger<CommandHandler> logger;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            HiddenCommandHandler hiddenCommandHandler,
            IConfiguration config,
            IServiceProvider provider,
            ILogger<CommandHandler> logger)
        {
            this.discord = discord;
            this.commands = commands;
            this.hiddenCommandHandler = hiddenCommandHandler;
            this.config = config;
            this.provider = provider;
            this.logger = logger;

            this.discord.MessageReceived += OnMessageReceivedAsync;
        }

        public async Task Init()
        {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage; // Ensure the message is from a user/bot

            if (msg == null ||
                msg.Author.IsBot || 
                msg.Author.Id == discord.CurrentUser.Id)// Ignore bots
            {
                return;
            }

            var context = new SocketCommandContext(discord, msg); // Create the command context

            try
            {
                int argPos = 0; // Check if the message has a valid command prefix
                if (msg.HasStringPrefix(config[Constants.PrefixKey], ref argPos) ||
                    msg.HasMentionPrefix(discord.CurrentUser, ref argPos))
                {
                    var result = await commands.ExecuteAsync(context, argPos, provider); // Execute the command

                    if (!result.IsSuccess) // If not successful, reply with the error.
                    {
                        logger.LogDebug("a command was misspelled in message: "+msg.Content);
                     //   await context.Channel.SendMessageAsync("I do not have this command! please try again");
                    }
                }
                else
                {
                    //Check for HiddenCommands
                    await hiddenCommandHandler.ExecuteAsync(msg, context, provider);
                }
            }
            catch (Exception e)
            {
                await context.Channel.SendMessageAsync(
                    "A bad Error occured, please contact the Support to check the logs!");
                logger.LogError($"Error on msg: {msg}", e);
            }
        }
    }
}