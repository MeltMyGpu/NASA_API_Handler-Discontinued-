namespace APIRequestHandler.APIFetch.JsonWrapper
{
    /// <summary>
    /// A Simplified wrapper for near earth objects data, while containing less information than the standard wrapper, the inforamtion is much easier to access and there is less redundant information inside.
    /// </summary>
    public class NEOSimpleWrapper 
    // a simplified wrapper for the Near earth objects that allows easy access to specific data without having to memorise the JSON formatting.
    {
        public string? NeoRefId { get; set; }
        public string Key { get; set; }
        public int Index { get; set; }
        public float EstimatedMaxDiameter { get; set; }
        public bool IsPotentialHazzard { get; set; }
        public string? CloseApproachDate { get; set; }
        public string? RelativeVelocity { get; set; }
        public string? MissDistance { get; set; }

    }
}