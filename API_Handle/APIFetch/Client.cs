using System.Net.NetworkInformation;
using System.Net;
using System.Text.Json;
using APIRequestHandler.JsonWrapper;

namespace APIRequestHandler.APIFetch
{
    public class Client
    {
        private static readonly HttpClient _client = new HttpClient();
        private bool _disposed;

        private bool _Connected;

        public Client() // Default
        {
            _client.DefaultRequestHeaders.Accept.Clear();
        }

        public Client(string url) // WORKING
        {
            ConnectionCheck(url);
            _client.DefaultRequestHeaders.Accept.Clear();

        }


        public bool ConnectionCheck(string url) // WORKING
        {

            _Connected = ConnectionPing();
            return _Connected;
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


        public async Task<NEORootObject?> SendAPIRequest(string url) // WORKING
        {
            APIValidCheck(url);

            var streamTask = _client.GetStreamAsync(url);
            var NEO = await JsonSerializer.DeserializeAsync<NEORootObject>(await streamTask);

            if (NEO != null)
                return NEO;
            else throw new InvalidOperationException("The Api call has returned a NUll Reference; ");
        }

        private static void APIValidCheck(string url) // WORKING
        {
            var httpWebRequestCheck = (HttpWebRequest)WebRequest.Create(url);
            var httpWebResponceCheck = (HttpWebResponse)httpWebRequestCheck.GetResponse();

            if (httpWebResponceCheck.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("API's connection status code is: 200 ");

            }
            httpWebResponceCheck.Close();
        }


        public void Dispose()  // WORKING
        {
            if (!_disposed)
            {
                _client.Dispose();
            }
            else Console.WriteLine("Client has already been disposed of; ");
        }
    }
}