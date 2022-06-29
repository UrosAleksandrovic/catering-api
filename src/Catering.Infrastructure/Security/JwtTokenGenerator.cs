using Catering.Domain.Entities.IdentityAggregate;
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
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(_options.ExpirationInDays)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> GenerateClaims<T>(T identity) where T : Identity
    {
        var expirationTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, identity.Id),
            new Claim(JwtRegisteredClaimNames.Email, identity.Email),
            new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer),
            new Claim(JwtRegisteredClaimNames.Iat, expirationTime),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Aud, _options.Audience)
        };

        foreach (var role in identity.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}
