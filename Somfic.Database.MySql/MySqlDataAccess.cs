
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

        /// <inheritdoc />
        public Task<T> GetAsync<T>(string sql, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.QueryFirstAsync<T>(sql, transaction: transaction);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SQL", sql);
                _log.LogError(ex, "Could not create record in database");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> GetListAsync<T>(string sql, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.QueryAsync<T>(sql, transaction: transaction);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SQL", sql);
                _log.LogError(ex, "Could not read record from database");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<int> InsertAsync<T>(string sql, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.ExecuteAsync(sql, transaction: transaction);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SQL", sql);
                _log.LogError(ex, "Could not insert record in database");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<int> UpdateAsync<T>(string sql, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.ExecuteAsync(sql, transaction: transaction);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SQL", sql);
                _log.LogError(ex, "Could not update record in database");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<int> DeleteAsync<T>(string sql, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.ExecuteAsync(sql, transaction: transaction);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SQL", sql);
                _log.LogError(ex, "Could not delete record from database");
                throw;
            }
        }
    }
}
