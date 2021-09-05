using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP03
{
    public enum Estados {Recibido, En_Camino, Entregado};

    class Pedido
    {
        private int nro;
        private string observacion;
        private Cliente cliente;
        private Estados estado;

        public int Nro { get => nro; set => nro = value; }
        public string Observacion { get => observacion; set => observacion = value; }
        
        internal Cliente Cliente { get => cliente; set => cliente = value; }
        public Estados Estado { get => estado; set => estado = value; }

        public Pedido()
        {

        }

        public Pedido(int nro, string observacion, Estados estado)
        {
            Nro = nro;
            Observacion = observacion;
            Estado = estado;
            Cliente = Cliente.CrearCliente();
        }
    }
}
