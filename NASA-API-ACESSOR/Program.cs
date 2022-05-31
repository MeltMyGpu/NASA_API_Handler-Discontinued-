using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace NASA_API_ACESSOR
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main()
        {
            await ProcessNasaData();
        }

        private static async Task ProcessNasaData()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var streamTask = client.GetStreamAsync("https://api.nasa.gov/neo/rest/v1/feed?start_date=2022-01-01&end_date=2022-01-08&api_key=mJvy7YJc61X7vQJcOc98F5Wr1dxm8HqZGildnXbc" );
            //var datas = await stringTask;
            var NEO = await JsonSerializer.DeserializeAsync<RootObject>(await streamTask);

            //foreach( var x in NEO)
            //{
            //    Console.WriteLine(x.near_earth_objects.Keys);
            //}

            //Console.WriteLine(datas);
        }
    }
}
