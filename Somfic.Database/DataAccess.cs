using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Somfic.Database
{
    /// <inheritdoc />
    public abstract class DataAccess : IDataAccess
    {
        private readonly ILogger _log;
        private readonly IConfiguration _config;

        protected DataAccess(ILogger log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        protected abstract IDbConnection MakeDbConnection(string connectionString);

        /// <inheritdoc />
        public async Task<T> FirstAsync<T>(Query query, IDbTransaction transaction = null) where T : class
        {
            return await FirstAsync<T>(query, transaction, "Could not get record");
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> ListAsync<T>(Query query, IDbTransaction transaction = null) where T : class
        {
            return await ListAsync<T>(query, transaction, "Could not get records");
        }

        /// <inheritdoc />
        public async Task<int> InsertAsync(Query query, IDbTransaction transaction = null)
        {
            return await ExecuteAsync(query, transaction, "Could not insert record");
        }

        /// <inheritdoc />
        public async Task<int> UpdateAsync(Query query, IDbTransaction transaction = null)
        {
            return await ExecuteAsync(query, transaction, "Could not update record");
        }

        /// <inheritdoc />
        public async Task<int> DeleteAsync(Query query, IDbTransaction transaction = null)
        {
            return await ExecuteAsync(query, transaction, "Could not delete record");
        }

        protected async Task<int> ExecuteAsync(Query query, IDbTransaction transaction, string errorMessage)
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

                AddExceptionData(ex, compiled);
                _log.LogTrace(ex, errorMessage);
                RemoveSensitiveExceptionData(ex);
                throw;
            }
        }

        protected async Task<T> FirstAsync<T>(Query query, IDbTransaction transaction, string errorMessage)
        {
            try
            {
                QueryFactory db = CreateFactory();
                T result = await db.FirstAsync<T>(query, transaction);
                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex, query, errorMessage);
                throw;
            }
        }

        protected async Task<IEnumerable<T>> ListAsync<T>(Query query, IDbTransaction transaction, string errorMessage)
        {
            try
            {
                QueryFactory db = CreateFactory();
                IEnumerable<T> result = await db.GetAsync<T>(query, transaction);
                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex, query, errorMessage);
                throw;
            }
        }

        private void HandleException(Exception ex, Query query, string message)
        {
            SqlResult compiled = CreateCompiler().Compile(query);

            AddExceptionData(ex, compiled);
            _log.LogTrace(ex, message);
            RemoveSensitiveExceptionData(ex);
        }

        private static void AddExceptionData(Exception ex, SqlResult sql)
        {
            ex.Data.Add("SQL", sql.Sql);
            if (sql.NamedBindings.Count > 0) { ex.Data.Add("Keys", sql.NamedBindings); }
        }

        private static void RemoveSensitiveExceptionData(Exception ex)
        {
            if (ex.Data.Contains("SQL")) { ex.Data.Remove("SQL"); }
            if (ex.Data.Contains("Keys")) { ex.Data.Remove("Keys"); }
        }

        private IDbConnection GetConnection()
        {
            string connectionString = "[NOT SET]";

            try
            {
                connectionString = GetConnectionString();
                return MakeDbConnection(connectionString);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Connection string", connectionString);
                _log.LogTrace(ex, "Could not create a connection to the database");
                throw;
            }
        }

        private string GetConnectionString()
        {
            string connectionName = _config.GetSection("ConnectionString")["Active"];
            if (string.IsNullOrWhiteSpace(connectionName)) { _log.LogWarning("No active connection string is set"); }


            string connectionString = _config.GetConnectionString(connectionName);
            if (string.IsNullOrWhiteSpace(connectionString)) { _log.LogWarning("The connection string is empty"); }

            return connectionString;
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
            return new QueryFactory(GetConnection(), CreateCompiler())
            {
                Logger = LogQuery
            };
        }

        private void LogQuery(SqlResult sql)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Executing \"");
            sb.Append(sql.RawSql);
            sb.Append("\" ");

            if (sql.Bindings.Count > 0)
            {
                sb.Append("with value");
                if (sql.Bindings.Count > 1) { sb.Append("s"); }
                sb.Append(" [");
                sb.Append(string.Join(", ", sql.Bindings.Select(x => x.ToString())));
                sb.Append("] ");
            }

            sb.Append("in ");
            sb.Append(GetConnection().Database);

            _log.LogTrace(sb.ToString());
        }
    }
}