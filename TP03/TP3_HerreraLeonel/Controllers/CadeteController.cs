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
            return View(dB.Cadeteria.ListadoCadetes);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AltaCadete(string _Nombre, string _Direccion, string _Telefono)
        {
            if (_Nombre == null || _Direccion == null || _Telefono == null)
            {
                return View(dB.Cadeteria.ListadoCadetes);
            }
            else
            {
                Cadete nuevoCadete = new Cadete(_Nombre, _Direccion, _Telefono);
                dB.Cadeteria.ListadoCadetes.Add(nuevoCadete);
                return View(dB.Cadeteria.ListadoCadetes);
            }
        }

        public IActionResult ModificarCadete(int id)
        {
            //Cadete nuevoCadete = new Cadete(_Nombre, _Direccion, _Telefono);
            //dB.Cadeteria.ListadoCadetes.Add(nuevoCadete);
            //return View(dB.Cadeteria.ListadoCadetes);
            Cadete cadeteADevolver = null;
            for (int i = 0; i < dB.Cadeteria.ListadoCadetes.Count(); i++)
            {
                if (dB.Cadeteria.ListadoCadetes[i].Id == id)
                {
                    cadeteADevolver = dB.Cadeteria.ListadoCadetes[i];
                    break;
                }
            }
            if (cadeteADevolver != null)
                return View(cadeteADevolver);
            else
                return Redirect("Index");
        }

        public IActionResult ModificarUnCadete(int id, string _Nombre, string _Direccion, string _Telefono)
        {
            //Cadete nuevoCadete = new Cadete(_Nombre, _Direccion, _Telefono);
            //dB.Cadeteria.ListadoCadetes.Add(nuevoCadete);
            //return View(dB.Cadeteria.ListadoCadetes);
            Cadete cadeteAModificar = null;
            for (int i = 0; i < dB.Cadeteria.ListadoCadetes.Count(); i++)
            {
                if (dB.Cadeteria.ListadoCadetes[i].Id == id)
                {
                    cadeteAModificar = dB.Cadeteria.ListadoCadetes[i];
                    break;
                }
            }
            if (cadeteAModificar != null)
            {
                cadeteAModificar.Nombre = _Nombre;
                cadeteAModificar.Direccion = _Direccion;
                cadeteAModificar.Telefono = _Telefono;
            }
            return Redirect("Index");
        }
        public void EliminarCadete(int id)
        {
            for(int i=0; i<dB.Cadeteria.ListadoCadetes.Count(); i++)
            {
                if (dB.Cadeteria.ListadoCadetes[i].Id == id)
                {
                    dB.Cadeteria.ListadoCadetes.Remove(dB.Cadeteria.ListadoCadetes[i]);
                    break;
                }
            }
            //redirect! 
        }
        public void DeleteAll_Cadetes()
        {
            dB.Cadeteria.DeleteAllCadetes();   
        }

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
