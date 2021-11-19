﻿using System;
using System.Collections.Generic;


namespace TP3_HerreraLeonel.Entities
{
    public class Cliente
    {
        //private static int count = 0;
        private int id;
        private string nombre;
        private string direccion;
        private string telefono;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Telefono { get => telefono; set => telefono = value; }

        public Cliente()
        {

        }

        public Cliente(string nombre, string direccion, string telefono)
        {
            //Id = count++;
            //Id = id;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
        }
        /*
        public static Cliente CrearCliente()
        {
            return new Cliente();
        }
        */
    }
}
