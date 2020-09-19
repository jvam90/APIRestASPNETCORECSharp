using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DesafioPitang.Models;
using Microsoft.IdentityModel.Tokens;

namespace DesafioPitang
{
    public class TokenService
    {
        public static string GenerateToken(User user)
        {
            //Gerador de token - usado para acessar a rota /me que requer autenticação
            var tokenHandler = new JwtSecurityTokenHandler();
            //Recuperando o segredo 
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Criando um Claim baseado no email do usuário
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //Retornando o token gerado
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}