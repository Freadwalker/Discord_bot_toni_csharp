using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Sharponi.Modules.HiddenCommands
{
    internal class CatFacts : IHiddenCommand
    {
        public bool FulfillsCondition(SocketUserMessage msg, SocketCommandContext context) => msg.Content.ToLower().Contains("cat");

        public Task Execute(SocketUserMessage msg, SocketCommandContext context)
        {
            return context.Channel.SendMessageAsync("I don't have any stuff about cats yet");
        }
    }
}
