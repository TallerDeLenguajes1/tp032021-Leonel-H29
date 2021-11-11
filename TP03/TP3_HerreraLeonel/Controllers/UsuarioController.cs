using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Models;
using TP3_HerreraLeonel.Entities;
using Microsoft.AspNetCore.Http;

namespace TP3_HerreraLeonel.Controllers
{
    public class UsuarioController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly SQLiteRepositorioUsuario repoUsuario;


        public UsuarioController(ILogger<UsuarioController> logger, SQLiteRepositorioUsuario usuario)
        {
            //_logger = logger;
            repoUsuario = usuario;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (repoUsuario.LoginUser(username, password))
            {
                HttpContext.Session.SetString("username", username);
                return Redirect("~/Home");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
