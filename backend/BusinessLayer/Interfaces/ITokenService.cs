using backend.Models.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface ITokenService
    {
        public string CreateNewToken(TokenRequest request);
        public string GenerateRefreshToken();
    }
}
