using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_HerreraLeonel.Entities
{
    public class Usuario
    {
        
        private string username;
        private string password;

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
