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
        public IActionResult AltaCadetes(string _Nombre, string _Direccion, string _Telefono)
        {
            if (_Nombre == null || _Direccion == null || _Telefono == null)
            {
                return View();
            }
            else
            {
                Cadete nuevoCadete = new Cadete(_Nombre, _Direccion, _Telefono);
                dB.Cadeteria.ListadoCadetes.Add(nuevoCadete);
                return View();
            }
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
