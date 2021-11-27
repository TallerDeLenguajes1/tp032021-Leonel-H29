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
                    
                    foreach (var ped in ListPedidos)
                    {
                        var _pedido = mapper.Map<Pedido>(ped);
                        ped.getCadete = mapper.Map<CadeteIndexViewModel>(DB.RepoPedido_Sqlite.get_Only_Pedido_delCadete(_pedido));
                    }
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
                if (UserVM.Username != null && UserVM.Username =="admin")
                {
                    var ListCadetes = mapper.Map<List<CadeteIndexViewModel>>(DB.RepoCadete_Sqlite.getAll());
                    AltaPedidoViewModel PedidoVM = new AltaPedidoViewModel();
                    PedidoVM.UsuarioLog = UserVM;
                    PedidoVM.ListaCadetes = ListCadetes;
                    return View(PedidoVM);
                }
                else if (UserVM.Username != null)
                {
                    
                    return RedirectToAction("Index");
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
                if (ModelState.IsValid)
                {
                    var nuevoPedido = mapper.Map<Pedido>(PedidoVM);
                    Cadete cadeteSeleccionado = DB.RepoCadete_Sqlite.getCadeteAModificar(PedidoVM.idCadete);
                    DB.RepoPedido_Sqlite.InsertPedidos(nuevoPedido, cadeteSeleccionado.Id);
                    return RedirectToAction("Index");
                }
                else
                {
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
                 if (UserVM.Username != null && UserVM.Username =="admin")
                 {
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
                 else if (UserVM.Username != null) {
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
            if (HttpContext.Session.GetString("username") == "admin") {
                DB.RepoPedido_Sqlite.DeletePedido(id);
            }
            return Redirect("~/Pedido");
        }

        //Elimino todos los pedidos
        public IActionResult DeleteAll_Pedidos()
        {
            if (HttpContext.Session.GetString("username") == "admin")
            {
                DB.RepoPedido_Sqlite.DeleteAllPedidos();
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