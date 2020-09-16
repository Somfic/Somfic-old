
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;
using SqlKata.Compilers;

namespace Somfic.Database.MySql
{
    /// <inheritdoc />
    public class MySqlDataAccess : DataAccess
    {
        public MySqlDataAccess(ILogger<MySqlDataAccess> log, IConfiguration config) : base(log, config)
        {
        }

        protected override IDbConnection MakeDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        protected override Compiler MakeCompiler()
        {
            return new MySqlCompiler();
        }
    }
}
