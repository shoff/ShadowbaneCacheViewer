using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace CacheViewer.Domain.Data
{
    public interface IQueryableDataContext : IDisposable
    {
        /// <summary>
        /// Returns a IDbSet instance for access to entities of the given type in the context,
        /// the ObjectStateManager, and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        DbSet<TEntity> SetEntity<TEntity>() where TEntity : class;

        /// <summary>
        /// Attaches the specified item.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The item.</param>
        void Attach<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Set object as modified
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The entity item to set as modifed</param>
        void SetModified<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        void Commit();

        /// <summary>
        /// Execute specific query with underliying persistence store
        /// </summary>
        /// <typeparam name="TEntity">Entity type to map query results</typeparam>
        /// <param name="sqlQuery">
        /// Dialect Query 
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer > {0}
        /// </example>
        /// </param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// Enumerable results 
        /// </returns>
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);

        /// <summary>
        /// Execute arbitrary command into underliying persistence store
        /// </summary>
        /// <param name="sqlCommand">Command to execute
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer &gt; {0}
        /// </example></param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// The number of affected records
        /// </returns>
        int ExecuteCommand(string sqlCommand, params object[] parameters);

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. 
        /// The task result contains the number of objects written to the underlying database.</returns>
        /// <throws>System.InvalidOperationException: Thrown if the context has been disposed.</throws>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported. 
        /// Use 'await' to ensure that any asynchronous operations have completed before 
        /// calling another method on this context.
        /// </remarks>
        Task<int> CommitAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. 
        /// The task result contains the number of objects written to the underlying database.</returns>
        /// <throws>System.InvalidOperationException: Thrown if the context has been disposed.</throws>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported. 
        /// Use 'await' to ensure that any asynchronous operations have completed before 
        /// calling another method on this context.
        /// </remarks>
        Task<int> CommitAsync();

        /// <summary>
        /// Gets or sets a value indicating whether [disable proxy creation].
        /// </summary>
        /// <value>
        /// <c>true</c> if [disable proxy creation]; otherwise, <c>false</c>.
        /// </value>
        bool CreateProxies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [validate on save].
        /// Default value is <c>true</c>
        /// </summary>
        /// <value>
        ///   <c>true</c> if [validate on save]; otherwise, <c>false</c>.
        /// </value>
        bool ValidateOnSave { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the 
        /// DbChangeTracker.DetectChanges() method is called automatically 
        /// by methods of System.Data.Entity.DbContext and related classes.  
        /// The default value is <c>true</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if [automatic detect changes]; otherwise, <c>false</c>.
        /// </value>
        bool AutoDetectChanges { get; set; }
    }
}