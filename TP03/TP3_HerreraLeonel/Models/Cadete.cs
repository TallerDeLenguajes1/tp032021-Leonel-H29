using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_HerreraLeonel.Entities
{
    public class Cadete
    {
        private static int count = 0;
        private int id;
        private string nombre;
        private string direccion;
        private string telefono;
        private List<Pedido> listadoPedidos = new List<Pedido>();

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public List<Pedido> ListadoPedidos { get => listadoPedidos; set => listadoPedidos = value; }

        public Cadete()
        {

        }

        public Cadete(string nombre, string direccion, string telefono)
        {
            count++;
            id = count;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
        }

        public void AgregarPedido(Pedido P)
        {
            ListadoPedidos.Add(P);
        }

        public int Pago()
        {
            int aux = 0;
            foreach (var item in listadoPedidos)
            {
                if (item.Estado == Pedido.Estados.Entregado)
                {
                    aux += 100;
                }
            }
            return aux;
        }
    }
}
