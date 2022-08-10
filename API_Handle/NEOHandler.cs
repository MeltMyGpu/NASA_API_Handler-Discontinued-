using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using APIRequestHandler.JsonWrapper;
using APIRequestHandler.APIFetch;

namespace APIRequestHandler
{

    /// <summary>This Class handes all interaction with the API.</summary>
    /// <remarks>
    ///   <para>When using this class, please remember to use the dispose method when you are done.</para>
    ///   <para>
    ///     <font color="#333333">Though this class handles all interaction with the API, the JSON format that the API produces is heavily over compicated, please consider using the NEODataLoader to load the called data into a NeoSimpleWrapper, as this makes it <font color="#333333">much easier to access useful parts of the data.</font></font>
    ///   </para>
    /// </remarks>
    public class NEOHandler
    {
        private readonly Client _Client;
        private NEORootObject? _NEOData;
        private string _APIKey;
        private string? _FeedUrl;
        private readonly Regex _DateFormatCheck;
        public bool Connected { get; private set; }
        public ILogger Logger { get; set; }


        /// <summary>Initializes a new instance of the <see cref="NEOHandler" /> class.</summary>
        /// <param name="key">The API key to be used by the NEOHandler, if you dont have a key, use "DEMO_KEY"</param>
        public NEOHandler(string key)
        {
            _Client = new Client();
            _APIKey = key; 
            _DateFormatCheck = new Regex("^[0-9][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9]$"); 
            // Is here to ensure the dates passed in are in the correct format //

            Logger = IloggerFactory.LoggerCreation();
            Logger.LogInformation(string.Format($"Api key set to {key}"));

            try
            {
                Connected = _Client.ConnectionCheck();
                Logger.LogInformation("Connection check successful;");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ping error, either URL is incorrect or internet is diconnected; ");
                throw;
            }

        }


        /// <summary>Nulls the input check.</summary>
        /// <param name="checkList">
        ///   <para>
        /// An array of string to be checked by this method.</para>
        ///   <para>These should be passed in via a new string array</para>
        /// </param>
        /// <exception cref="System.NullReferenceException">If the string passed in are == null or empty</exception>
        /// <example>NullInputCheck(new string[] {"this", "is", "an exsample"});
        /// <code></code></example>
        private void NullInputCheck(string[] checkList) 
        {
            foreach(string check in checkList)
            {
                if (string.IsNullOrEmpty(check))
                {
                    Logger.LogError(new NullReferenceException(), "Null or empty string passed; ");
                    throw new NullReferenceException();
                }
            }
        }

        /// <summary>Disposes of the HTMLclient</summary>
        /// <remarks>should be called when you're done making API requests.</remarks>
        public void DisposeOfClient()
        {
            _Client.Dispose();
            Logger.LogInformation("HTTP Client disposed; ");
        }

        /// <summary>Can be used to change the active API key if needed.</summary>
        /// <param name="key">A string containing your API key</param>
        /// <remarks>
        ///   <para>
        /// More here as a debugging tool, as its real world use is limited.
        /// </para>
        /// </remarks>
        public void ChangeAPIKey(string key)
        {
            NullInputCheck(new string[] {key});
            _APIKey = key;
            Logger.LogInformation(String.Format($"Api key changed to {key}"));
        }


        /// <summary>Sends a Feed Request to the NEO API, for a 7 day period of data.</summary>
        /// <param name="dateStart">The date start. in format 'YYYY-MM-DD'</param>
        /// <param name="dateEnd">The date end, must be 7 days after the dateStart. in format 'YYYY-MM-DD'</param>
        /// <returns>An NEORootObject conatining the requested Information</returns>
        /// <exception cref="System.FormatException">If the format of the date is incorrect.</exception>
        public async Task<NEORootObject> GetNEOData(string dateStart, string dateEnd)
        {
            NullInputCheck(new string[] { dateStart, dateEnd });

            if (_DateFormatCheck.IsMatch(dateStart) && _DateFormatCheck.IsMatch(dateEnd))
            {
                _FeedUrl = string.Format($"https://api.nasa.gov/neo/rest/v1/feed?start_date={dateStart}&end_date={dateEnd}&api_key={_APIKey}");

                try
                {
                    _NEOData = await _Client.SendApiFeedRequest(_FeedUrl);
                    Logger.LogInformation("API Fetch was successful");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, " Error Fetching from API");
                    throw;
                }
            }
            else
            {
                Logger.LogError("Incorrect date format", dateEnd + dateEnd);
                throw new FormatException(dateStart + dateEnd);
            }

            
            return _NEOData; 
        }


        /// <summary>Sends a NEO lookup request for a specified near earth object to the API.</summary>
        /// <param name="neoReferenceId">The near earth object identifier.</param>
        /// <returns>Returns an Observation object containing all information about the request NEO.</returns>
        /// <remarks>
        /// This API request returns a much larger amount of information about an NEO than the Feed request does, as it conatians all the information about that object, with no time limit on the data.
        /// </remarks>
        public async Task<Observation> GetNEOLookupData(string neoReferenceId)
        {
            NullInputCheck(new string[] { neoReferenceId } );
            var NeoLookUpData = new Observation();

            if (int.TryParse(neoReferenceId, out int dispose))
            {
                string LookUpUrl = $"https://api.nasa.gov/neo/rest/v1/neo/{neoReferenceId}?api_key={_APIKey}";


                try
                {
                    NeoLookUpData = await _Client.SendApiLookUpRequest(LookUpUrl);
                    Logger.LogInformation("API Lookup fetch was successful");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error when fetching from API");
                    throw;
                }
            }

            return NeoLookUpData;
        }

                        
    }
}
