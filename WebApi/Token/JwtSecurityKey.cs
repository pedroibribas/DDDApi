using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Token
{
    /// <summary>
    /// Método que gera um SymetricSecutiryKey.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string key) => 
            new(Encoding.UTF8.GetBytes(key));
    }
}
