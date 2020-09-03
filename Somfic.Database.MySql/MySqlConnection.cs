
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Somfic.Database.MySql
{
    /// <summary>
    /// A MySql database connection service
    /// </summary>
    public class MySqlConnection : IDatabaseConnection
    {
        private readonly ILogger<MySqlConnection> _log;

        /// <summary>
        /// Creates a new MySql database connection service
        /// </summary>
        /// <param name="log">Logging information</param>
        /// <param name="configuration">Configuration information</param>
        public MySqlConnection(ILogger<MySqlConnection> log, IConfiguration configuration)
        {
            try
            {
                _log = log;
                DapperAsyncExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();
                ConnectionString = configuration["ConnectionString"];
                _connection = new global::MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            }
            catch (Exception ex)
            {
                log?.LogWarning(ex, "Could not start MySql connection service");
                throw;
            }
        }

        private readonly IDbConnection _connection;

        /// <inheritdoc />
        public string ConnectionString { get; }

        /// <inheritdoc />
        public async Task<T> Get<T>(object id, IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                T person = await _connection.GetAsync<T>(id, transaction);
                _connection.Close();

                return person;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not get record from {table} with Id of {id}", typeof(T).Name, (object)id);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IList<T>> Get<T>(Func<T, bool> filter, IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                IEnumerable<T> people = await _connection.GetListAsync<T>(filter, null, transaction);
                _connection.Close();

                return people.ToList();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not get records from {table} with filter.", typeof(T).Name);
                throw;
            }
        }


        /// <inheritdoc />
        public async Task<IList<T>> Get<T>(Func<T, bool> filter, int resultsPerPage, int page = 1, IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                IEnumerable<T> person = await _connection.GetPageAsync<T>(filter, null, page, resultsPerPage, transaction);
                _connection.Close();

                return person.ToList();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not get records from {table} with filter on page {page} with {maxResult} results per page.", typeof(T).Name, page, resultsPerPage);
                throw;
            }
        }


        /// <inheritdoc />
        public async Task<IEnumerable<T>> Get<T>(IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                var all = await _connection.GetListAsync<T>(transaction: transaction);
                _connection.Close();

                return all;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not get all records from {table}.", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Counts the amount of records against a predicate
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="filter">The predicate of the filter</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>The amount of records</returns>
        public async Task<int> Count<T>(Func<T, bool> filter, IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                int amount = await _connection.CountAsync<T>(filter, transaction);
                _connection.Close();

                return amount;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not count records from {table} with filter.", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Inserts a record
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entity">The record to be inserted</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>The inserted Id</returns>
        public async Task<object> Insert<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                object id = await _connection.InsertAsync(entity, transaction);
                _connection.Close();

                return id;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not insert record in {table} ({record}).", typeof(T).Name, entity);
                throw;
            }
        }

        /// <summary>
        /// Inserts multiple records
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entities">The records to be inserted</param>
        /// <param name="transaction">The transaction this task is part of</param>
        public async Task Insert<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                await _connection.InsertAsync(entities, transaction);
                _connection.Close();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not insert records in {table} ({record}).", typeof(T).Name, entities);
                throw;
            }
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entity">The record to be updated</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>Whether the update was a success</returns>
        public async Task<bool> Update<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                bool success = await _connection.UpdateAsync(entity, transaction);
                _connection.Close();

                return success;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not update record in {table} ({entity})", typeof(T).Name, entity);
                throw;
            }
        }

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entity">The record to be deleted</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>Whether the deletion was a success</returns>
        public async Task<bool> Delete<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
                _connection.Open();
                bool success = await _connection.DeleteAsync(entity, transaction);
                _connection.Close();

                return success;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not delete record from {table} ({entity})", typeof(T).Name, entity);
                throw;
            }
        }
    }

}
