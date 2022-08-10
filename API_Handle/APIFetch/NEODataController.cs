using APIRequestHandler.JsonWrapper;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using APIRequestHandler.APIFetch.JsonWrapper;

namespace APIRequestHandler.APIFetch
{
    /// <summary>This class can be used to load a Feed request into the SortedDataHolder, for a simpler access format.</summary>
    public static class NEODataController // A static class used to load data into the SortedDataHolder for more easier access and use.
    {
        private static SortedDataHolder SortedData = SortedDataHolder.Instance;


        /// <summary>Sorts the handed Feed data.</summary>
        /// <param name="nEO">A NEORootObject containing a feed request return data. </param>
        public static void SortData(NEORootObject nEO)
        {

            foreach (var key in nEO.near_earth_objects.Keys)
            {
                if (!SortedData.NeoSimplifiedObjectAccess.ContainsKey(key)) // Here to avoid EX if SortData ran twice on dataset containing same keys;
                {
                    SortedData.NeoSimplifiedObjectAccess.Add(key, new List<NEOSimpleWrapper>());

                    for (int i = 0; i < nEO.near_earth_objects[key].Length - 1; i++)
                    {
                        SortedData.NeoSimplifiedObjectAccess[key].Add
                            (
                                new NEOSimpleWrapper()
                                {
                                    NeoRefId = nEO.near_earth_objects[key][i].neo_reference_id,
                                    Key = key,
                                    Index = i,
                                    EstimatedMaxDiameter = nEO.near_earth_objects[key][i].estimated_diameter.meters.estimated_diameter_max,
                                    IsPotentialHazzard = nEO.near_earth_objects[key][i].is_potentially_hazardous_asteroid,
                                    CloseApproachDate = nEO.near_earth_objects[key][i].close_approach_data[0].close_approach_date,
                                    RelativeVelocity = nEO.near_earth_objects[key][i].close_approach_data[0].relative_velocity.kilometers_per_hour,
                                    MissDistance = nEO.near_earth_objects[key][i].close_approach_data[0].miss_distance.kilometers
                                }
                            );
                        if (nEO.near_earth_objects[key][i].estimated_diameter.meters.estimated_diameter_max > SortedData.LargestDiameterObject.EstimatedMaxDiameter)
                        {
                            SortedData.LargestDiameterObject = SortedData.NeoSimplifiedObjectAccess[key][i];
                        }

                        if (nEO.near_earth_objects[key][i].is_potentially_hazardous_asteroid)
                        {
                            SortedData.PontentialHazardList.Add(SortedData.NeoSimplifiedObjectAccess[key][i]);
                        }

                        if (Convert.ToDouble(nEO.near_earth_objects[key][i].close_approach_data[0].relative_velocity.kilometers_per_hour) > Convert.ToDouble(SortedData.HighestRelativeVelocityObject.RelativeVelocity))
                        {
                            SortedData.HighestRelativeVelocityObject = SortedData.NeoSimplifiedObjectAccess[key][i];
                        }

                        if (Convert.ToDouble(nEO.near_earth_objects[key][i].close_approach_data[0].miss_distance.kilometers) < Convert.ToDouble(SortedData.SmallestMissDistanceObject.MissDistance))
                        {
                            SortedData.SmallestMissDistanceObject = SortedData.NeoSimplifiedObjectAccess[key][i];
                        }



                    }

                }
                else continue;

            }

        }

        /// <summary>
        ///   <para>
        /// Clears the sorted data holder so new data can be loaded in.</para>
        ///   <para>This must be ran before loading in new data from a diffrent feed request. 
        /// </para>
        /// </summary>
        public static void ClearSortedDataHolder()
        {
            SortedData.NeoSimplifiedObjectAccess.Clear();
            SortedData.PontentialHazardList.Clear();
            SortedData.LargestDiameterObject = new NEOSimpleWrapper();
            SortedData.HighestRelativeVelocityObject = new NEOSimpleWrapper();
            SortedData.SmallestMissDistanceObject = new NEOSimpleWrapper() { MissDistance = "9999999999999999999" };
        }

    }
}
