using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Models;
using TP3_HerreraLeonel.Entities;

namespace TP3_HerreraLeonel.Controllers
{
    public class CadeteController : Controller
    {
        private readonly ILogger<CadeteController> _logger;
        private readonly DBTemporal dB;

        public CadeteController(ILogger<CadeteController> logger, DBTemporal dataBase)
        {
            _logger = logger;
            dB = dataBase;
        }

        public IActionResult Index()
        {
            try {
                return View(DBTemporal.leerArchivoCadetes());
            }
            catch (Exception ex){
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
                return View(dB.Cadeteria.ListadoCadetes);
            }
            else
            {
                Cadete nuevoCadete = new Cadete(_Nombre, _Direccion, _Telefono);
                //dB.Cadeteria.ListadoCadetes.Add(nuevoCadete);
                dB.Cadeteria.ListadoCadetes = DBTemporal.guardarCadete(nuevoCadete);
                return View(dB.Cadeteria.ListadoCadetes);
            }
        }

        //Muestro los datos del cadete en el form de edicion
        public IActionResult ModificarCadete(int id)
        {
            Cadete cadeteADevolver = DBTemporal.VerCadete(id);
            
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
                DBTemporal.ModificarCadete(cadeteAModificar);
            }
            
            return Redirect("~/Cadete");
        }
        
        //Elimino el cadete
        public IActionResult EliminarCadete(int id)
        {
            DBTemporal.BorrarCadete(id);
            return Redirect("~/Cadete");
        }
        //Elimino todos los cadetes
        /*
        public IActionResult DeleteAll_Cadetes()
        {
            DBTemporal.BorrarTodosLosCadete();
            return Redirect("~/Cadete");
        }
        */

        public void CargarCadete(string _Nombre, string _Direccion, string _Telefono)
        {
            Cadete nuevoCadete = new Cadete(_Nombre, _Direccion, _Telefono);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
