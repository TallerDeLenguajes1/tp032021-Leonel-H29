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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly List<Cadete> _listaCadete;

        public HomeController(ILogger<HomeController> logger, List<Cadete> listaCadete)
        {
            _logger = logger;
            _listaCadete = listaCadete;
        }

        public IActionResult Index()
        {
            return View(_listaCadete);
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
                _listaCadete.Add(nuevoCadete);
                return View();
            }
        }

        public IActionResult AltaPedidos(string _NombreClie, string _DireccionClie, string _TelefonoClie, string _Obs, Pedido.Estados _Estado, int _IdCadete)
        {
            if (_NombreClie == null || _DireccionClie == null || _TelefonoClie == null)
            {
                return View(_listaCadete);
            }
            else
            {
                Pedido nuevoPedido = new Pedido(_Obs, _Estado, _NombreClie, _DireccionClie, _TelefonoClie);

                foreach (var item in _listaCadete)
                {
                    if (item.Id == _IdCadete)
                    {
                        item.AgregarPedido(nuevoPedido);
                    }
                }

                return View(_listaCadete);
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
