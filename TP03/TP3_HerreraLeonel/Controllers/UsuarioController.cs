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
            Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
            var UserVM = mapper.Map<IndexViewModel>(user);
            if (UserVM.Username != null && UserVM.Username == "admin")
            {
                var ListUsuariosVM = mapper.Map<List<IndexViewModel>>(DB.RepoUsuario_Sqlite.getAll());

                return View(new Tuple<List<IndexViewModel>, IndexViewModel>(ListUsuariosVM, UserVM));

            }
            
            else
            {
                if (UserVM.Username != null)
                {
                    return Redirect("~/Home");
                    
                }
                else {
                     return Redirect("~/Usuario/Login");
                }
            }
        }

        //Inicio de seccion
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("username") == null)
                return View(new LoginViewModel());
            else
                return Redirect("~/Home");
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
            if (HttpContext.Session.GetString("username") == null)
            {
                return View(new AltaUsuarioViewModel());
            }
            return Redirect("~/Home");
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


        //Muestro los datos del usuario en el form de edicion
        public IActionResult ModificarUsuario(int id)
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UsuarioMV = mapper.Map<IndexViewModel>(user);

                if (UsuarioMV.Username != null && UsuarioMV.Username == "admin")
                {
                    var usuarioADevolver = mapper.Map<ModificarUsuarioViewModel>(DB.RepoUsuario_Sqlite.getUsuarioAModificar(id));
                    usuarioADevolver.UsuarioLog = UsuarioMV;

                    if (usuarioADevolver != null)
                        return View(usuarioADevolver);
                    else
                        return RedirectToAction("Index");
                }
                else if (UsuarioMV.Username != null)
                {

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception) { return RedirectToAction("Login"); }
        }

        //Modifico los datos del usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModificarUsuario(ModificarUsuarioViewModel UsuarioVM)
        {
            if (ModelState.IsValid && UsuarioVM.Id_usuario>0)
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UsuarioLogMV = mapper.Map<IndexViewModel>(user);
                UsuarioVM.UsuarioLog = UsuarioLogMV;

                if (UsuarioVM.Password==UsuarioVM.Confirm_Password)
                {
                    var usuarioAModificar = mapper.Map<Usuario>(UsuarioVM);
                    DB.RepoUsuario_Sqlite.UpdateUsuarios(usuarioAModificar);
                    return RedirectToAction("Index");
                }
                else
                {
                    UsuarioVM.ErrorMessage = "Las contraseñas tienen que coincidir";
                    return View(UsuarioVM);
                }
            }
            return RedirectToAction("Error");

        }

        //Elimino el cadete
        public IActionResult EliminarUsuario(int id)
        {
            if (HttpContext.Session.GetString("username") == "admin")
            {
                DB.RepoUsuario_Sqlite.DeleteUsuarios(id);
            }

            return RedirectToAction("Index");
        }

        //Elimino todos los cadetes
        public IActionResult DeleteAll_Usuarios()
        {
            if (HttpContext.Session.GetString("username") == "admin")
            {
                DB.RepoUsuario_Sqlite.DeleteAllUsers();
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
