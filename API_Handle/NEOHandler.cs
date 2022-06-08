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
    public class NEOHandler
    {
        private readonly Client _Client;
        private NEORootObject? _NEOData;
        private string _APIKey;
        private string? _FeedUrl;
        private readonly Regex _DateFormatCheck;
        public bool Connected { get; private set; }
        public ILogger Logger { get; set; }


        public NEOHandler(string key)
        {
            _Client = new Client();
            _APIKey = key; 
            _DateFormatCheck = new Regex("^[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]$"); // Is here to ensure the dates passed in are in teh correct format //
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

        public void DisposeOfClient()
        {
            _Client.Dispose();
            Logger.LogInformation("HTTP Client disposed; ");
        }

        public void ChangeAPIKey(string key)
        {
            NullInputCheck(new string[] {key});
            _APIKey = key;
            Logger.LogInformation(String.Format($"Api key changed to {key}"));
        }


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
