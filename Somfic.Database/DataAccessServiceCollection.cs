using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Somfic.Database
{
    public static class DataAccessServiceCollection
    {
        /// <summary>
        /// Adds an <seealso cref="IDataAccess"/> class to the <seealso cref="IServiceCollection"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="IDataAccess"/> implementation</typeparam>
        /// <returns></returns>
        public static IServiceCollection AddDataAccess<T>(this IServiceCollection services) where T : class, IDataAccess
        {
            services.AddSingleton<IDataAccess, T>();

            return services;
        }
    }
}
