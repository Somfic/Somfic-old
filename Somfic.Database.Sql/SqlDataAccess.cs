using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlKata.Compilers;

namespace Somfic.Database.Sql
{
    public class SqlDataAccess : DataAccess
    {
        public SqlDataAccess(ILogger<SqlDataAccess> log, IConfiguration config) : base(log, config)
        {
        }

        protected override IDbConnection MakeDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        protected override Compiler MakeCompiler()
        {
            return new SqlServerCompiler();
        }
    }
}