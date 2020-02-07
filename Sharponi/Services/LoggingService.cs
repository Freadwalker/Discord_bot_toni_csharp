using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sharponi.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;

        private string logDirectory { get; }
        private string logFile => Path.Combine(logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");

        // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
        public LoggingService(DiscordSocketClient discord, CommandService commands)
        {
            logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            this.discord = discord;
            this.commands = commands;

            this.discord.Log += OnLogAsync;
            this.commands.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            if (!Directory.Exists(logDirectory))     // Create the log directory if it doesn't exist
            {
                Directory.CreateDirectory(logDirectory);
            }

            if (!File.Exists(logFile))               // Create today's log file if it doesn't exist
            {
                File.Create(logFile).Dispose();
            }

            string logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            File.AppendAllText(logFile, logText + "\n");     // Write the log text to a file

            return Console.Out.WriteLineAsync(logText);       // Write the log text to the console
        }
    }
}
