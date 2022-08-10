using System.Net.NetworkInformation;
using System.Net;
using System.Text.Json;
using APIRequestHandler.JsonWrapper;

namespace APIRequestHandler.APIFetch
{
    /// <summary>This class handles the requests to the API and the Json unpacking.</summary>
    /// <remarks>This class is handled by the NEOHandler, and should not be used directly. any changes to this class could result in breaking the whole system.</remarks>
    public class Client
    {
        private static readonly HttpClient _Client = new HttpClient();
        private bool _Disposed;

        private bool _Connected;

        /// <summary>Initializes a new instance of the <see cref="Client" /> class
        /// and clears headers.</summary>
        public Client() // Default
        {
            _Client.DefaultRequestHeaders.Accept.Clear();
        }

        /// <summary>Checks if the host PC is connected to the internet via a PING call. </summary>
        /// <returns>returns Boolan based on success of ping check.</returns>
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


        /// <summary>Sends the API feed request.</summary>
        /// <param name="url">The URL created by the NEOHandler from the dates and key given.</param>
        /// <returns>A NEORootObject containing the requested information</returns>
        /// <exception cref="System.InvalidOperationException">The Api call has returned a NUll value;</exception>
        public async Task<NEORootObject?> SendApiFeedRequest(string url) // WORKING
        {
            APIValidCheck(url);

            var StreamTask = _Client.GetStreamAsync(url);
            var NEO = await JsonSerializer.DeserializeAsync<NEORootObject>(await StreamTask);

            if (NEO != null)
                return NEO;
            else throw new InvalidOperationException("The Api call has returned a NUll value; ");
        }


        /// <summary>Sends the API look up request.</summary>
        /// <param name="url">The URL that is created by NEOHandler from the NeoID and key.</param>
        /// <returns>Returns an observation object containing all information related the the requested neo.</returns>
        /// <exception cref="System.InvalidOperationException">The Api call has returned a NULL value</exception>
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


        /// <summary>
        ///   <para>
        /// Releases unmanaged and - optionally - managed resources.
        /// </para>
        ///   <para>Called by the NEOHandler.</para>
        /// </summary>
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