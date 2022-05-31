using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NASA_API_ACESSOR
{
    public class RootObject
    {
        public PageLinks links { get; set; }
        public int element_count { get; set; }
        public Dictionary<string,Observation> near_earth_objects { get; set; }
    }

    public class PageLinks
    {
        public string? next { get; set; }
        public string? prev { get; set; }
        public string?  self { get; set; }

    }
}
