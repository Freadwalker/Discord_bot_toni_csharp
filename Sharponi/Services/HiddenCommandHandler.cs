using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Sharponi.Modules.HiddenCommands;

namespace Sharponi.Services
{
    public class HiddenCommandHandler
    {
        private readonly IServiceProvider provider;
        private IList<IHiddenCommand> commands;

        public HiddenCommandHandler(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public async Task ExecuteAsync(SocketUserMessage msg, SocketCommandContext context)
        {
            foreach (IHiddenCommand hiddenCommand in commands)
            {
                if (hiddenCommand.FulfillsCondition(msg, context))
                {
                    await hiddenCommand.Execute(msg, context).ConfigureAwait(false);
                }
            }
        }

        public void Init()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                return;
            }

            commands = entryAssembly.GetTypes()
                                    .Where(type => typeof(IHiddenCommand).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                                    .Select(type => (IHiddenCommand)Activator.CreateInstance(type))
                                    .ToList();
        }
    }
}
