using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SqlKata;

namespace Somfic.Database
{
    /// <summary>
    /// Interface for accessing data from a database
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Reads a record from the database
        /// </summary>
        /// <typeparam name="T">The model</typeparam>
        /// <param name="query">The SQL query</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The rows</returns>
        public Task<T> GetAsync<T>(Query query, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Updates a record in the database
        /// </summary>
        /// <typeparam name="T">The model</typeparam>
        /// <param name="query">The SQL query</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The amount of affected rows</returns>
        public Task<IEnumerable<T>> GetListAsync<T>(Query query, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Creates a record in the database
        /// </summary>
        /// <param name="query">The SQL query</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The amount of affected rows</returns>
        public Task<int> InsertAsync(Query query, IDbTransaction transaction = null);

        /// <summary>
        /// Updates a record in the database
        /// </summary>
        /// <param name="query">The SQL query</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The amount of affected rows</returns>
        public Task<int> UpdateAsync(Query query, IDbTransaction transaction = null);

        /// <summary>
        /// Deletes a record from the database
        /// </summary>
        /// <param name="query">The SQL query</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The amount of affected rows</returns>
        public Task<int> DeleteAsync(Query query, IDbTransaction transaction = null);

        /// <summary>
        /// Creates a <seealso cref="IDbConnection"/>
        /// </summary>
        /// <returns>The connection</returns>
        public IDbConnection CreateConnection();
    }
}
