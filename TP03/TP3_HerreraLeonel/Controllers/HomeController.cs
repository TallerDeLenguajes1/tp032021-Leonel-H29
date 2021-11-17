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
    public class HomeController : Controller
    {

        private readonly IDataBase DB;        

        public HomeController(IDataBase dataBase)
        {
            DB = dataBase;
        }

        public IActionResult Index()
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                IndexViewModel UserLog = new IndexViewModel {  
                    usuario = user
                };
                if (UserLog.usuario.Username != null) {
                    return View(UserLog);
                }
                else {
                    return Redirect("~/Usuario/Login");
                }
                
            }
            catch (Exception) {
                return Redirect("~/Usuario/Login");
            }
        }

        public IActionResult _NavAdminPartial()
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                IndexViewModel UserLog = new IndexViewModel {  
                    usuario = user
                };
                if (UserLog.usuario.Username != null) {
                    return PartialView(UserLog);
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
