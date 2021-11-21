using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TP3_HerreraLeonel.Models;
using TP3_HerreraLeonel.Entities;
using TP3_HerreraLeonel.ViewModels;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace TP3_HerreraLeonel.Controllers
{
    public class UsuarioController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IDataBase DB;
        private readonly IMapper mapper;

        public UsuarioController(IDataBase dataBase, IMapper autoMap)
        {
            DB = dataBase;
            mapper = autoMap;
        }

        public IActionResult Index()
        {
            return View();
        }
        //Inicio de seccion
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel UserVM)
        {    
            if (DB.RepoUsuario_Sqlite.IsResgisterUser(UserVM.Username,UserVM.Password))
            {
                HttpContext.Session.SetString("username", UserVM.Username);
                return Redirect("~/Home");
            }
            else
            {
                UserVM.ErrorMessage = "Usuario o contraseña incorrecta";
                return View(UserVM);
            }
            
        }
        
        //Cierro la seccion
        public IActionResult Logout()
        {

            if (HttpContext.Session.GetString("username")!=null)
            {
                
                HttpContext.Session.Clear();
                return Redirect("~/Usuario/Login");
            }
            return View(new LoginViewModel());
        }


        //Alta de Usuario
        public IActionResult AltaUsuario()
        {
            return View(new AltaUsuarioViewModel());
        }

        [HttpPost]
        public IActionResult AltaUsuario(AltaUsuarioViewModel UsuarioVM)
        {
            if (ModelState.IsValid)
            {
                if (UsuarioVM.Password != UsuarioVM.Confirm_Password)
                {
                    UsuarioVM.ErrorMessage = "Las contraseñas deben ser iguales";
                    return View(UsuarioVM);
                }
                else
                {
                    Usuario New_usuario = mapper.Map<Usuario>(UsuarioVM);
                    if (!DB.RepoUsuario_Sqlite.IsResgisterUser(New_usuario.Username, New_usuario.Password))
                    {
                        DB.RepoUsuario_Sqlite.InsertUsuarios(New_usuario);
                        Console.WriteLine("Usuario Creado");
                        return RedirectToAction("Login");
                    }
                    else {
                        UsuarioVM.ErrorMessage = "El usuario '" + UsuarioVM.Username + "' ya se encuentra registrado"; ;
                        return View(UsuarioVM);
                    }
                }
            }
            else {
                UsuarioVM.ErrorMessage = "Ha ocurrido un error. Intente nuevamente";
                return View(UsuarioVM);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
