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
using AutoMapper;


namespace TP3_HerreraLeonel.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IDataBase DB;

        private readonly IMapper mapper;

        public PedidoController(IDataBase dataBase, IMapper autoMap)
        {
            DB = dataBase;
            mapper = autoMap;
        }
        
        public IActionResult Index()
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UserVM = mapper.Map<IndexViewModel>(user);
                if (UserVM.Username != null)
                {
                    var ListPedidos = mapper.Map<List<PedidoIndexViewModel>>(DB.RepoPedido_Sqlite.getAllPedidos());
                    return View(new Tuple<List<PedidoIndexViewModel>, IndexViewModel>(ListPedidos, UserVM));
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
        public IActionResult AltaPedido()
        {
            try
            {
                Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                var UserVM = mapper.Map<IndexViewModel>(user);
                if (UserVM.Username != null)
                {
                    var ListCadetes = mapper.Map<List<CadeteIndexViewModel>>(DB.RepoCadete_Sqlite.getAll());
                    AltaPedidoViewModel PedidoVM = new AltaPedidoViewModel();
                    PedidoVM.UsuarioLog = UserVM;
                    PedidoVM.ListaCadetes = ListCadetes;
                    return View(PedidoVM);
                }
                else
                {
                    return Redirect("~/Usuario/Login");
                }
            }
            catch (Exception)
            {
                return Redirect("~/Usuario/Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltaPedido(AltaPedidoViewModel PedidoVM)
        {
            try
            {
                
                //Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                //PedidoVM.UsuarioLog = mapper.Map<IndexViewModel>(user);
                if (ModelState.IsValid)
                {
                    var nuevoPedido = mapper.Map<Pedido>(PedidoVM);
                    Cadete cadeteSeleccionado = DB.RepoCadete_Sqlite.getCadeteAModificar(PedidoVM.idCadete);
                    DB.RepoPedido_Sqlite.InsertPedidos(nuevoPedido, cadeteSeleccionado.Id);
                    return RedirectToAction("Index");
                }
                else
                {
                    //Console.WriteLine("El modelo no es valido");
                    return RedirectToAction("Error");
                }
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Redirect("~/Usuario/Login");
            }
        }
        

         //Muestro el pedido a modificar en el form
         public IActionResult ModificarPedido(int id)
         {
             try
             {
                 Usuario user = DB.RepoUsuario_Sqlite.LoginUser(HttpContext.Session.GetString("username"));
                 var UserVM = mapper.Map<IndexViewModel>(user);
                 if (UserVM != null)
                 {
                    //List<Cadete> ListCadetes = DB.RepoCadete_Sqlite.getAll();
                    //Pedido pedidoADevolver = DB.RepoPedido_Sqlite.getPedidoAModificar(id);
                    var ListCadetes = mapper.Map<List<CadeteIndexViewModel>>(DB.RepoCadete_Sqlite.getAll());
                    var pedidoADevolver = mapper.Map<PedidoIndexViewModel>(DB.RepoPedido_Sqlite.getPedidoAModificar(id));
                     if (pedidoADevolver != null)
                     {
                        ModificarPedidoViewModel modificar = new ModificarPedidoViewModel
                        {
                            UsuarioLog = UserVM,
                            ListaCadetes = ListCadetes,
                            Nro = pedidoADevolver.Nro,
                            Observacion = pedidoADevolver.Observacion,
                            Estado = (ModificarPedidoViewModel.Estados)Enum.Parse(typeof(ModificarPedidoViewModel.Estados), pedidoADevolver.Estado.ToString()) ,
                            Cliente = pedidoADevolver.Cliente,
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModificarPedido(ModificarPedidoViewModel PedidoVM)
         {
             if (ModelState.IsValid)
             {
                Pedido pedidoADevolver = mapper.Map<Pedido>(PedidoVM);
                DB.RepoPedido_Sqlite.UpdatePedidos(pedidoADevolver, PedidoVM.idCadete);
                return Redirect("~/Pedido");
            }
            return RedirectToAction("Error");
         }
         
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