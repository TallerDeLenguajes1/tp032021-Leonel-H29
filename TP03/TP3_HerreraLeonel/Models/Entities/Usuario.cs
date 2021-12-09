using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_HerreraLeonel.Entities
{
    public class Usuario
    {
        private int id_usuario;
        private string username;
        private string password;

        public int Id_usuario { get => id_usuario; set => id_usuario = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        public Usuario()
        {

        }

        public Usuario(string user, string pass)
        {
            //count++;
            this.username = user;
            this.password = pass;     
        }

    }
}
