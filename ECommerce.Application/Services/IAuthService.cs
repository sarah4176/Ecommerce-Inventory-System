using ECommerce.Application.DTOs;

namespace ECommerce.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO refreshTokenRequest);
        Task<bool> RevokeTokenAsync(string username);
    }
}
