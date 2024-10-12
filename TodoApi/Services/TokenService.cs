using TodoApi.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;
using System.Security.Claims;

namespace TodoApi.Services;
public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(User user)
    {
        // cria um manipulador para gerar o token
        var handler = new JwtSecurityTokenHandler();

        // criar um array de bytes com a chave gerada
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        //REALIZA A ASSINATURA DO TOKEN E QUAL ALGORITMO PARA ENCRIPTAR O TOKEN
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        //vai gerar um security token 
        var token = handler.CreateToken(tokenDescriptor);

        //vai escrever uma string baseada no token
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));

        return ci;
    }
}