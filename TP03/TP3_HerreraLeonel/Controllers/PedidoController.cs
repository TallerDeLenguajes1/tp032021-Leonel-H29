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
        private readonly RepositorioCadete repoCadete;
        private readonly RepositorioPedido repoPedido;

        public PedidoController(ILogger<PedidoController> logger, RepositorioPedido repo, RepositorioCadete RepoCadetes)
        {
            _logger = logger;
            repoPedido = repo;
            repoCadete = RepoCadetes;
        }

        public IActionResult Index()
        {
            try
            {
                return View(repoPedido.getAllPedidos());
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
            /*
            List<Pedido> ListPedidos = repoPedido.getAllPedidos();
            int idMax = 0;
            if (ListPedidos.Count()>0) {
                idMax = ListPedidos.Max(x => x.Nro);
            }
            */
           
            
            if (_NombreClie == null || _DireccionClie == null || _TelefonoClie == null)
            {
                return View(repoCadete.getAll());
            }
            else
            {
                List<Cliente> ListClienteDB = repoPedido.getAllClientes();
                
                int idMaxCliente = 0;

                if (ListClienteDB.Count() > 0)
                {
                    idMaxCliente= ListClienteDB.Max(x => x.Id);
                }
                
                Pedido nuevoPedido = new Pedido(_Obs, _Estado, idMaxCliente+1,_NombreClie, _DireccionClie, _TelefonoClie);
                List<Cadete> cadeteLista = repoCadete.getAll();
                Cadete cadeteSeleccionado = repoCadete.getCadeteAModificar(_IdCadete);
                cadeteSeleccionado.AgregarPedido(nuevoPedido);

                
                repoPedido.InsertPedidos(nuevoPedido, cadeteSeleccionado.Id);

                return View(repoCadete.getAll());
            }
        }

        
        //Muestro el pedido a modificar en el form
        public IActionResult ModificarPedido(int id)
        {
            List<Cadete> ListCadetes = repoCadete.getAll();
            Pedido pedidoADevolver = repoPedido.getPedidoAModificar(id);

            if (pedidoADevolver != null)
                return View(new Tuple<Pedido, List<Cadete>>(pedidoADevolver, ListCadetes));
            else
                return Redirect("~/Pedido");
        }

        //Modifico los datos del pedido
        public IActionResult ModificarUnPedido(int id, int id_cli ,string _NombreClie, string _DireccionClie, string _TelefonoClie, string _Obs, Pedido.Estados _Estado, int _IdCadete)
        {
            if (id >0 && _IdCadete>0)
            {
                Pedido pedidoADevolver = new Pedido();
                pedidoADevolver.Nro = id;
                pedidoADevolver.Cliente.Id = id_cli;
                pedidoADevolver.Cliente.Nombre = _NombreClie;
                pedidoADevolver.Cliente.Direccion = _DireccionClie;
                pedidoADevolver.Cliente.Telefono = _TelefonoClie;
                pedidoADevolver.Observacion = _Obs;
                pedidoADevolver.Estado = _Estado;
                repoPedido.UpdatePedidos(pedidoADevolver, _IdCadete);
            }
            return Redirect("~/Pedido");
        }
        
        //Elimino un pedido
        public IActionResult EliminarPedido(int id)
        {
            repoPedido.DeletePedido(id);
            return Redirect("~/Pedido");
        }

        //Elimino todos los pedidos
        public IActionResult DeleteAll_Pedidos()
        {
            repoPedido.DeleteAllPedidos();
            return Redirect("~/Pedido");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}