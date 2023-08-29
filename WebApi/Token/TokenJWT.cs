using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Token;

/// <summary>
/// Classe que disponibiliza informações sobre o token JWT.
/// </summary>
public class TokenJWT
{
    private readonly JwtSecurityToken _token;

    internal TokenJWT(JwtSecurityToken token)
    {
        _token = token;
    }

    public DateTime ExpiresIn => _token.ValidTo;

    public string Value => new JwtSecurityTokenHandler().WriteToken(_token);
}
