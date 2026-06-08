using backend.Models;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;
using backend.Models.DTOs;
using BusinessLayer.Services;
using System.Security.Cryptography;
using System.Security.Authentication;
using System.Text;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserServices: IUserServices
    {
        IRepository<int, User> _userRepository;

        private readonly ITokenService _tokenService ;
        public UserServices(IRepository<int, User> userRepository,ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
       public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            UserValidator.ValidateUser(request);

            User user = await MapUserObjectFromRequest(request);
            User userFromDb = await _userRepository.Create(user);
            return new RegisterUserResponse
            {
                UserId = userFromDb.UserId,
                Role = userFromDb.Role
            };
        }
        private async Task<User> MapUserObjectFromRequest(RegisterUserRequest request)
        {
            HMACSHA256 hMACSHA256 = new HMACSHA256();
            User user = new User();
            user.UserName = request.UserName;
            user.FullName = request.FullName;
            user.Email = request.Email;
            user.PasswordHash = hMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            user.HashKey = hMACSHA256.Key;
            user.Role = request.Role;
            user.CreatedAt = DateTime.UtcNow;
            user.Status = UserStatus.Active;
            return user;
        }
    
        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            //  UserValidator.ValidateLoginRequest(request);

             User user = await _userRepository.GetByUserName(request.UserName);
            if (user == null)
            {
                throw new AuthenticationException("Invalid username or password.");
            }

            using (HMACSHA256 hMACSHA256 = new HMACSHA256(user.HashKey))
            {
                byte[] computedHash = hMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
                if (!computedHash.SequenceEqual(user.PasswordHash))
                {
                    throw new AuthenticationException("Invalid username or password.");
                }

            }

            return new LoginResponse
            {
                UserName = user.UserName,
                Token = _tokenService.CreateNewToken(new TokenRequest
                {
                    Username = user.UserName,
                    Role = user.Role
                })
            };                  
        }

        public async Task<User> GetUser(int id)
        {
            return await _userRepository.Get(id);
        }
        
    }
}