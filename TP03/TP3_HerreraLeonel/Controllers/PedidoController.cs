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
    public class PedidoController : Controller
    {
        private readonly IDataBase DB;

        public PedidoController(IDataBase dataBase)
        {
            DB = dataBase;
        }
        /*
        public IActionResult Index()
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                IndexViewModel UserLog = new IndexViewModel
                {
                    usuario = user
                };
                if (UserLog.usuario.Username != null)
                {
                    return View(new Tuple<List<Pedido>, IndexViewModel>(DB.RepoPedido_Sqlite.getAllPedidos(), UserLog));
                }
                else
                {
                    return Redirect("~/Usuario/Login");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Redirect("~/Usuario/Login");
            }
        }

       
        
        //Alta de pedidos
        public IActionResult AltaPedido(string _NombreClie, string _DireccionClie, string _TelefonoClie, string _Obs, Pedido.Estados _Estado, int _IdCadete)
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                IndexViewModel UserLog = new IndexViewModel
                {
                    usuario = user
                };
                if (UserLog.usuario.Username != null)
                {
                    if (_NombreClie == null || _DireccionClie == null || _TelefonoClie == null)
                    {
                        return View(new Tuple<List<Cadete>, IndexViewModel>(DB.RepoCadete_Sqlite.getAll(), UserLog));
                    }
                    else
                    {
                        Pedido nuevoPedido = new Pedido(_Obs, _Estado, _NombreClie, _DireccionClie, _TelefonoClie);
                        Cadete cadeteSeleccionado = DB.RepoCadete_Sqlite.getCadeteAModificar(_IdCadete);

                        DB.RepoPedido_Sqlite.InsertPedidos(nuevoPedido, cadeteSeleccionado.Id);

                        return View(new Tuple<List<Cadete>, IndexViewModel>(DB.RepoCadete_Sqlite.getAll(), UserLog));
                    }
                }
                else
                {
                    return Redirect("~/Usuario/Login");
                }    
            }catch(Exception) {
                return Redirect("~/Usuario/Login");
            }
        }
        
        
        //Muestro el pedido a modificar en el form
        public IActionResult ModificarPedido(int id)
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                IndexViewModel UserLog = new IndexViewModel
                {
                    usuario = user
                };
                if (UserLog.usuario.Username != null)
                {
                    List<Cadete> ListCadetes = DB.RepoCadete_Sqlite.getAll();
                    Pedido pedidoADevolver = DB.RepoPedido_Sqlite.getPedidoAModificar(id);
                    

                    if (pedidoADevolver != null)
                    {
                        ModificarPedidoViewModel modificar = new ModificarPedidoViewModel
                        {
                            UsuarioLog = UserLog,
                            listCadetes = ListCadetes,
                            pedido = pedidoADevolver
                        };
                        return View(modificar);
                    }

                    else
                        return Redirect("~/Pedido");
                }
                else
                {
                    return Redirect("~/Usuario/Login");
                }
                
            }
            catch (Exception) {
                return Redirect("~/Usuario/Login");
            }
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
                DB.RepoPedido_Sqlite.UpdatePedidos(pedidoADevolver, _IdCadete);
            }
            return Redirect("~/Pedido");
        }
        */
        //Elimino un pedido
        public IActionResult EliminarPedido(int id)
        {
            DB.RepoPedido_Sqlite.DeletePedido(id);
            return Redirect("~/Pedido");
        }

        //Elimino todos los pedidos
        public IActionResult DeleteAll_Pedidos()
        {
            DB.RepoPedido_Sqlite.DeleteAllPedidos();
            return Redirect("~/Pedido");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}