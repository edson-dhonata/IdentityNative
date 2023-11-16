using IdentityNative.Models;
using IdentityNative.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace IdentityNative.Controllers
{
    public class LoginController : Controller
    {
        private readonly Context.Context _context;

        public LoginController(Context.Context context)
        {
            _context = context;
        }

        public IActionResult Index(Login login)
        {
            return View(login);
        }

        [HttpPost]
        public IActionResult Logar([FromServices] TokenService tokenService, Login login)
        {
            ModelState["Nome"].ValidationState = ModelValidationState.Valid;

			if (!ModelState.IsValid)
				return View("Index",login);
			else
            { 
                if(!_context.USUARIOS.Any(x => x.EMAIL.Equals(login.Email)))
                {
                    ViewData["Error"] = "Usuário ou senha inválidos!";
                    return View("Index");
                }            
            
                if(!_context.USUARIOS.Any(x => x.SENHA.Equals(login.Senha)))
                {
                    ViewData["Error"] = "Usuário ou senha inválidos!";
                    return View("Index");
                }            
            
                if(!_context.USUARIOS.Any(x => x.EMAIL.Equals(login.Email) &&
                                               x.SENHA.Equals(login.Senha) &&
                                               x.ATIVO == true)
                  )
                  {
                    ViewData["Error"] = "Usuário Inativo!";
                    return View("Index");
                  }

                var usuario = _context.USUARIOS.Where(x =>
                                                            x.EMAIL.Equals(login.Email) &&
                                                            x.SENHA.Equals(login.Senha) &&
                                                            x.ATIVO == true

                                                     )
                                               .Include(x => x.GRUPOS_USUARIOS)
                                                    .ThenInclude(x => x.GRUPO)
                                                    .ThenInclude(x => x.GRUPOS_PERMISSOES)
                                                    .ThenInclude(x => x.PERMISSAO)
                                               .FirstOrDefault();   

                var token = tokenService.GerarToken(usuario);

                HttpContext.Session.SetString("Token", token);
            
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
