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
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = GenerateClaims(identity);
        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            issuer: _options.Issuer,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(_options.ExpirationInDays)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> GenerateClaims<T>(T identity) where T : Identity
    {
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, identity.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer));

        claims.Add(new Claim(
            JwtRegisteredClaimNames.Iat,
            new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()));

        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        claims.Add(new Claim(nameof(Identity.Permissions), ((byte)identity.Permissions).ToString()));
        return claims;

    }
}
