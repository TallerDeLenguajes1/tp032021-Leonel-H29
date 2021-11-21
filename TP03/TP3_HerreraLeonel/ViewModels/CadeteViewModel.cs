using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TP3_HerreraLeonel.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace TP3_HerreraLeonel.ViewModels
{
    public class CadeteIndexViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")][StringLength(100)]
        public string Nombre { get; set; }

        [ValidateNever][StringLength(200)]
        public string Direccion { get; set; }

        [ValidateNever][StringLength(20)]
        public string Telefono { get; set; }

        public List<Pedido> ListadoPedidos { get; set; }

        //public IndexViewModel UsuarioLog { get; set; }

        public CadeteIndexViewModel() { }

        public int Pago()
        {
            int aux = 0;
            foreach (var item in this.ListadoPedidos)
            {
                if (item.EstadoPedido() == "Entregado")
                {
                    aux += 100;
                }
            }
            return aux;
        }
    }

    public class CadeteAltaViewModel
    {
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [ValidateNever]
        [StringLength(200)]
        public string Direccion { get; set; }

        [ValidateNever]
        [StringLength(20)]
        public string Telefono { get; set; }

        public List<Pedido> ListadoPedidos { get; set; }

        public IndexViewModel UsuarioLog { get; set; }

        public CadeteAltaViewModel() { }
    }

    public class CadeteModificarViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [ValidateNever]
        [StringLength(200)]
        public string Direccion { get; set; }

        [ValidateNever]
        [StringLength(20)]
        public string Telefono { get; set; }

        public List<Pedido> ListadoPedidos { get; set; }

        public IndexViewModel UsuarioLog { get; set; }

        public CadeteModificarViewModel() { }
    }

}
