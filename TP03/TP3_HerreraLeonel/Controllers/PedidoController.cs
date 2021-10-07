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
            try
            {
                return View(DBTemporal.leerArchivoPedidos());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Redirect("~/Cadete");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //Alta de pedidos
        public IActionResult AltaPedido(string _NombreClie, string _DireccionClie, string _TelefonoClie, string _Obs, Pedido.Estados _Estado, int _IdCadete)
        {
            List<Pedido> ListPedidos = DBTemporal.leerArchivoPedidos();
            int idMax = 0;
            if (ListPedidos.Count()>0) {
                idMax = ListPedidos.Max(x => x.Nro);
            }
           
            
            if (_NombreClie == null || _DireccionClie == null || _TelefonoClie == null)
            {
                return View(DBTemporal.leerArchivoCadetes());
            }
            else
            {
                Pedido nuevoPedido = new Pedido(idMax+1,_Obs, _Estado, _NombreClie, _DireccionClie, _TelefonoClie);

                List<Cadete> cadeteLista = DBTemporal.leerArchivoCadetes();
                Cadete cadeteSeleccionado = cadeteLista.Find(x => x.Id == _IdCadete);
                cadeteSeleccionado.AgregarPedido(nuevoPedido);

                DBTemporal.AsignarPedidoAlCadete(cadeteSeleccionado);
                dB.Cadeteria.ListadoPedidos = DBTemporal.guardarPedido(nuevoPedido);

                return View(DBTemporal.leerArchivoCadetes());
            }
        }

        //Muestro el pedido a modificar en el form
        public IActionResult ModificarPedido(int id)
        {
            Pedido pedidoADevolver = DBTemporal.VerPedido(id);
            
            if (pedidoADevolver != null)
                return View(pedidoADevolver);
            else
                return Redirect("~/Pedido");
        }

        //Modifico los datos del pedido
        public IActionResult ModificarUnPedido(int id, string _NombreClie, string _DireccionClie, string _TelefonoClie, string _Obs, Pedido.Estados _Estado, int _IdCadete)
        {
            if (id >0)
            {
                Pedido pedidoADevolver = new Pedido();
                pedidoADevolver.Nro = id;
                pedidoADevolver.Cliente.Nombre = _NombreClie;
                pedidoADevolver.Cliente.Direccion = _DireccionClie;
                pedidoADevolver.Cliente.Telefono = _TelefonoClie;
                pedidoADevolver.Observacion = _Obs;
                pedidoADevolver.Estado = _Estado;
                DBTemporal.ModificarPedido(pedidoADevolver);
            }
            return Redirect("~/Pedido");
        }

        //Elimino un pedido
        public IActionResult EliminarPedido(int id)
        {
            DBTemporal.BorrarPedido(id);
            return Redirect("~/Pedido");
        }

        //Elimino todos los pedidos
        public IActionResult DeleteAll_Pedidos()
        {
            dB.Cadeteria.DeleteAllPedidos();
            return View(dB.Cadeteria.ListadoCadetes);
        }
        



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}