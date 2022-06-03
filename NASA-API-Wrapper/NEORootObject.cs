namespace NASA_API_NEO_Wrapper
{
    public class NEORootObject
    {
        public PageLinks links { get; set; }
        public int element_count { get; set; }
        public Dictionary<string, Observation[]> near_earth_objects { get; set; }
    }

    public class PageLinks
    {
        public string? next { get; set; }
        public string? prev { get; set; }
        public string self { get; set; }

    }
}