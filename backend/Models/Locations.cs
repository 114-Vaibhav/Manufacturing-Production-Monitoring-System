namespace backend.Models
{
    public enum LocationStatus
    {
        Active,
        Inactive,
        UnderMaintenance
    }
    public class Location
    {
        public int LocationId { get; set; }

        public string FloorNo { get; set; } = string.Empty;

        public string TerminalNo { get; set; } = string.Empty;
        
        public LocationStatus Status { get; set; }
    }
}