using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NASA_API_NEO_Wrapper;
using Microsoft.Extensions.Logging;



namespace API_Handle
{
    public class NEO_Handler
    {
        private readonly Client _client;
        private NEORootObject? _NEOData;
        private string _APIKey;
        private string? _url;
        private readonly Regex _dateFormatCheck;
        public ILogger Logger { get; set; }


        public NEO_Handler(string key)
        {
            _client = new Client();
            _APIKey = key; 
            _dateFormatCheck = new Regex("^[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]$"); // Is here to ensure the dates passed in are in teh correct format //
            Logger = IloggerFactory.LoggerCreation();
            Logger.LogInformation("Api key set to", key);

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

        public void ChangeAPIKey(string key)
        {
            NullInputCheck(new string[] {key});
            _APIKey = key;
            Logger.LogInformation(String.Format($"Api key changed to {key}"));
        }


        public async void GetNEOData(string dateStart, string dateEnd)
        {
            NullInputCheck(new string[] { dateStart, dateEnd });
            await GetOutNEOData(dateStart, dateEnd);
        }


        public async Task<NEORootObject> GetOutNEOData(string dateStart, string dateEnd)
        {
            NullInputCheck(new string[] { dateStart, dateEnd });
            if (_dateFormatCheck.IsMatch(dateStart) && _dateFormatCheck.IsMatch(dateEnd))
            {
                _url = string.Format($"https://api.nasa.gov/neo/rest/v1/feed?start_date={dateStart}&end_date={dateEnd}&api_key={_APIKey}");

                try
                {
                    _client.ConnectionCheck(_url);
                    Logger.LogInformation("Connection check successful;");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Ping error, either URL is incorrect or internet is diconnected; ");
                    throw;
                }

                try
                {
                    _NEOData = await _client.SendAPIRequest(_url);
                    Logger.LogInformation("API Fetch was successful"); // This log entry never hit?!
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

                        
    }
}
