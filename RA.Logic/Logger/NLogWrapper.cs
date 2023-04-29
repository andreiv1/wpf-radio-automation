using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Logic.Logger
{
    public class AppLogger : ILogger
    {
        private readonly NLog.Logger logger;
        public AppLogger()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ConsoleTarget("consoleTarget")
            {
                Layout = "${longdate} ${uppercase:${level}} ${message} ${exception:format=tostring}"
            };

            config.AddTarget(consoleTarget);

            var consoleRule = new LoggingRule("*", LogLevel.Info, consoleTarget);
            config.LoggingRules.Add(consoleRule);

            LogManager.Configuration = config;

            logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            logger.Info(message);
        }
    }
}
