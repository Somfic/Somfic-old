
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

        private IDbConnection CreateConnection(string connectionName = "ASM")
        {
            try
            {
                _connectionString = _config.GetConnectionString(connectionName);
                if (string.IsNullOrWhiteSpace(_connectionString)) { _log.LogWarning("The connection string is empty"); }

                return new global::MySql.Data.MySqlClient.MySqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                ex.Data.Add("Connection name", connectionName);
                ex.Data.Add("Connection string", _connectionString);
                _log.LogTrace(ex, "Could not create a connection to the database");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<int> Create<T>(string sql, T data, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.ExecuteAsync(sql, data, transaction);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SQL", sql);
                ex.Data.Add("Data", data);
                _log.LogError(ex, "Could not create record in database");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> Read<T>(string sql, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.QueryAsync<T>(sql, transaction);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SQL", sql);
                _log.LogError(ex, "Could not read record from database");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<int> Update<T>(string sql, T data, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.ExecuteAsync(sql, data, transaction);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SQL", sql);
                _log.LogError(ex, "Could not update record in database");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<int> Delete<T>(string sql, T data, IDbTransaction transaction = null) where T : class
        {
            try
            {
                using IDbConnection c = CreateConnection();
                return c.ExecuteAsync(sql, data, transaction);
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
