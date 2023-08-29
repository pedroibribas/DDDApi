
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Token;

/// <summary>
/// Classe com métodos para construir o token JWT.
/// </summary>
public class TokenJWTBuilder
{
    public SecurityKey? securityKey;
    public string subject = "";
    public string issuer = "";
    public string audience = "";
    public Dictionary<string, string> claims = new() { };
    public DateTime? expiresIn;

    public TokenJWTBuilder AddSecurityKey(SecurityKey securityKey)
    {
        this.securityKey = securityKey;
        return this;
    }

    public TokenJWTBuilder AddSubject(string subject)
    {
        this.subject = subject;
        return this;
    }

    public TokenJWTBuilder AddIssuer(string issuer)
    {
        this.issuer = issuer;
        return this;
    }

    public TokenJWTBuilder AddAudience(string audience)
    {
        this.audience = audience;
        return this;
    }

    public TokenJWTBuilder AddClaim(string key, string value)
    {
        claims?.Add(key, value);
        return this;
    }

    public TokenJWTBuilder SetExpiresIn(int minutes)
    {
        expiresIn = DateTime.Now.AddMinutes(minutes);
        return this;
    }

    private void EnsureArguments()
    {
        if (securityKey == null)
            throw new ArgumentNullException("SecurityKey");

        if (string.IsNullOrEmpty(subject))
            throw new ArgumentNullException("Subject");

        if (string.IsNullOrEmpty(issuer))
            throw new ArgumentNullException("Issuer");

        if (string.IsNullOrEmpty(audience))
            throw new ArgumentNullException("Audience");
    }

    public TokenJWT Builder()
    {
        EnsureArguments();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }.Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresIn,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

        return new TokenJWT(token);
    }
}
