using IdentityNative.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityNative.Services
{
    public class TokenService
    {
        public string GerarToken(USUARIOS User) 
        {

            //Classe utilizada para gerar nosso token
            var tokenHandler = new JwtSecurityTokenHandler();

            //Pega e criptografa a chave
            var key =  Encoding.ASCII.GetBytes(Configuration.JwtKey);

            List<Claim> Clains = new List<Claim>();

            Clains.Add(new Claim(ClaimTypes.Name, value: User.NOME));
            Clains.Add(new Claim(ClaimTypes.Email, value: User.EMAIL));

			foreach (var grupo in User.GRUPOS_USUARIOS)
			{
				//Clains de grupo
				Clains.Add(new Claim(ClaimTypes.Role, value: grupo.GRUPO.DESCRICAO));

				foreach (var permissoes in grupo.GRUPO.GRUPOS_PERMISSOES)
				{
					//Clains de permissao
					Clains.Add(new Claim(ClaimTypes.Role, value: permissoes.PERMISSAO.DESCRICAO));
				}
            }

            //Cria especificação token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Clains),
                
                //Expiração
                Expires = DateTime.UtcNow.AddMinutes(1),

                //Desencripta nosso token
                SigningCredentials = new SigningCredentials 
                (
                    new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature
                )
            };

            //Cria o token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //Converte o token em string
            return tokenHandler.WriteToken(token);
        }
    }
}
