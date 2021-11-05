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
        //private readonly DBTemporal dB;
        private readonly RepositorioCadete repoCadete;

        public CadeteController(ILogger<CadeteController> logger, RepositorioCadete RepoCadetes)
        {
            _logger = logger;
            //dB = dataBase;
            repoCadete = RepoCadetes;
        }

        public IActionResult Index()
        {
            try {
                return View(repoCadete.getAll());
            }
            catch (Exception ex){
                Console.WriteLine(ex);
                return Redirect("~/Cadete"); 
            }
                
        }
        
        //Vista de los pedidos asignados a cada cadete
        public IActionResult ListPedidos(int id)
        {
            List<Cadete> ListCadetes = repoCadete.getAll();
            
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
                return View(repoCadete.getAll());
            }
            else
            {
                Cadete nuevoCadete = new Cadete(_Nombre, _Direccion, _Telefono);
                repoCadete.InsertCadetes(nuevoCadete);
                return View(repoCadete.getAll());
            }
        }
        
        //Muestro los datos del cadete en el form de edicion
        public IActionResult ModificarCadete(int id)
        {
            Cadete cadeteADevolver = repoCadete.getCadeteAModificar(id);

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
                repoCadete.UpdateCadetes(cadeteAModificar);
            }
            
            return Redirect("~/Cadete");
        }
        
        //Elimino el cadete
        public IActionResult EliminarCadete(int id)
        {
            repoCadete.DeleteCadetes(id);
            return Redirect("~/Cadete");
        }

        //Elimino todos los cadetes
        public IActionResult DeleteAll_Cadetes()
        {
            repoCadete.DeleteAllCadetes();
            return Redirect("~/Cadete");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
