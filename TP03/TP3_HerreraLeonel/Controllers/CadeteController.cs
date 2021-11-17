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
    public class CadeteController : Controller
    {
        private readonly IDataBase DB;
       
        public CadeteController(IDataBase dataBase)
        {
            DB = dataBase;
        }

        public IActionResult Index()
        {
            try {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                IndexViewModel UserLog = new IndexViewModel
                {
                    usuario = user
                };
                if (UserLog.usuario.Username != null)
                {
                    return View(new Tuple<List<Cadete>,IndexViewModel>(DB.RepoCadete_Sqlite.getAll(), UserLog));
                }
                else
                {
                    return Redirect("~/Usuario/Login");
                }
                //return View(DB.RepoCadete_Sqlite.getAll());
            }
            catch (Exception ex){
                Console.WriteLine(ex);
                return Redirect("~/Cadete"); 
            }
                
        }

        //Vista de los pedidos asignados a cada cadete
        public IActionResult ListPedidos(int id)
        {
            List<Cadete> ListCadetes = DB.RepoCadete_Sqlite.getAll();
            
            try
            {
                return View(ListCadetes.Where(cad => cad.Id == id).Single());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Redirect("~/Cadete");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Alta de Cadete
        public IActionResult AltaCadete(string _Nombre, string _Direccion, string _Telefono)
        {
            if (_Nombre == null || _Direccion == null || _Telefono == null)
            {
                return View(DB.RepoCadete_Sqlite.getAll());
            }
            else
            {
                Cadete nuevoCadete = new Cadete(_Nombre, _Direccion, _Telefono);
                DB.RepoCadete_Sqlite.InsertCadetes(nuevoCadete);//Inserto en la DB
                DB.RepoCadete_Json.InsertCadetes(nuevoCadete);//Inserto en el Archivo Json
                return View(DB.RepoCadete_Sqlite.getAll());
            }
        }
        
        //Muestro los datos del cadete en el form de edicion
        public IActionResult ModificarCadete(int id)
        {
            Cadete cadeteADevolver = DB.RepoCadete_Sqlite.getCadeteAModificar(id);

            if (cadeteADevolver != null)
                return View(cadeteADevolver);
            else
                return Redirect("~/Cadete");
        }

        //Modifico los datos del cadete
        public IActionResult ModificarUnCadete(int id, string _Nombre, string _Direccion, string _Telefono)
        {
            if (id > 0)
            {
                Cadete cadeteAModificar = new Cadete();
                cadeteAModificar.Id = id;
                cadeteAModificar.Nombre = _Nombre;
                cadeteAModificar.Direccion = _Direccion;
                cadeteAModificar.Telefono = _Telefono;
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
