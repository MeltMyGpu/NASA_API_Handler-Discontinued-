using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace APIRequestHandler.APIFetch
{
    public class IloggerFactory
    {

        public static ILogger LoggerCreation()
        {
            var loggerFactory = LoggerFactory.Create(
                builder => builder
                            .AddConsole() // adds console as logging target
                            .AddDebug()     // add debug as logging target
                            .SetMinimumLevel(LogLevel.Debug) // set min level to log 
                            );

            var logger = loggerFactory.CreateLogger<NEOHandler>();
            return logger;

        }
    }
}
