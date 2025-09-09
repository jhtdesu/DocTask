using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Auth;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockTask.Api.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(new ApiResponse<AuthResponse> { Data = result, Message = "User registered successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object> { Success = false, Error = ex.Message });
        }
        catch (Exception )
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Error = "An error occurred during registration" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            return Ok(new ApiResponse<AuthResponse> { Data = result, Message = "Login successful" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ApiResponse<object> { Success = false, Error = ex.Message });
        }
        catch (Exception )
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Error = "An error occurred during login" });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(new ApiResponse<AuthResponse> { Data = result, Message = "Token refreshed successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ApiResponse<object> { Success = false, Error = ex.Message });
        }
        catch (Exception )
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Error = "An error occurred during token refresh" });
        }
    }

    [HttpPost("revoke-token")]
    [Authorize]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
    {
        try
        {
            var result = await _authService.RevokeTokenAsync(request.RefreshToken);
            if (result)
            {
                return Ok(new ApiResponse<object> { Message = "Token revoked successfully" });
            }
            return BadRequest(new ApiResponse<object> { Success = false, Error = "Invalid refresh token" });
        }
        catch (Exception )
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Error = "An error occurred during token revocation" });
        }
    }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class RevokeTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
