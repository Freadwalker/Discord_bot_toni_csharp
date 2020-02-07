using Discord;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sharponi.Services
{
    public static class LoggingService
    {
        public static LogLevel ConvertLogLevel(LogSeverity logSeverity)
        {
            switch (logSeverity)
            {
                case LogSeverity.Critical:
                    return LogLevel.Critical;
                case LogSeverity.Error:
                    return LogLevel.Error;
                case LogSeverity.Warning:
                    return LogLevel.Warning;
                case LogSeverity.Info:
                    return LogLevel.Information;
                case LogSeverity.Verbose:
                    return LogLevel.Trace;
                case LogSeverity.Debug:
                    return LogLevel.Debug;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logSeverity), logSeverity, null);
            }
        }

        public static LogLevel LogLevelFromConfiguration(IConfiguration config)
        {
            switch (config[Constants.LogLevelKey]?.ToLower())
            {
                case "debug" : return LogLevel.Debug;
                case "info" : return LogLevel.Information;
                case "warning" : return LogLevel.Warning;
                case "error" : return LogLevel.Error;
                case "all" : return LogLevel.Trace;
                default: return LogLevel.Warning;
            }
        }
    }
}
