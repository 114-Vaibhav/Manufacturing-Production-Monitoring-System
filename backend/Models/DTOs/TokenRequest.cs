namespace backend.Models.DTOs
{
    public class TokenRequest
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        
    }
}
