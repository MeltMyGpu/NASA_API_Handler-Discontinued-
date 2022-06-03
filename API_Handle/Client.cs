using System.Net.NetworkInformation;
using System.Net;
using NASA_API_NEO_Wrapper;
using System.Text.Json;

namespace API_Handle
{
    public class Client
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly bool _disposed;
        private readonly bool _connected;

        public Client(string url) // WORKING
        {
            _connected = ConnectionCheck(url);
            _client.DefaultRequestHeaders.Accept.Clear();
          
        }


        public static bool ConnectionCheck(string url) // WORKING
        {
            bool IsConnected = ConnectionPing(url);

            if (!IsConnected)
                Console.WriteLine("You are not connected to the internet, or the provided URL was unreachable... "); 

            return IsConnected;
        }

        private static bool ConnectionPing(string url) // WORKING
        {
            bool Pingable = false;
            Ping? pinger = null;

            try
            {
                 pinger = new Ping();
                 var pingCheck = pinger.Send(url, 3000);                   
                 Pingable = pingCheck.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                Console.WriteLine("You have entered an incorrect URL...");               
                return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return Pingable;
        }

        public async Task<NEORootObject?> SendAPIRequest(string dateStart, string dateEnd, string key)
        {
            var url = string.Format($"https://api.nasa.gov/neo/rest/v1/feed?start_date={dateStart}&end_date={dateEnd}&api_key={key}");

            try
            {
               APIValidCheck(url);
            }
            catch  (WebException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }

            var streamTask = _client.GetStreamAsync(url);
            var NEO = await JsonSerializer.DeserializeAsync<NEORootObject>(await streamTask);

            return NEO;

        }

        public static void APIValidCheck(string url)
        {
            try
            {
                var httpWebRequestCheck = (HttpWebRequest)WebRequest.Create(url);
                var httpWebResponceCheck = (HttpWebResponse)httpWebRequestCheck.GetResponse();

                if ( httpWebResponceCheck.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(string.Format($"API connection status code 200: "));
                }
                httpWebResponceCheck.Close();

            }
            catch (WebException e)
            {
                Console.WriteLine($" Web exception raised, the following error occured: {e.Status}");
                throw;
            }
            catch  ( Exception e)
            {
                Console.WriteLine($" Exeption thrown : {e.Message} " );
                throw;
            }
        }

    }
}