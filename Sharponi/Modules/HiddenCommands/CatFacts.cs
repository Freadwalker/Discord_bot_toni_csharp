using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Sharponi.Modules.HiddenCommands
{
    internal class CatFacts : IHiddenCommand
    {
        public bool FulfillsCondition(SocketUserMessage msg, SocketCommandContext socketCommandContext, IServiceProvider provider) => msg.Content.ToLower().Contains("cat");

        public Task Execute(SocketUserMessage msg, SocketCommandContext context, IServiceProvider provider)
        {
            return context.Channel.SendMessageAsync("I don't have any stuff about cats yet");
        }
    }
}
