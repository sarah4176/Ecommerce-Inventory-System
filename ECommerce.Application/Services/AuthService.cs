using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.UnitOfWork;
using Ecommerce.Middleware.Common;
using Ecommerce.Middleware.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ecommerce.Middleware.JWT.Utility;

namespace ECommerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordHasher _passwordHasher;
  

        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings,IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto)
        {
            if (await _unitOfWork.Users.GetByEmailAsync(registerDto.Email) != null)
                throw ApiException.Conflict("Email already registered");

            if (await _unitOfWork.Users.GetByUsernameAsync(registerDto.Username) != null)
                throw ApiException.Conflict("Username already taken");
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _passwordHasher.HashPassword(registerDto.Password),
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);
            if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
                throw ApiException.Unauthorized("Invalid username or password");

            return GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO refreshTokenRequest)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.Token);
            var username = principal.Identity.Name;

            var user = await _unitOfWork.Users.GetByUsernameAsync(username);
            if (user == null || user.RefreshToken != refreshTokenRequest.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
                throw ApiException.Unauthorized("Invalid refresh token");
            return GenerateAuthResponse(user);
        }

        public async Task<bool> RevokeTokenAsync(string username)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(username);
            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = DateTime.MinValue;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            return true;
        }

        private AuthResponseDTO GenerateAuthResponse(User user)
        {
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
            user.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            _unitOfWork.SaveAsync().Wait(); 

            return new AuthResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes)
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
