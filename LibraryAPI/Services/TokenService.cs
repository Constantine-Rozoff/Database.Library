using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContextAndModels.Models;
using LibraryAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.Services;

internal class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration)
    {
        var strkey = configuration["TokenKey"];
        if (strkey == null) throw new ArgumentNullException(nameof(strkey));
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

    }
    public string CreateToken(Employee user)
    {
        var claims = new List<Claim>
        {
            new (Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.NameId,user.Login),
            new (Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email,user.Email),
            new (ClaimTypes.Role, user.GetType().Name)
        };
        var signature = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var descr = new SecurityTokenDescriptor
        {
            SigningCredentials = signature,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(5)
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descr);
        return handler.WriteToken(token);
    }
}