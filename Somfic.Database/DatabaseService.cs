using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Somfic.Database
{
    /// <summary>
    /// An abstract database connection service
    /// </summary>
    public abstract class DatabaseService
    {
        /// <summary>
        /// The connection string used for this instance
        /// </summary>
        protected abstract string ConnectionString { get; }

        /// <summary>
        /// The connection to the database
        /// </summary>
        protected IDbConnection Connection { get; set; }

        /// <summary>
        /// Creates a new transaction
        /// </summary>
        /// <returns>The new transaction</returns>
        public IDbTransaction NewTransaction()
        {
            return Connection.BeginTransaction();
        }

        /// <summary>
        /// Gets a record from a table
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="id">The id of the record</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>The record</returns>
        public abstract Task<T> Get<T>(dynamic id, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Gets multiple records from a table
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="filter">The predicate of the filter</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>A list of records</returns>
        public abstract Task<IList<T>> Get<T>(Func<T, bool> filter, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Gets multiple records from a table with pagination
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="resultsPerPage">The amount of results per page</param>
        /// <param name="page">The page index</param>
        /// <param name="filter">The predicate of the filter</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>A list of records</returns>
        public abstract Task<IList<T>> Get<T>(Func<T, bool> filter, int resultsPerPage, int page = 1, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Counts the amount of records against a predicate
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="filter">The predicate of the filter</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>The amount of records</returns>
        public abstract Task<int> Count<T>(Func<T, bool> filter, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Inserts a record
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entity">The record to be inserted</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>The inserted Id</returns>
        public abstract Task<dynamic> Insert<T>(T entity, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Inserts multiple records
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entities">The records to be inserted</param>
        /// <param name="transaction">The transaction this task is part of</param>
        public abstract Task Insert<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entity">The record to be updated</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>Whether the update was a success</returns>
        public abstract Task<bool> Update<T>(T entity, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <typeparam name="T">The type of record</typeparam>
        /// <param name="entity">The record to be deleted</param>
        /// <param name="transaction">The transaction this task is part of</param>
        /// <returns>Whether the deletion was a success</returns>
        public abstract Task<bool> Delete<T>(T entity, IDbTransaction transaction = null) where T : class;
    }
}
