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
using AutoMapper;


namespace TP3_HerreraLeonel.Controllers
{
    public class HomeController : Controller
    {

        private readonly IDataBase DB;
        private readonly IMapper mapper;

        public HomeController(IDataBase dataBase, IMapper autoMap)
        {
            DB = dataBase;
            mapper = autoMap;
        }

        public IActionResult Index()
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UserVM = mapper.Map<IndexViewModel>(user);
                if (UserVM.Username != null) {
                    return View(UserVM);
                }
                else {
                    return Redirect("~/Usuario/Login");
                }
                
            }
            catch (Exception) {
                return Redirect("~/Usuario/Login");
            }
        }

        //Muestro el user logueado en el menu de navegacion
        public IActionResult _NavAdminPartial()
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UserVM = mapper.Map<IndexViewModel>(user);


                if (UserVM.Username != null) {
                    return PartialView(UserVM);
                }
                else {
                    return Redirect("~/Usuario/Login");
                }
                
            }
            catch (Exception) {
                return Redirect("~/Usuario/Login");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
