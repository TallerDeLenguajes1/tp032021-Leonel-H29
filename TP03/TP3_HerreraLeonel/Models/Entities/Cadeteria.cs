using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_HerreraLeonel.Entities
{
    public class Cadeteria
    {
        public List<Cadete> ListadoCadetes { get; set; }
        public List<Pedido> ListadoPedidos { get; set; }

        public Cadeteria()
        {
            ListadoCadetes = new List<Cadete>();
            ListadoPedidos = new List<Pedido>();
        }
    }
}
