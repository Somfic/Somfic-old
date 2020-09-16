
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;

namespace Somfic.Database.MySql
{
    /// <inheritdoc />
    public class MySqlDataAccess : DataAccess
    {
        public MySqlDataAccess(ILogger log, IConfiguration config) : base(log, config)
        {
        }

        protected override IDbConnection MakeDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
