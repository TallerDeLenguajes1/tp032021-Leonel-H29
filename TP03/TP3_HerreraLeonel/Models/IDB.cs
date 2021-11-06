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

    public interface IDBJSON
    {
        JSONRepositorioCadete RepositorioCadete { get; set; }
        JSONRepositorioPedido RepositorioPedido { get; set; }
    }

    public class DBJSON : IDBJSON
    {
        public JSONRepositorioCadete RepositorioCadete { get; set; }
        public JSONRepositorioPedido RepositorioPedido { get; set; }

        public DBJSON(ILogger logger)
        {
            RepositorioCadete = new JSONRepositorioCadete(logger);
            //RepositorioCliente = new SQLiteCliente(_ConnectionString, logger);
            RepositorioPedido = new JSONRepositorioPedido(logger);

        }
    }
}
