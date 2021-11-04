using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_HerreraLeonel.Entities
{
   
    public class Pedido
    {
        //private static int count = 0;
        public enum Estados { Recibido, En_Camino, Entregado };
        private int nro;
        private string observacion;
        private Cliente cliente;
        private Estados estado;

        public int Nro { get => nro; set => nro = value; }
        public string Observacion { get => observacion; set => observacion = value; }
        
        public Cliente Cliente { get => cliente; set => cliente = value; }
        public Estados Estado { get => estado; set => estado = value; }

        public Pedido()
        {
            this.Cliente = new Cliente();
        }

        public Pedido(int numero,string observacion, Estados estado, int ID,string _Nombre, string _Direccion, string _Telefo)
        {
            //count++;
            //this.Nro = count;
            this.Nro = numero;
            this.Observacion = observacion;
            this.Estado = estado;
            this.Cliente = new Cliente(ID, _Nombre, _Direccion, _Telefo);
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
