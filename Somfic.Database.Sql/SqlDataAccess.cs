using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
    }
}