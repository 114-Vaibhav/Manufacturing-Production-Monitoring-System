namespace backend.Models.DTOs
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; } = string.Empty;
        
    }
}