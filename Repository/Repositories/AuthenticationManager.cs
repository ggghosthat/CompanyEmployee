using Contracts.Interfaces;
using Entities.Models;
using Entities.DTO;

using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
namespace Repository;
public class AuthenticationManager : IAuthenticationManager
{
    private readonly UserManager<User> _userManger;
    private readonly IConfiguration _configuration;

    private User user;

    public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManger = userManager;
        _configuration = configuration;
    }

    public async Task<bool> ValidateUser(UserAuthenticationDto userForAuth)
    {
        user = await _userManger.FindByNameAsync(userForAuth.Username);

        return (user != null && await _userManger.CheckPasswordAsync(user, userForAuth.Password));
    }

    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET1"));
        var secret = new SymmetricSecurityKey(key);
        
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var roles = await _userManger.GetRolesAsync(user);
        foreach(var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
                                                  List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings.GetSection("validIssuer").Value,
            audience: jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}
