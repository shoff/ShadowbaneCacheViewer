namespace CacheViewer.Domain.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Data.Entities;
    using CacheViewer.Domain.Models;
    using NLog;
    using Utility;

    public class DataContext : DbContext, IDataContext
    {
        /// <summary>
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // MultipleActiveResultSets=true
        /// <summary>
        /// </summary>
        public DataContext()
            : this("DefaultConnection")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataContext" /> class.
        /// </summary>
        /// <param name="connectionString">
        ///     The connection string.
        /// </param>
        public DataContext(string connectionString)
            : base(connectionString)
        {
            this.ReThrowExceptions = true;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to [re throw exceptions].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [re throw exceptions]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>The default value is <c>true</c> this is here to allow for test coverage.</remarks>
        internal bool ReThrowExceptions { get; set; }

        /// <summary>
        ///     Returns a IDbSet instance for access to entities of the given type in the context,
        ///     the ObjectStateManager, and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     The type of the entity.
        /// </typeparam>
        /// <returns>
        /// </returns>
        /// <remarks>
        ///     This method is virtual
        /// </remarks>
        public DbSet<TEntity> SetEntity<TEntity>()
            where TEntity : class
        {
            return this.Set<TEntity>();
        }

        /// <summary>
        ///     Attaches the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     The type of the entity.
        /// </typeparam>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <remarks>
        ///     This method is virtual
        /// </remarks>
        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (this.Entry(entity).State == EntityState.Detached)
            {
                this.Set<TEntity>().Attach(entity);
            }
        }

        public void Commit()
        {
            this.SaveChanges();
        }

        /// <summary>
        ///     Sets the modified.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     The type of the entity.
        /// </typeparam>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <remarks>
        ///     This method is virtual
        /// </remarks>
        public void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        ///     Execute specific query with underliying persistence store
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Entity type to map query results
        /// </typeparam>
        /// <param name="sqlQuery">
        ///     Dialect Query
        ///     <example>
        ///         SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer &gt; {0}
        ///     </example>
        /// </param>
        /// <param name="parameters">
        ///     A vector of parameters values
        /// </param>
        /// <returns>
        ///     Enumerable results
        /// </returns>
        /// <remarks>
        ///     This method is virtual
        /// </remarks>
        /// <code>
        /// context.Database.SqlQuery&lt;EntityType&gt;( 
        ///     "EXEC ProcName @param1, @param2", 
        ///     new SqlParameter("param1", param1), 
        ///     new SqlParameter("param2", param2));
        /// </code>
        /// <code>
        /// context.Database.SqlQuery&lt;MyEntityType&gt;("mySpName @param1 = {0}", param1)
        /// </code>
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return this.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        /// <summary>
        ///     Execute arbitrary command into underlying persistence store
        /// </summary>
        /// <param name="sqlCommand">
        ///     Command to execute
        ///     <example>
        ///         SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer &gt; {0}
        ///     </example>
        /// </param>
        /// <param name="parameters">
        ///     A vector of parameters values
        /// </param>
        /// <returns>
        ///     The number of affected records
        /// </returns>
        /// <remarks>
        ///     This method is virtual
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     The value of 'sqlCommand' cannot be null.
        /// </exception>
        /// <exception cref="SqlException">
        ///     If ReThrowExceptions.
        /// </exception>
        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            if (string.IsNullOrEmpty(sqlCommand))
            {
                throw new ArgumentNullException("sqlCommand");
            }

            try
            {
                return this.Database.ExecuteSqlCommand(sqlCommand, parameters);
            }
            catch (SqlException sql)
            {
                logger.Error(sql.Message, sql);
                if (this.ReThrowExceptions)
                {
                    throw;
                }

                return -1;
            }
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        ///     The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <throws>System.InvalidOperationException: Thrown if the context has been disposed.</throws>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.
        ///     Use 'await' to ensure that any asynchronous operations have completed before
        ///     calling another method on this context.
        /// </remarks>
        /// <exception cref="DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually indicates an optimistic
        ///     concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        public async Task<int> CommitAsync()
        {
            return await this.SaveChangesAsync();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [disable proxy creation]. The default
        ///     value is set to <c>true</c>
        /// </summary>
        /// <value>
        ///     <c>true</c> if [disable proxy creation]; otherwise, <c>false</c>.
        /// </value>
        public bool CreateProxies
        {
            get => this.Configuration.ProxyCreationEnabled;
            set => this.Configuration.ProxyCreationEnabled = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [validate on save].
        ///     Default value is
        ///     <c>true</c>
        /// </summary>
        /// <value>
        ///     <c>true</c> if [validate on save]; otherwise, <c>false</c>.
        /// </value>
        public bool ValidateOnSave
        {
            get => this.Configuration.ValidateOnSaveEnabled;
            set => this.Configuration.ValidateOnSaveEnabled = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the
        ///     DbChangeTracker.DetectChanges() method is called automatically
        ///     by methods of System.Data.Entity.DbContext and related classes.
        ///     The default value is
        ///     <c>true</c>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [automatic detect changes]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoDetectChanges
        {
            get => this.Configuration.AutoDetectChangesEnabled;
            set => this.Configuration.AutoDetectChangesEnabled = value;
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A System.Threading.CancellationToken to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous save operation.
        ///     The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <throws>System.InvalidOperationException: Thrown if the context has been disposed.</throws>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.
        ///     Use 'await' to ensure that any asynchronous operations have completed before
        ///     calling another method on this context.
        /// </remarks>
        public virtual async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await this.SaveChangesAsync(cancellationToken);
        }


        /// <summary>
        ///     Called when [model creation].
        /// </summary>
        /// <param name="modelBuilder">
        ///     The model builder.
        /// </param>
        /// <remarks>
        ///     This method is virtual
        /// </remarks>
        protected void OnModelCreation(DbModelBuilder modelBuilder)
        {
        }

        /// <summary>
        ///     This method is called when the model for a derived context has been initialized, but
        ///     before the model has been locked down and used to initialize the context.  The default
        ///     implementation of this method does nothing, but it can be overridden in a derived class
        ///     such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">
        ///     The builder that defines the model for the context being created.
        /// </param>
        /// <remarks>
        ///     Typically, this method is called only once when the first instance of a derived context
        ///     is created.  The model for that context is then cached and is for all further instances of
        ///     the context in the app domain.  This caching can be disabled by setting the ModelCaching
        ///     property on the given ModelBuidler, but note that this can seriously degrade performance.
        ///     More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        ///     classes directly.
        ///     This method is virtual
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.LazyLoadingEnabled = true;
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            this.OnModelCreation(modelBuilder);
        }

        public DbSet<CacheIndexEntity> CacheIndexes { get; set; }
        public DbSet<CacheObjectEntity> CacheObjectEntities { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MeshEntity> MeshEntities { get; set; }
        public DbSet<MobileEntity> MobileEntities { get; set; }
        public DbSet<MotionEntity> MotionEntities{ get; set; }
        public DbSet<RenderAndOffset> RenderAndOffsets { get; set; }
        public DbSet<RenderEntity> RenderEntities { get; set; }
        public DbSet<SkeletonEntity> SkeletonEntities { get; set; }
        public DbSet<TextureEntity> Textures { get; set; }
        public DbSet<RenderTexture> RenderTextures { get; set; }

    }
}