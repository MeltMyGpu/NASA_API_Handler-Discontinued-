using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using APIRequestHandler.JsonWrapper;
using APIRequestHandler.APIFetch.JsonWrapper;

namespace APIRequestHandler // singleton instance to avoid overwriting wehn loading data into it.
{
    public sealed class SortedDataHolder
    {
        private static SortedDataHolder? instance = null;
        private static readonly object padLock = new object();



        SortedDataHolder() {
            NeoSimplifiedObjectAccess = new Dictionary<string, List<NEOSimpleWrapper>>();
            PontentialHazardList = new List<NEOSimpleWrapper>();
            LargestDiameterObject = new NEOSimpleWrapper();
            HighestRelativeVelocityObject = new NEOSimpleWrapper();
            SmallestMissDistanceObject = new NEOSimpleWrapper() { MissDistance = "9999999999999999999"};
        }

        public static SortedDataHolder Instance // Ensures only one instance of the object is ever created // 
        {
            get
            {
                lock (padLock) // ensures this is thread safe //
                {
                    if (instance == null)
                    {
                        instance = new SortedDataHolder();
                    }
                    return instance;
                }
            }
        }


        public Dictionary<string, List<NEOSimpleWrapper>> NeoSimplifiedObjectAccess { get; set; } // key is shared with json wrapper //
        public NEOSimpleWrapper LargestDiameterObject { get; set; }              
        public List<NEOSimpleWrapper> PontentialHazardList { get; set; }   
        public NEOSimpleWrapper HighestRelativeVelocityObject { get; set; }   
        public NEOSimpleWrapper SmallestMissDistanceObject { get; set; }      



    }
    

}
