
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Somfic.Database.MySql
{
    /// <inheritdoc />
    public class MySqlDataAccess : IDataAccess
    {
        private readonly ILogger<MySqlDataAccess> _log;
        private readonly IConfiguration _config;

        private string _connectionString;

        public MySqlDataAccess(ILogger<MySqlDataAccess> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(Query query, IDbTransaction transaction = null) where T : class
        {
            try
            {
                QueryFactory db = CreateFactory();
                T result = await db.FirstOrDefaultAsync<T>(query, transaction);
                return result;
            }
            catch (Exception ex)
            {
                SqlResult compiled = CreateCompiler().Compile(query);

                ex.Data.Add("SQL", compiled.Sql);
                ex.Data.Add("Keys", compiled.NamedBindings);
                _log.LogTrace(ex, "Could not get record from database");
                ex.Data.Remove("Keys");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetListAsync<T>(Query query, IDbTransaction transaction = null) where T : class
        {
            try
            {
                QueryFactory db = CreateFactory();
                IEnumerable<T> result = await db.GetAsync<T>(query, transaction);
                return result;
            }
            catch (Exception ex)
            {
                SqlResult compiled = CreateCompiler().Compile(query);

                ex.Data.Add("SQL", compiled.Sql);
                ex.Data.Add("Keys", compiled.NamedBindings);
                _log.LogTrace(ex, "Could not get records from database");
                ex.Data.Remove("Keys");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<int> InsertAsync(Query query, IDbTransaction transaction = null)
        {
            try
            {
                QueryFactory db = CreateFactory();
                int result = await db.ExecuteAsync(query, transaction);
                return result;
            }
            catch (Exception ex)
            {
                SqlResult compiled = CreateCompiler().Compile(query);

                ex.Data.Add("SQL", compiled.Sql);
                ex.Data.Add("Keys", compiled.NamedBindings);
                _log.LogTrace(ex, "Could not insert record in database");
                ex.Data.Remove("Keys");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<int> UpdateAsync(Query query, IDbTransaction transaction = null)
        {
            try
            {
                QueryFactory db = CreateFactory();
                int result = await db.ExecuteAsync(query, transaction);
                return result;
            }
            catch (Exception ex)
            {
                SqlResult compiled = CreateCompiler().Compile(query);

                ex.Data.Add("SQL", compiled.Sql);
                ex.Data.Add("Keys", compiled.NamedBindings);
                _log.LogTrace(ex, "Could not update record in database");
                ex.Data.Remove("Keys");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<int> DeleteAsync(Query query, IDbTransaction transaction = null)
        {
            try
            {
                QueryFactory db = CreateFactory();
                int result = await db.ExecuteAsync(query, transaction);
                return result;
            }
            catch (Exception ex)
            {
                SqlResult compiled = CreateCompiler().Compile(query);

                ex.Data.Add("SQL", compiled.Sql);
                ex.Data.Add("Keys", compiled.NamedBindings);
                _log.LogTrace(ex, "Could not delete record from database");
                ex.Data.Remove("Keys");
                throw;
            }
        }

        private IDbConnection CreateConnection()
        {
            try
            {
                string connectionName = _config.GetSection("ConnectionString")["Active"];
                if (string.IsNullOrWhiteSpace(connectionName)) { _log.LogWarning("No active connection string is set"); }


                _connectionString = _config.GetConnectionString(connectionName);
                if (string.IsNullOrWhiteSpace(_connectionString)) { _log.LogWarning("The connection string is empty"); }

                return new global::MySql.Data.MySqlClient.MySqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Connection string", _connectionString);
                _log.LogTrace(ex, "Could not create a connection to the database");
                throw;
            }
        }

        private Compiler CreateCompiler()
        {
            try
            {
                return new MySqlCompiler();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not create MySqlCompiler");
                throw;
            }
        }

        private QueryFactory CreateFactory()
        {
            return new QueryFactory(CreateConnection(), CreateCompiler());
        }
    }
}
