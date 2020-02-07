using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Sharponi.Modules.ApiEntities;
using Sharponi.Services;

namespace Sharponi.Modules
{
    [Name("Fun")]
    [Summary("Contains some funny calls")]
    public class FunModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<FunModule> logger;
        private readonly IServiceProvider provider;
        private readonly IConfiguration config;

        public FunModule(ILogger<FunModule> logger, IServiceProvider provider, IConfiguration config)
        {
            this.logger = logger;
            this.provider = provider;
            this.config = config;
        }
        
        [Command("chuck")]
        [Summary("chucknorris joke")]
        public async Task Chuck()
        {
            ChuckFact fact = await SimpleApiCall.Call<ChuckFact>("https://api.chucknorris.io/jokes/random", provider);
            if (fact != null)
            {
                await ReplyAsync(fact.value);
            }
        }
        
        [Command("dumb")]
        [Summary("Tronald dumb")]
        public async Task Dumb()
        {
            JObject fact = await SimpleApiCall.Call("https://api.tronalddump.io/random/quote", provider);
            if (fact != null)
            {
                await ReplyAsync(fact["value"].ToString());
            }
        }
        
        [Command("geek")]
        [Summary("geek Joke")]
        public async Task Geek()
        {
            string fact = await SimpleApiCall.Call<string>("https://geek-jokes.sameerkumar.website/api", provider);
            if (fact != null)
            {
                await ReplyAsync(fact);
            }
        }
        
        [Command("urgent")]
        [Summary("urgent qoutes")]
        public async Task Urgent()
        {
            JObject fact = await SimpleApiCall.Call("https://corporatebs-generator.sameerkumar.website/", provider);
            if (fact != null)
            {
                await ReplyAsync(fact["phrase"].ToString());
            }
        }
        
        [Command("prog")]
        [Summary("prog qoutes")]
        public async Task Prog()
        {
            JObject fact = await SimpleApiCall.Call("https://programming-quotes-api.herokuapp.com/quotes/random", provider);
            if (fact != null)
            {
                await ReplyAsync("> "+fact["en"] +"\n"+fact["author"]);
            }
        }
        
        [Command("dadJoke")]
        [Alias("dj")]
        [Summary("dad joke")]
        public async Task DadJoke()
        {
            JObject fact = await SimpleApiCall.Call("https://icanhazdadjoke.com/", provider);
            if (fact != null)
            {
                await ReplyAsync(fact["joke"].ToString());
            }
        }
        
        [Command("advice")]
        [Summary("give you an advice")]
        public async Task Advice()
        {
            JObject fact = await SimpleApiCall.Call("https://api.adviceslip.com/advice", provider);
            if (fact != null)
            {
                await ReplyAsync(fact["slip"]["advice"].ToString());
            }
        }
        
        [Command("gif")]
        [Summary("searches for a gif")]
        public async Task Gif(string term)
        {
            JObject fact = await SimpleApiCall.Call(GiphyUrl(term), provider);
            if (fact != null)
            {
                await ReplyAsync(fact["data"]["images"]["fixed_height"]["url"].ToString());
            }
        }
        
        [Command("cat")]
        [Summary("searches a cat gif")]
        public async Task Cat()
        {
            await Gif("cat");
        }
        
        [Command("idiot")]
        [Summary("shows an idiot")]
        public async Task Idiot()
        {
            await Gif("donald trump");
        }
        
        [Command("joke")]
        [Summary("a random joke")]
        public async Task Joke()
        {
            JObject fact = await SimpleApiCall.Call(JokeUrl("any"), provider);
            if (fact != null)
            {
                await SendTwoPartJoke(fact);
            }
        }
        
        [Command("programmerJoke")]
        [Alias("jp")]
        [Summary("a programmer joke")]
        public async Task ProgrammerJoke()
        {
            JObject fact = await SimpleApiCall.Call(JokeUrl("Programming"), provider);
            if (fact != null)
            {
                await SendTwoPartJoke(fact);
            }
        }

        [Command("miscellaneousJoke")]
        [Alias("jm")]
        [Summary("a miscellaneous joke")]
        public async Task MiscellaneousJoke()
        {
            JObject fact = await SimpleApiCall.Call(JokeUrl("Miscellaneous"), provider);
            if (fact != null)
            {
                await SendTwoPartJoke(fact);
            }
        }

        [Command("darkJoke")]
        [Alias("jd")]
        [Summary("a dark joke")]
        public async Task DarkJoke()
        {
            JObject fact = await SimpleApiCall.Call(JokeUrl("Dark"), provider);
            if (fact != null)
            {
                await SendTwoPartJoke(fact);
            }
        }

        [Command("egal")]
        [Summary("the wendler guy")]
        public async Task Egal()
        {
            await ReplyAsync("https://tenor.com/view/egal-singing-ocean-sea-boat-ride-gif-16080257");
        }

        private async Task SendTwoPartJoke(JObject fact)
        {
            if (fact["setup"] != null && fact["delivery"] != null)
            {
                await ReplyAsync(fact["setup"] + "\n> " + fact["delivery"]);
            }
            else
            {
                await ReplyAsync(fact["joke"].ToString());
            }
        }

        private string GiphyUrl(string term)
        {
            return $"http://api.giphy.com/v1/gifs/random?tag={term}&api_key={config[Constants.GiphyKey]}";
        }

        private string JokeUrl(string category)
        {
            return "https://sv443.net/jokeapi/category/" + category;
        }
    }
}