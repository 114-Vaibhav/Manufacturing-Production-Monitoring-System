using backend.Models;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;
using backend.Models.DTOs;
using System.Security.Cryptography;
using System.Security.Authentication;
using System.Text;
using System.Net.Http.Headers;


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

            User user = MapUserObjectFromRequest(request);
            User userFromDb = await _userRepository.Create(user);
            return new RegisterUserResponse
            {
                UserId = userFromDb.UserId,
                Role = userFromDb.Role
            };
        }
        private static User MapUserObjectFromRequest(RegisterUserRequest request)
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
            Console.WriteLine(request);
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
                    throw new AuthenticationException("Invalid username or password or role.");
                }

            }
            if(user.Role != request.Role)
            {
                // throw new AuthenticationException()
                throw new AuthenticationException("Invalid Role");
            }
            
            Console.WriteLine($"User {user.UserName} authenticated successfully with role {user.Role}.");
            
            // 1. Generate the JWT Access Token
            var accessToken = _tokenService.CreateNewToken(new TokenRequest
            {
                UserId = user.UserId,
                Username = user.UserName,
                Role = user.Role
            });

            // 2. Generate the Refresh Token
            var refreshToken = _tokenService.GenerateRefreshToken();

            // 3. Return the updated response
            return new LoginResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status,
                AccessToken = accessToken,      // <-- Attached Access Token
                RefreshToken = refreshToken     // <-- Attached Refresh Token
            };          
        }

        public async Task<User> GetUser(int id)
        {
            return await _userRepository.Get(id);
        }
        public static bool IsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            bool hasMinimum8Chars = password.Length >= 8;
            bool hasUpperChar = password.Any(char.IsUpper);
            bool hasLowerChar = password.Any(char.IsLower);
            bool hasNumber = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasMinimum8Chars && 
                hasUpperChar && 
                hasLowerChar && 
                hasNumber && 
                hasSpecialChar;
        }

        public async Task<User> UpdatePassword(UpdatePasswordRequest request)
        {
            HMACSHA256 hMACSHA256 = new HMACSHA256();
            if(!IsValid(request.NewPassword))
            {
                throw new ArgumentException("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                // return null;
            }
            User user = await GetUser(request.UserId);
            user.PasswordHash = hMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(request.NewPassword));
            user.HashKey = hMACSHA256.Key;
            return await _userRepository.Update(user.UserId,user);
        }
        
    }
}
