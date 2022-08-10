﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using APIRequestHandler;
using System.Collections;
using APIRequestHandler.APIFetch;
using APIRequestHandler.APIFetch.JsonWrapper;

namespace NASA_API_ACESSOR
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        
        static async Task Main() // this is all testing functionality and should be ignored.
         {
            var NEO_hand = new NEOHandler("DEMO_KEY");
            var NEO =  await NEO_hand.GetNEOData("2022-01-01","2022-01-08");
            //NEO_hand.DisposeOfClient();

            NEODataController.SortData(NEO);
            var Sorted = SortedDataHolder.Instance;
            var NeoLookUp = await NEO_hand.GetNEOLookupData(Sorted.LargestDiameterObject.NeoRefId);

            Console.WriteLine(NeoLookUp.id + " : "+ NeoLookUp.close_approach_data[0].miss_distance);
            Console.WriteLine();
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
    }
}
