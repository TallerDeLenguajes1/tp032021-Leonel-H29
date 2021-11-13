using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Models;
using TP3_HerreraLeonel.Entities;
using TP3_HerreraLeonel.ViewModels;
using Microsoft.AspNetCore.Http;

namespace TP3_HerreraLeonel.Controllers
{
    public class UsuarioController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IDataBase DB;


        public UsuarioController(IDataBase dataBase)
        {
            DB = dataBase;
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
            return View(true);
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            
            if (DB.RepoUsuario_Sqlite.IsResgisterUser(username, password))
            {
                HttpContext.Session.SetString("username", username);
                return Redirect("~/Home");
            }
            return View(false);
        }
        
        public IActionResult Logout()
        {

            if (HttpContext.Session.GetString("username")!=null)
            {
                
                HttpContext.Session.Clear();
                return Redirect("~/Usuario/Login");
            }
            return View(false);
        }
        


        //Alta de Usuario
        public IActionResult AltaUsuario(string username, string password)
        {
            if (username == null || password == null)
            {
                //return View(DB.RepositorioCadete.getAll());
                return View(new Usuario());
            }
            else
            {
                Usuario New_usuario = new Usuario(username, password);
                DB.RepoUsuario_Sqlite.InsertUsuarios(New_usuario);
                //return View(DB.RepositorioCadete.getAll());
                return View(new Usuario());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
