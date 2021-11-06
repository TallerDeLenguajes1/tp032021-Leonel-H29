using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using TP3_HerreraLeonel.Models;

namespace TP3_HerreraLeonel.Entities
{
    public interface IDBSQLite
    {
        SQLiteRepositorioCadete RepositorioCadete { get; set; }
        SQLiteRepositorioPedido RepositorioPedido { get; set; }
    }

    public class DBSQLite : IDBSQLite
    {
        public SQLiteRepositorioCadete RepositorioCadete { get; set; }
        //public IRepositorioCliente RepositorioCliente { get; set; }
        public SQLiteRepositorioPedido RepositorioPedido { get; set; }

        public DBSQLite(string _ConnectionString, ILogger logger)
        {
            RepositorioCadete = new SQLiteRepositorioCadete(_ConnectionString, logger);
            //RepositorioCliente = new SQLiteCliente(_ConnectionString, logger);
            RepositorioPedido = new SQLiteRepositorioPedido(_ConnectionString, logger);

        }
    }


}
