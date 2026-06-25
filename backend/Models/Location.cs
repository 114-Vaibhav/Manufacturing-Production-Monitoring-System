namespace backend.Models
{

    public class Location
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string FloorNo { get; set; } = string.Empty;

        public string TerminalNo { get; set; } = string.Empty;
        
        public LocationStatus Status { get; set; }
    }
}