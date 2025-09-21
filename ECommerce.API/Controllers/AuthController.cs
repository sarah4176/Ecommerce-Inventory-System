using ECommerce.Application.DTOs;
using ECommerce.Application.Services;
using Ecommerce.Middleware.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.ErrorResponse("Invalid input data", 400));

            var authResponse = await _authService.RegisterAsync(registerDto);
            return Ok(ApiResponse.SuccessResponse(authResponse, "User registered successfully"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.ErrorResponse("Invalid input data", 400));

            var authResponse = await _authService.LoginAsync(loginDto);
            return Ok(ApiResponse.SuccessResponse(authResponse, "Login successful"));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse>> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.ErrorResponse("Invalid input data", 400));

            var authResponse = await _authService.RefreshTokenAsync(refreshTokenRequest);
            return Ok(ApiResponse.SuccessResponse(authResponse, "Token refreshed successfully"));
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> RevokeToken()
        {
            var username = User.Identity.Name;
            var result = await _authService.RevokeTokenAsync(username);

            if (!result)
                return BadRequest(ApiResponse.ErrorResponse("Failed to revoke token", 400));

            return Ok(ApiResponse.SuccessResponse(null, "Token revoked successfully"));
        }
    }
}
