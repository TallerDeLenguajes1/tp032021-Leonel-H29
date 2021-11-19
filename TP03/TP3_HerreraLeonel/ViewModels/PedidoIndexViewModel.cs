using System;
using System.Collections.Generic;
using TP3_HerreraLeonel.Entities;

namespace TP3_HerreraLeonel.ViewModels
{
   
    public class PedidoIndexViewModel
    {
        public int Nro { get; set; }
        public string Observacion { get; set; }
        public enum Estados { Recibido, En_Camino, Entregado };

        public Cliente Cliente { get; set; }
        public Estados Estado { get; set; }

        public PedidoIndexViewModel()
        {
        }   

        public string EstadoPedido()
        {
            string _Estado = "";
            switch (this.Estado)
            {
                case Estados.Recibido:
                    _Estado = "Recibido";
                    break;
                case Estados.En_Camino:
                    _Estado = "En Camino";
                    break;
                case Estados.Entregado:
                    _Estado = "Entregado";
                    break;
                default:
                    _Estado = "";
                    break;
            }
            return _Estado;
        }
    }

    public class AltaPedidoViewModel
    {
        
        public string Observacion { get; set; }
        public Cliente Cliente { get; set; }
        public List<CadeteIndexViewModel> ListaCadetes { get; set; }
        public CadeteIndexViewModel Cadete { get; set; }
        public IndexViewModel UsuarioLog { get; set; }

        public AltaPedidoViewModel()
        {
        }
    }

    public class ModificarPedidoViewModel
    {
        public int Nro { get; set; }
        public string Observacion { get; set; }
        public enum Estados { Recibido, En_Camino, Entregado };

        public Cliente Cliente { get; set; }
        public Estados Estado { get; set; }

        public ModificarPedidoViewModel()
        {
        }

        public string EstadoPedido()
        {
            string _Estado = "";
            switch (this.Estado)
            {
                case Estados.Recibido:
                    _Estado = "Recibido";
                    break;
                case Estados.En_Camino:
                    _Estado = "En Camino";
                    break;
                case Estados.Entregado:
                    _Estado = "Entregado";
                    break;
                default:
                    _Estado = "";
                    break;
            }
            return _Estado;
        }
    }


}
