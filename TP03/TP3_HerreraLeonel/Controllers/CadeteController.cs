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
    public class CadeteController : Controller
    {
        private readonly IDataBase DB;
        private readonly IMapper mapper;
       
        public CadeteController(IDataBase dataBase, IMapper autoMap)
        {
            DB = dataBase;
            mapper = autoMap;
        }
        
        public IActionResult Index()
        {
            try {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UsuarioMV = mapper.Map<IndexViewModel>(user);
                var CadetesMV = mapper.Map<List<CadeteIndexViewModel>>(DB.RepoCadete_Sqlite.getAll());
                if (UsuarioMV.Username != null)
                {
                    return View(new Tuple<List<CadeteIndexViewModel>,IndexViewModel>(CadetesMV, UsuarioMV));
                }
                else
                {
                    return Redirect("~/Usuario/Login");
                }
            }
            catch (Exception){
                return Redirect("~/Usuario/Login");
            }            
        }
        
        //Vista de los pedidos asignados a cada cadete
        public IActionResult ListPedidos(int id)
        {
            List<Cadete> ListCadetes = DB.RepoCadete_Sqlite.getAll();
            
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UsuarioMV = mapper.Map<IndexViewModel>(user);
                var CadeteMV = mapper.Map<CadeteIndexViewModel>(ListCadetes.Where(cad => cad.Id == id).Single());
                if (UsuarioMV.Username != null)
                {
                    return View(new Tuple<CadeteIndexViewModel, IndexViewModel>( CadeteMV, UsuarioMV));
                }
                else
                {
                    return Redirect("~/Usuario/Login");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Redirect("~/Cadete");
            }
        }

        //Alta de Cadete 
        public IActionResult AltaCadete()
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UsuarioMV = mapper.Map<IndexViewModel>(user);

                if (UsuarioMV.Username != null)
                {
                    var CadeteVM = new CadeteAltaViewModel();
                    CadeteVM.UsuarioLog = UsuarioMV;
                    return View(CadeteVM);
                }
                else
                {
                    return Error();
                }

            }
            catch (Exception)
            {
                return Redirect("~/Usuario/Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaCadete(CadeteAltaViewModel CadeteVM)
        {
            
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                CadeteVM.UsuarioLog = mapper.Map<IndexViewModel>(user);

                if (CadeteVM.UsuarioLog != null && ModelState.IsValid)
                {
                    Cadete nuevoCadete = mapper.Map<Cadete>(CadeteVM);
                    DB.RepoCadete_Sqlite.InsertCadetes(nuevoCadete);//Inserto en la DB
                    DB.RepoCadete_Json.InsertCadetes(nuevoCadete);//Inserto en el Archivo Json
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Error");
                }

            }
            catch (Exception)
            {
                return Redirect("~/Usuario/Login");
            }
        }

        
        //Muestro los datos del cadete en el form de edicion
        public IActionResult ModificarCadete(int id)
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UsuarioMV = mapper.Map<IndexViewModel>(user);
                
                if (UsuarioMV.Username != null)
                {
                    var cadeteADevolver = mapper.Map<CadeteModificarViewModel>(DB.RepoCadete_Sqlite.getCadeteAModificar(id));
                    cadeteADevolver.UsuarioLog = UsuarioMV;

                    if (cadeteADevolver != null)
                        return View(cadeteADevolver);
                    else
                        return Redirect("~/Cadete");
                }
                else
                {
                    return Redirect("~/Usuario/Login");
                }
            }
            catch (Exception) { return Redirect("~/Usuario/Login"); }       
        }

        //Modifico los datos del cadete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModificarCadete(CadeteModificarViewModel CadeteVM)
        {
            if (ModelState.IsValid && CadeteVM.Id > 0)
            {
                var cadeteAModificar = mapper.Map<Cadete>(CadeteVM);
                DB.RepoCadete_Sqlite.UpdateCadetes(cadeteAModificar);
                DB.RepoCadete_Json.UpdateCadetes(cadeteAModificar);
            }
            
            return Redirect("~/Cadete");
        }
        
        //Elimino el cadete
        public IActionResult EliminarCadete(int id)
        {
            DB.RepoCadete_Sqlite.DeleteCadetes(id);
            DB.RepoCadete_Json.DeleteCadetes(id);
            return Redirect("~/Cadete");
        }

        //Elimino todos los cadetes
        public IActionResult DeleteAll_Cadetes()
        {
            DB.RepoCadete_Sqlite.DeleteAllCadetes();
            DB.RepoCadete_Json.DeleteAllCadetes();
            return Redirect("~/Cadete");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
