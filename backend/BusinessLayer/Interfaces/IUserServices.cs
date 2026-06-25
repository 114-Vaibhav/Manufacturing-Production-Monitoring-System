using backend.Models;
using backend.Models.DTOs;
namespace BusinessLayer.Interfaces
{
    public interface IUserServices
    {
        public Task<RegisterUserResponse> RegisterUser(RegisterUserRequest user);
        public Task<LoginResponse> LoginUser(LoginRequest user);
        public Task<User> GetUser(int id);
        public Task<User> UpdatePassword(UpdatePasswordRequest request);
    }
}