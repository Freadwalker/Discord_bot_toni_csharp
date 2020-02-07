using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Sharponi.Attributes;

namespace Sharponi.Modules.HiddenCommands
{
    [HiddenCommand]
    internal interface IHiddenCommand
    {
        bool FulfillsCondition(SocketUserMessage msg, SocketCommandContext context, IServiceProvider provider);
        Task Execute(SocketUserMessage msg, SocketCommandContext context, IServiceProvider provider);
    }
}
