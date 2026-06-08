namespace backend.Models.DTOs
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int? MachineId { get; set; }
        public int? LocationId { get; set; }

    }
}