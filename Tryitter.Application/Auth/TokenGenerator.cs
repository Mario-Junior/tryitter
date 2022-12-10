using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tryitter.Models;

namespace Tryitter.Auth;
public class TokenGenerator
{
  public string TokenSecret = "4ed51ab544377ac85d7f0f802aeac690";
  public string Generate(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();

    var tokenDescriptor = new SecurityTokenDescriptor()
    {
      Subject = AddClaims(user),
      SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenSecret)),
        SecurityAlgorithms.HmacSha256Signature),
      Expires = DateTime.Now.AddDays(1)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }

  private static ClaimsIdentity AddClaims(User user)
  {
    var claims = new ClaimsIdentity();

    claims.AddClaim(new Claim("Username", user.Username.ToString()));

    return claims;
  }
}