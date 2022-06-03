using System.Net.NetworkInformation;
using System.Net;
using NASA_API_NEO_Wrapper;
using System.Text.Json;

namespace API_Handle
{
    public class Client
    {
        private static readonly HttpClient _client = new HttpClient();
        private bool _disposed;
        private bool _connected;

        public Client() // Default
        {
            _client.DefaultRequestHeaders.Accept.Clear();
        }

        public Client(string url) // WORKING
        {
            ConnectionCheck(url);
            _client.DefaultRequestHeaders.Accept.Clear();
          
        }


        public void ConnectionCheck(string url) // WORKING
        {
            try
            {
                _connected = ConnectionPing();
                if (!_connected)
                {
                    throw new Exception("Internet not connected");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        private static bool ConnectionPing() // WORKING
        {
            bool Pingable;
            Ping pinger = new();

            var pingCheck = pinger.Send("www.google.com", 3000);                   
            Pingable = pingCheck.Status == IPStatus.Success;
            pinger.Dispose();
            
            return Pingable;
        }


        public async Task<NEORootObject?> SendAPIRequest(string url)
        {

            try
            {
                APIValidCheck(url);
            }
            catch (WebException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            var streamTask = _client.GetStreamAsync(url);
            var NEO = await JsonSerializer.DeserializeAsync<NEORootObject>(await streamTask);

            if (NEO != null)
                return NEO;
            else throw new InvalidOperationException("The Api call has returned a NUll Reference; ");
        }

        private static void APIValidCheck(string url)
        {
            var httpWebRequestCheck = (HttpWebRequest)WebRequest.Create(url);
            var httpWebResponceCheck = (HttpWebResponse)httpWebRequestCheck.GetResponse();

            if (httpWebResponceCheck.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(string.Format($"API connection status code 200: "));
                
            }
            httpWebResponceCheck.Close();
        }

    }
}