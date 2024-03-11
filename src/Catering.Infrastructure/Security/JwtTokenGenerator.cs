using Catering.Domain.Aggregates.Identity;
using Catering.Infrastructure.Security.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Catering.Infrastructure.Security;

internal class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly SecurityJwtSettings _options;

    public JwtTokenGenerator(IOptions<SecurityJwtSettings> options)
    {
        _options = options.Value;
    }

    public string GenerateToken<T>(T identity) where T : Identity
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = GenerateClaims(identity);
        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(_options.ExpirationInDays)).DateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> GenerateClaims<T>(T identity) where T : Identity
    {
        var expirationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, identity.Id),
            new(JwtRegisteredClaimNames.Email, identity.Email),
            new(JwtRegisteredClaimNames.Iss, _options.Issuer),
            new(JwtRegisteredClaimNames.Iat, expirationTime),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Aud, _options.Audience),
            new(ClaimTypes.Role, identity.Role.ToString())
        };

        return claims;
    }
}
