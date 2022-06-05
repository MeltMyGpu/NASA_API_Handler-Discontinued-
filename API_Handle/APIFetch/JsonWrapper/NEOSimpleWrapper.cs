namespace APIRequestHandler.APIFetch.JsonWrapper
{
    public class NEOSimpleWrapper 
    // a simplified wrapper for the Near earth objects that allows easy access to specific data without having to memorise the JSON formatting.
    {
        public string? NeoRefId { get; set; }
        public int Index { get; set; }
        public float EstimatedMaxDiameter { get; set; }
        public bool IsPotentialHazzard { get; set; }
        public string? CloseApproachDate { get; set; }
        public string? RelativeVelocity { get; set; }
        public string? MissDistance { get; set; }

    }
}