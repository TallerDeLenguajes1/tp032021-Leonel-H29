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
    public class PedidosController : Controller
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly DBTemporal dB;

        public PedidosController(ILogger<PedidosController> logger, DBTemporal dataBase)
        {
            _logger = logger;
            dB = dataBase;
            //_listaCadete = listaCadete;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
       

        public IActionResult AltaPedidos(string _NombreClie, string _DireccionClie, string _TelefonoClie, string _Obs, Pedido.Estados _Estado, int _IdCadete)
        {
            if (_NombreClie == null || _DireccionClie == null || _TelefonoClie == null)
            {
                return View(dB.Cadeteria.ListadoCadetes);
            }
            else
            {
                Pedido nuevoPedido = new Pedido(_Obs, _Estado, _NombreClie, _DireccionClie, _TelefonoClie);

                foreach (var item in dB.Cadeteria.ListadoCadetes)
                {
                    if (item.Id == _IdCadete)
                    {
                        item.AgregarPedido(nuevoPedido);
                    }
                }

                return View(dB.Cadeteria.ListadoCadetes);
            }
        }


        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
