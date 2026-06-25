namespace backend.Models.DTOs
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }        
    }
}