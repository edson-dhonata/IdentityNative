using IdentityNative.Models;
using IdentityNative.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityNative.Controllers
{
    public class HomeController : Controller
    {
        [Autorizacao("Usuário, Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        [Autorizacao("Gerente")]
        public IActionResult Privacy()
        {
            return View();
        }

		[Autorizacao("Usuário")]
		public IActionResult Dashboard() 
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}