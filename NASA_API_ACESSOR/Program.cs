using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using APIRequestHandler;
using APIRequestHandler.JsonWrapper;
using System.Collections;

namespace NASA_API_ACESSOR
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        

        static async Task Main()
         {
            var NEO_hand = new NEOHandler("DEMO_KEY");
            var NEO =  await NEO_hand.GetNEOData("2022-01-01","2022-01-08");
            NEO_hand.DisposeOfClient();

            NEODataController.SortData(NEO);
            NEODataController.SortData(NEO);
            var Sorted = SortedDataHolder.Instance;

            Console.WriteLine(
                 "NeoRefId" + " : " +
                 "EstimatedMaxDiameter" + " : " +
                 "IsPotentialHazzard" + " : " +
                 "CloseApproachDate" + " : " +
                 "RelativeVelocity" + " : " +
                 "MissDistance" + " // "
                 );

            foreach (var item in Sorted.NeoSimplifiedObjectAccess.Keys)
            {
                for(int i = 0; i < Sorted.NeoSimplifiedObjectAccess[item].Count; i++)
                Console.WriteLine(
                    Sorted.NeoSimplifiedObjectAccess[item][i].NeoRefId+ " :      " +
                    Sorted.NeoSimplifiedObjectAccess[item][i].EstimatedMaxDiameter+ "      :        " +
                    Sorted.NeoSimplifiedObjectAccess[item][i].IsPotentialHazzard + "          :   " +
                    Sorted.NeoSimplifiedObjectAccess[item][i].CloseApproachDate + "   : " +
                    Sorted.NeoSimplifiedObjectAccess[item][i].RelativeVelocity + " : " +
                    Sorted.NeoSimplifiedObjectAccess[item][i].MissDistance + " // "
                    );
            }
            

        }

        private static async Task ProcessNasaData()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var streamTask = client.GetStreamAsync("https://api.nasa.gov/neo/rest/v1/feed?start_date=2022-01-01&end_date=2022-01-08&api_key=mJvy7YJc61X7vQJcOc98F5Wr1dxm8HqZGildnXbc");
            //var datas = await stringTask;
            var NEO = await JsonSerializer.DeserializeAsync<NEORootObject>(await streamTask);
        }
    }
}
