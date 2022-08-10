using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace APIRequestHandler.APIFetch
{ 
    /// <summary>LoggerClass conatiaining one method.</summary>
    public class IloggerFactory
    {

    /// <summary>Calling this function creates a logger instance for the NEOhandler,
    /// this class should not be used outisde of the NEOhandler</summary>
    /// <returns>An instance of the logger</returns>
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
