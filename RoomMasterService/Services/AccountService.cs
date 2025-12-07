using RoomMasterService.Data;
using RoomMasterService.Models;
using RoomMasterService.DTOs;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace RoomMasterService.Services;

public class AccountService : IAccountService
{
    private readonly IDataAccess _dataAccess;
    private readonly IConfiguration _configuration;

    public AccountService(IDataAccess dataAccess, IConfiguration configuration)
    {
        _dataAccess = dataAccess;
        _configuration = configuration;
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private string GenerateRefreshToken()
    {
        var random = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(random);
        return Convert.ToBase64String(random);
    }

    private string GenerateJwtToken(Account account)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured");
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "roommaster";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "roommaster-audience";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.Account_Id),
            new Claim(ClaimTypes.Name, account.Username),
            new Claim(ClaimTypes.Role, account.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(AuthResponse? Response, string? Error)> AuthenticateAsync(string username, string password)
    {
        var acc = await _dataAccess.GetAccountByUsernameAsync(username);
        if (acc == null)
            return (null, "Invalid username or password");

        var hashed = HashPassword(password);
        if (acc.Password != hashed)
            return (null, "Invalid username or password");

        // generate tokens
        var accessToken = GenerateJwtToken(acc);
        var refreshToken = GenerateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(7);

        await _dataAccess.SaveRefreshTokenAsync(acc.Account_Id, refreshToken, refreshExpiry);

        // hide password
        acc.Password = string.Empty;

        var response = new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken, Account = acc };
        return (response, null);
    }

    public async Task<string> CreateAccountAsync(Account account)
    {
        account.Password = HashPassword(account.Password);
        return await _dataAccess.CreateAccountAsync(account);
    }

    public async Task<(AuthResponse? Response, string? Error)> RefreshTokenAsync(string refreshToken)
    {
        var acc = await _dataAccess.GetAccountByRefreshTokenAsync(refreshToken);
        if (acc == null)
            return (null, "Invalid refresh token");

        if (acc.RefreshTokenExpires == null || acc.RefreshTokenExpires < DateTime.UtcNow)
            return (null, "Refresh token expired");

        var accessToken = GenerateJwtToken(acc);
        var newRefresh = GenerateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(7);
        await _dataAccess.SaveRefreshTokenAsync(acc.Account_Id, newRefresh, refreshExpiry);

        acc.Password = string.Empty;
        var response = new AuthResponse { AccessToken = accessToken, RefreshToken = newRefresh, Account = acc };
        return (response, null);
    }
}
