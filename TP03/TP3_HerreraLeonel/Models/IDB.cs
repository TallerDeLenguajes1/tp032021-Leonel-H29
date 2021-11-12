using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using TP3_HerreraLeonel.Models;

namespace TP3_HerreraLeonel.Entities
{
    public interface IDataBase
    {
        JSONRepositorioCadete RepoCadete_Json { get; set; }
        SQLiteRepositorioCadete RepoCadete_Sqlite { get; set; }
        JSONRepositorioPedido RepoPedido_Json { get; set; }
        SQLiteRepositorioPedido RepoPedido_Sqlite { get; set; }
        SQLiteRepositorioUsuario RepoUsuario_Sqlite { get; set; }
    }

    public class DataBase : IDataBase
    {
        public SQLiteRepositorioCadete RepoCadete_Sqlite { get; set; }
        public SQLiteRepositorioUsuario RepoUsuario_Sqlite { get; set; }
        //public IRepositorioCliente RepositorioCliente { get; set; }
        public SQLiteRepositorioPedido RepoPedido_Sqlite { get; set; }
        public JSONRepositorioCadete RepoCadete_Json { get; set; }
        public JSONRepositorioPedido RepoPedido_Json { get; set; }

        public DataBase(string _ConnectionString, ILogger logger)
        {
            RepoCadete_Sqlite = new SQLiteRepositorioCadete(_ConnectionString, logger);
            //RepositorioCliente = new SQLiteCliente(_ConnectionString, logger);
            RepoPedido_Sqlite = new SQLiteRepositorioPedido(_ConnectionString, logger);
            RepoUsuario_Sqlite = new SQLiteRepositorioUsuario(_ConnectionString, logger);
            RepoCadete_Json = new JSONRepositorioCadete(logger);
            //RepositorioCliente = new SQLiteCliente(_ConnectionString, logger);
            RepoPedido_Json = new JSONRepositorioPedido(logger);
        }
    }

}
