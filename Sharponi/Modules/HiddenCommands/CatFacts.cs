using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sharponi.Services;

namespace Sharponi.Modules.HiddenCommands
{
    internal class CatFacts : IHiddenCommand
    {
        public bool FulfillsCondition(SocketUserMessage msg,
                                      SocketCommandContext socketCommandContext,
                                      IServiceProvider provider) => msg.Content.ToLower().Contains("cat");

        public async Task Execute(SocketUserMessage msg, SocketCommandContext context, IServiceProvider provider)
        {
            CatFact fact = await SimpleApiCall.Call<CatFact>("https://catfact.ninja/fact", provider);
            if (fact != null)
            {
                await context.Channel.SendMessageAsync(fact.fact);
            }
        }

        internal class CatFact
        {
            public string fact { get; set; }
            public int length { get; set; }
        }
    }
}
