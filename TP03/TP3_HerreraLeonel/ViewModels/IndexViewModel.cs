using System;
using System.Collections.Generic;
using TP3_HerreraLeonel.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TP3_HerreraLeonel.ViewModels
{
    
    public class IndexViewModel
    {
        [Required(ErrorMessage = "El campo Username es requerido")]
        [StringLength(16)]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo Password es requerido")]
        [StringLength(100)]
        public string Password { get; set; }
 
        public IndexViewModel() { }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo Username es requerido")]
        [StringLength(16)]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo Password es requerido")]
        [StringLength(100)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginViewModel() { }
    }

    public class AltaUsuarioViewModel
    {
        [Required(ErrorMessage = "El campo Username es requerido")]
        [StringLength(16)]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo Password es requerido")]
        [StringLength(100)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo Password es requerido")]
        [StringLength(100)]
        public string Confirm_Password { get; set; }

        public string ErrorMessage { get; set; }

        public AltaUsuarioViewModel() { }
    }
}



