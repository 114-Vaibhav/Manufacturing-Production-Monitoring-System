namespace backend.Models.DTOs
{
    public class TokenRequest
    {
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        
    }
}
