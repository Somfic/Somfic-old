using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Somfic.Database
{
    /// <summary>
    /// Interface for accessing data from a database
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Creates a record in the database
        /// </summary>
        /// <typeparam name="T">The model</typeparam>
        /// <param name="sql">The sql</param>
        /// <param name="data">The model data</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The amount of affected rows</returns>
        public Task<int> Create<T>(string sql, T data, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Reads a record from the database
        /// </summary>
        /// <typeparam name="T">The model</typeparam>
        /// <param name="sql">The sql</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The rows</returns>
        public Task<IEnumerable<T>> Read<T>(string sql, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Updates a record in the database
        /// </summary>
        /// <typeparam name="T">The model</typeparam>
        /// <param name="sql">The sql</param>
        /// <param name="data">The model data</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The amount of affected rows</returns>
        public Task<int> Update<T>(string sql, T data, IDbTransaction transaction = null) where T : class;

        /// <summary>
        /// Deletes a record from the database
        /// </summary>
        /// <typeparam name="T">The model</typeparam>
        /// <param name="sql">The sql</param>
        /// <param name="data">The model data</param>
        /// <param name="transaction">The database transaction</param>
        /// <returns>The amount of affected rows</returns>
        public Task<int> Delete<T>(string sql, T data, IDbTransaction transaction = null) where T : class;
    }
}
