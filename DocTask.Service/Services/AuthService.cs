using DocTask.Core.Dtos.Auth;
using DocTask.Core.Interfaces.Services;
using DocTask.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DocTask.Service.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (dbUser == null)
        {
            throw new UnauthorizedAccessException("User not found in database");
        }

        var token = await GenerateJwtTokenAsync(user);
        var refreshToken = GenerateRefreshToken();

        // Update refresh token in database
        dbUser.Refreshtoken = refreshToken;
        dbUser.Refreshtokenexpirytime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = new UserInfo
            {
                UserId = dbUser.UserId,
                Username = dbUser.Username,
                Email = dbUser.Email ?? string.Empty,
                FullName = dbUser.FullName,
                PhoneNumber = dbUser.PhoneNumber,
                Role = dbUser.Role,
                OrgId = dbUser.OrgId,
                UnitId = dbUser.UnitId,
                PositionId = dbUser.PositionId,
                PositionName = dbUser.PositionName
            }
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (existingUsername != null)
        {
            throw new InvalidOperationException("Username already exists");
        }

        var identityUser = new IdentityUser
        {
            UserName = request.Username,
            Email = request.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(identityUser, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        // Create user in our custom User table
        var dbUser = new DocTask.Core.Models.User
        {
            Username = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Password = HashPassword(request.Password), // Store hashed password for compatibility
            Role = "User", // Default role
            OrgId = request.OrgId,
            UnitId = request.UnitId,
            PositionId = null, // Set to null to avoid foreign key constraint issues
            PositionName = request.PositionName,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(dbUser);
        await _context.SaveChangesAsync();

        var token = await GenerateJwtTokenAsync(identityUser);
        var refreshToken = GenerateRefreshToken();

        // Update refresh token in database
        dbUser.Refreshtoken = refreshToken;
        dbUser.Refreshtokenexpirytime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = new UserInfo
            {
                UserId = dbUser.UserId,
                Username = dbUser.Username,
                Email = dbUser.Email ?? string.Empty,
                FullName = dbUser.FullName,
                PhoneNumber = dbUser.PhoneNumber,
                Role = dbUser.Role,
                OrgId = dbUser.OrgId,
                UnitId = dbUser.UnitId,
                PositionId = dbUser.PositionId,
                PositionName = dbUser.PositionName
            }
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Refreshtoken == refreshToken);
        if (user == null || user.Refreshtokenexpirytime <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token");
        }

        var identityUser = await _userManager.FindByEmailAsync(user.Email);
        if (identityUser == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var newToken = await GenerateJwtTokenAsync(identityUser);
        var newRefreshToken = GenerateRefreshToken();

        // Update refresh token in database
        user.Refreshtoken = newRefreshToken;
        user.Refreshtokenexpirytime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = new UserInfo
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                OrgId = user.OrgId,
                UnitId = user.UnitId,
                PositionId = user.PositionId,
                PositionName = user.PositionName
            }
        };
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Refreshtoken == refreshToken);
        if (user == null)
        {
            return false;
        }

        user.Refreshtoken = null;
        user.Refreshtokenexpirytime = null;
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new("jti", Guid.NewGuid().ToString()),
            new("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (dbUser != null)
        {
            claims.Add(new Claim("UserId", dbUser.UserId.ToString()));
            claims.Add(new Claim("Role", dbUser.Role));
            claims.Add(new Claim("FullName", dbUser.FullName));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static string HashPassword(string password)
    {
        // Simple hash for compatibility - in production, use proper hashing
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
