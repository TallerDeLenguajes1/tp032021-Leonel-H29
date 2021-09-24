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
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;
        private readonly DBTemporal dB;

        public PedidoController(ILogger<PedidoController> logger, DBTemporal dataBase)
        {
            _logger = logger;
            dB = dataBase;
            //_listaCadete = listaCadete;
        }

        public IActionResult Index()
        {
            return View(dB.Cadeteria.ListadoPedidos);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult AltaPedido(string _NombreClie, string _DireccionClie, string _TelefonoClie, string _Obs, Pedido.Estados _Estado, int _IdCadete)
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
                //dB.Cadeteria.ListadoPedidos.Add(nuevoPedido);
                dB.Cadeteria.ListadoPedidos = DBTemporal.guardarPedido(nuevoPedido);

                return View(dB.Cadeteria.ListadoCadetes);
            }
        }


        public IActionResult ModificarPedido(int id)
        {
            Pedido pedidoADevolver = null;
            for (int i = 0; i < dB.Cadeteria.ListadoPedidos.Count(); i++)
            {
                if (dB.Cadeteria.ListadoPedidos[i].Nro == id)
                {
                    pedidoADevolver = dB.Cadeteria.ListadoPedidos[i];
                    break;
                }
            }
            if (pedidoADevolver != null)
                return View(pedidoADevolver);
            else
                return Redirect("Index");
        }

        public IActionResult ModificarUnPedido(int id, string _NombreClie, string _DireccionClie, string _TelefonoClie, string _Obs, Pedido.Estados _Estado, int _IdCadete)
        {
            Pedido pedidoADevolver = null;
            for (int i = 0; i < dB.Cadeteria.ListadoPedidos.Count(); i++)
            {
                if (dB.Cadeteria.ListadoPedidos[i].Nro == id)
                {
                    pedidoADevolver = dB.Cadeteria.ListadoPedidos[i];
                    break;
                }
            }
            if (pedidoADevolver != null)
            {
                pedidoADevolver.Cliente.Nombre = _NombreClie;
                pedidoADevolver.Cliente.Direccion = _DireccionClie;
                pedidoADevolver.Cliente.Telefono = _TelefonoClie;
                pedidoADevolver.Observacion = _Obs;
                pedidoADevolver.Estado = _Estado;
            }
            return Redirect("Index");
        }

        public IActionResult EliminarPedido(int id)
        {
            //Pedido pedidoAEliminar = null;
            for (int i = 0; i < dB.Cadeteria.ListadoPedidos.Count(); i++)
            {
                if (dB.Cadeteria.ListadoPedidos[i].Nro == id)
                {
                    dB.Cadeteria.ListadoPedidos = DBTemporal.borrarPedido(i);
                    //pedidoAEliminar = dB.Cadeteria.ListadoPedidos[i];
                    //dB.Cadeteria.ListadoPedidos.Remove(pedidoAEliminar);
                    break;
                }
            }
            return Redirect("~/Pedido");
        }
        
        public IActionResult DeleteAll_Pedidos()
        {
            for (int i = 0; i < dB.Cadeteria.ListadoPedidos.Count(); i++)
            {
                dB.Cadeteria.ListadoPedidos = DBTemporal.borrarPedido(i);    
            }
            return Redirect("~/Pedido");
        }
        





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}