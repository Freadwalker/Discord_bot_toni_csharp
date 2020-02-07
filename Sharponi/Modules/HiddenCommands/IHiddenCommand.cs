using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Sharponi.Attributes;

namespace Sharponi.Modules.HiddenCommands
{
    [HiddenCommand]
    interface IHiddenCommand
    {
        bool FulfillsCondition(SocketUserMessage msg, SocketCommandContext context, IServiceProvider provider);
        Task Execute(SocketUserMessage msg, SocketCommandContext context, IServiceProvider provider);
    }
}
