using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_Herrera_Leonel.Entities
{
    public class Cadeteria
    {
        private string nombre;
        private List<Cadete> listadoCadetes;

        public string Nombre { get => nombre; set => nombre = value; }
        internal List<Cadete> ListadoCadetes { get => listadoCadetes; set => listadoCadetes = value; }

        public Cadeteria()
        {

        }

        public Cadeteria(string nombre)
        {
            Nombre = nombre;
            ListadoCadetes = new List<Cadete>();
        }
    }
}
