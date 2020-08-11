
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
    public class MySqlService : DatabaseService
    {
        private readonly ILogger<DatabaseService> _log;

        /// <summary>
        /// Creates a new MySql database connection service
        /// </summary>
        /// <param name="log">Logging information</param>
        /// <param name="configuration">Configuration information</param>
        /// <param name="connectionString">The connection string</param>
        public MySqlService(ILogger<DatabaseService> log, IConfiguration configuration)
        {
            _log = log;
            DapperAsyncExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();
            ConnectionString = configuration["ConnectionString"];
        }

        /// <summary>
        /// The connection string used for this instance
        /// </summary>
        protected override string ConnectionString { get; }

        /// <summary>
        /// Gets a record from a table
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="id">The id of the record</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>The record</returns>
        public override async Task<T> Get<T>(dynamic id, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Connection.Open();
                T person = await Connection.GetAsync<T>((object)id, transaction);
                Connection.Close();

                return person;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not get record from '{table}' with Id of '{id}'", typeof(T).Name, (object)id);
                throw;
            }
        }

        /// <summary>
        /// Gets multiple records from a table
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="filter">The predicate of the filter</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>A list of records</returns>
        public override async Task<IList<T>> Get<T>(Func<T, bool> filter, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Connection.Open();
                IEnumerable<T> people = await Connection.GetListAsync<T>(filter, null, transaction);
                Connection.Close();

                return people.ToList();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not get records from '{table}' with filter.", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Gets multiple records from a table with pagination
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="resultsPerPage">The amount of results per page</param>
        /// <param name="page">The page index</param>
        /// <param name="filter">The predicate of the filter</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>A list of records</returns>
        public override async Task<IList<T>> Get<T>(Func<T, bool> filter, int resultsPerPage, int page = 1, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Connection.Open();
                IEnumerable<T> person = await Connection.GetPageAsync<T>(filter, null, page, resultsPerPage, transaction);
                Connection.Close();

                return person.ToList();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not get records from '{table}' with filter on page {page} with {maxResult} results per page.", typeof(T).Name, page, resultsPerPage);
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
        public override async Task<int> Count<T>(Func<T, bool> filter, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Connection.Open();
                int amount = await Connection.CountAsync<T>(filter, transaction);
                Connection.Close();


                return amount;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not count records from '{table}' with filter.", typeof(T).Name);
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
        public override async Task<dynamic> Insert<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Connection.Open();
                dynamic id = await Connection.InsertAsync(entity, transaction);
                Connection.Close();

                return id;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not insert record in '{table}' ({record}).", typeof(T).Name, entity);
                throw;
            }
        }

        /// <summary>
        /// Inserts multiple records
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entities">The records to be inserted</param>
        /// <param name="transaction">The transaction this task is part of</param>
        public override async Task Insert<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Connection.Open();
                await Connection.InsertAsync(entities, transaction);
                Connection.Close();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not insert records in '{table}' ({record}).", typeof(T).Name, entities);
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
        public override async Task<bool> Update<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Connection.Open();
                bool success = await Connection.UpdateAsync(entity, transaction);
                Connection.Close();

                return success;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not update record in '{table}' ({entity})", typeof(T).Name, entity);
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
        public override async Task<bool> Delete<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Connection.Open();
                bool success = await Connection.DeleteAsync(entity, transaction);
                Connection.Close();

                return success;
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not delete record from '{table}' ({entity})", typeof(T).Name, entity);
                throw;
            }
        }
    }

}
