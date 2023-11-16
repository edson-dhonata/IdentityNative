using IdentityNative.Models;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityNative.Services
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Autorizacao : Attribute, IAuthorizationFilter
    {
        private readonly string[] _Permissoes;
        private Login login = new Login();

		public Autorizacao(string permissao)
        {
            _Permissoes = permissao.Split(",");
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContextAccessor = context.HttpContext.RequestServices.GetService<IHttpContextAccessor>();

            var userToken = httpContextAccessor?.HttpContext?.Session.GetString("Token");

			if (userToken == null)
            {
				context.Result = new RedirectToActionResult("Index", "Login", null); // O usuário não está autenticado pois não tem token, redireciona para o login
            }
			else
            {
                var tokenDescript = LerToken(userToken, context);

                var autorizado = false;

                if (tokenDescript != null)
                {
                    foreach (var permissao in _Permissoes)
                    {
                        if (tokenDescript.Claims.Any(x => x.Value.Contains(permissao.Trim())))
                        {
                            autorizado = true;
                            break;
                        }
                    }
                }

                if (!autorizado)
                {
                    login.Error.Add("Erro ao acessar a página solicitada, você não tem acesso a esté recurso!");
				    context.Result = new RedirectToActionResult("Index", "Login", login);
                }
			}
        }

        public ClaimsPrincipal LerToken(string token, AuthorizationFilterContext context)
        {
            try
            {
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// Outros parâmetros de validação, se necessário
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;

				// Obtém as informações do token
				var claims = jwtToken.Claims;

				// Cria e retorna um ClaimsPrincipal com base nas informações do token
				return new ClaimsPrincipal(new ClaimsIdentity(claims));
			}
            catch (Exception ex)
            {
				login.Error.Add(ex.Message);
				return new ClaimsPrincipal(new ClaimsIdentity());
			}
        }
    }
}
