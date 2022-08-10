using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using APIRequestHandler.JsonWrapper;

namespace APIRequestHandler.APIFetch.JsonWrapper // singleton instance to avoid overwriting wehn loading data into it.
{
    /// <summary>This singeton class holds the sorted data form a Feed request, this class can be loaded using the Static class NeoDataController.</summary>
    public sealed class SortedDataHolder
    {
        private static SortedDataHolder? instance = null;
        private static readonly object padLock = new object();



        SortedDataHolder()
        {
            NeoSimplifiedObjectAccess = new Dictionary<string, List<NEOSimpleWrapper>>();
            PontentialHazardList = new List<NEOSimpleWrapper>();
            LargestDiameterObject = new NEOSimpleWrapper();
            HighestRelativeVelocityObject = new NEOSimpleWrapper();
            SmallestMissDistanceObject = new NEOSimpleWrapper() { MissDistance = "9999999999999999999" };
        }

        /// <summary>Gets a refrence instance of the SortedDataHolder.</summary>
        /// <value>The instance.</value>
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


        /// <summary>Gets or sets the neo simplified object access.</summary>
        /// <value>The neo simplified object access.</value>
        /// <remarks>
        ///   <para>A dictonary containing a simpler version of the object data fetched by a feed request.</para>
        ///   <para>The dictonary Key is the same date as the NEORootObjects dictonary.</para>
        /// </remarks>
        public Dictionary<string, List<NEOSimpleWrapper>> NeoSimplifiedObjectAccess { get; set; } // key is shared with json wrapper //

        /// <summary>Gets or sets the largest diameter object.</summary>
        /// <value>Conatins a NEOSimpleWrapper of the Largest diameter object in a loaded feed request</value>
        public NEOSimpleWrapper LargestDiameterObject { get; set; }

        /// <summary>Gets or sets the pontential hazard list.</summary>
        /// <value>A list of all neo objects that are flagged as a potentail hazard.</value>
        public List<NEOSimpleWrapper> PontentialHazardList { get; set; }

        /// <summary>Gets or sets the highest relative velocity object.</summary>
        /// <value>
        ///   <para>
        /// Contains highest relative velocity object from a feed request.</para>
        /// </value>
        public NEOSimpleWrapper HighestRelativeVelocityObject { get; set; }

        /// <summary>Gets or sets the smallest miss distance object.</summary>
        /// <value>contains smallest miss distance object from a feed request.</value>
        public NEOSimpleWrapper SmallestMissDistanceObject { get; set; }



    }


}
