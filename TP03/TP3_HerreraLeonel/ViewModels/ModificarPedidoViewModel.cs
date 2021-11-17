using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Entities;
using TP3_HerreraLeonel.Models;

namespace TP3_HerreraLeonel.ViewModels
{
    
    public class ModificarPedidoViewModel
    {
        public IndexViewModel UsuarioLog { get; set; }
        public Pedido pedido { get; set; }
        public List<Cadete> listCadetes { get; set; }
    }
}
