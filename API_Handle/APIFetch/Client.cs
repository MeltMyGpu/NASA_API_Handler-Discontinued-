using System.Net.NetworkInformation;
using System.Net;
using System.Text.Json;
using APIRequestHandler.JsonWrapper;

namespace APIRequestHandler.APIFetch
{
    public class Client
    {
        private static readonly HttpClient _Client = new HttpClient();
        private bool _Disposed;

        private bool _Connected;

        public Client() // Default
        {
            _Client.DefaultRequestHeaders.Accept.Clear();
        }

        public Client(string url) // WORKING
        {
            ConnectionCheck();
            _Client.DefaultRequestHeaders.Accept.Clear();

        }


        public bool ConnectionCheck() // WORKING
        {

            _Connected = ConnectionPing();
            return _Connected;
        }


        private static bool ConnectionPing() // WORKING
        {
            bool Pingable;
            Ping Pinger = new();

            var PingCheck = Pinger.Send("www.google.com", 3000);
            Pingable = PingCheck.Status == IPStatus.Success;
            Pinger.Dispose();

            return Pingable;
        }


        public async Task<NEORootObject?> SendApiFeedRequest(string url) // WORKING
        {
            APIValidCheck(url);

            var StreamTask = _Client.GetStreamAsync(url);
            var NEO = await JsonSerializer.DeserializeAsync<NEORootObject>(await StreamTask);

            if (NEO != null)
                return NEO;
            else throw new InvalidOperationException("The Api call has returned a NUll value; ");
        }


        public async Task<Observation> SendApiLookUpRequest(string url)
        {
            APIValidCheck(url);

            var StreamTask = _Client.GetStreamAsync(url);
            var NeoLookUpData = await JsonSerializer.DeserializeAsync<Observation>(await StreamTask);

            if (NeoLookUpData != null)
                return NeoLookUpData;
            else throw new InvalidOperationException("The Api call has returned a NULL value");
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
            if (!_Disposed)
            {
                _Client.Dispose();
                _Disposed = true;
            }
            else Console.WriteLine("Client has already been disposed of; ");
        }
    }
}