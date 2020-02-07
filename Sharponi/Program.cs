using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharponi.Services;

namespace Sharponi
{
    internal class Program
    {
        // setup our fields we assign later
        private readonly IConfiguration config;
        private DiscordSocketClient client;
        private ILogger logger;

        private static void Main()
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            // create the configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json");

            // build the configuration and assign to _config          
            config = builder.Build();
        }

        public async Task MainAsync()
        {
            // call ConfigureServices to create the ServiceCollection/Provider for passing around the services
            using (var services = ConfigureServices())
            {
                logger = services.GetService<ILogger<Program>>();
                // get the client and assign to client 
                // you get the services via GetRequiredService<T>
                client = services.GetRequiredService<DiscordSocketClient>();

                // setup logging and the ready event
                client.Log += LogAsync;
                client.Ready += ReadyAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                // this is where we get the Token value from the configuration file, and start the bot
                await client.LoginAsync(TokenType.Bot, config[Constants.BotTokenKey]);
                await client.StartAsync();

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await services.GetRequiredService<CommandHandler>().Init();
                services.GetRequiredService<HiddenCommandHandler>().Init();

                await Task.Delay(-1);
            }
        }

        private Task LogAsync(LogMessage log)
        {
            LogLevel logLevel = LoggingService.ConvertLogLevel(log.Severity);
            logger?.Log(logLevel, log.Message, log.Exception);
            return Task.CompletedTask;
        }


        private Task ReadyAsync()
        {
            logger?.LogInformation($"Connected as -> [{client.CurrentUser}] :)");
            return Task.CompletedTask;
        }

        // this method handles the ServiceCollection creation/configuration, and builds out the service provider we can call on later
        private ServiceProvider ConfigureServices()
        {
            // this returns a ServiceProvider that is used later to call for those services
            // we can add types we have access to here, hence adding the new using statement:
            // using csharpi.Services;
            // the config we build is also added, which comes in handy for setting the command prefix!
            return new ServiceCollection()
                .AddSingleton(config).AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {                                       // Add discord to the collection
                    LogLevel = LogSeverity.Debug,     // Tell the logger to give Verbose amount of info
                    MessageCacheSize = 1000             // Cache 1,000 messages per channel
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {                                       // Add the command service to the collection
                    LogLevel = LogSeverity.Debug,     // Tell the logger to give Verbose amount of info
                    DefaultRunMode = RunMode.Async,     // Force all commands to run async by default
                }))
                .AddSingleton<CommandHandler>()
                .AddSingleton<HiddenCommandHandler>()
                .AddLogging(builder => builder.
                                AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LoggingService.LogLevelFromConfiguration(config)) 
                .BuildServiceProvider();
        }
    }
}