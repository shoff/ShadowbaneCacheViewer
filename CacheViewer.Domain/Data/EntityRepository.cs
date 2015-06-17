using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CacheViewer.Domain.Data
{
    /// <summary> 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TK">The type of the k.</typeparam>
    public class EntityRepository<TEntity, TK> : IEntityRepository<TEntity, TK>, IDisposable
        where TEntity : class
    {
        // ReSharper disable once InconsistentNaming
        protected IQueryableDataContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRepository{TEntity, TK}" /> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public EntityRepository(IQueryableDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public IQueryableDataContext Context
        {
            get { return this.dataContext; }
            set { this.dataContext = value; }
        }

        /// <summary>
        /// Gets the TEnity asynchronously.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="doNotTrack">if set to <c>true</c> [do not track].</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="configureAwait">if set to <c>true</c> [configure await].</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool doNotTrack = false, string includeProperties = "", bool configureAwait = true)
        {
            return await Task.Factory.StartNew(() =>
                this.Get(filter, orderBy, doNotTrack, includeProperties)).ConfigureAwait(configureAwait);
        }

        /// <summary>
        /// Generic method to get a collection of Entities
        /// </summary>
        /// <param name="filter">Filter expression for the return Entities</param>
        /// <param name="orderBy">Represents the order of the return Entities</param>
        /// <param name="doNotTrack">if set to <c>true</c> [do not track].</param>
        /// <param name="includeProperties">Include Properties for the navigation properties</param>
        /// <returns>
        /// A Enumerable of Entities
        /// </returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool doNotTrack = false, string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dataContext.SetEntity<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (doNotTrack)
            {
                query = query.AsNoTracking();
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                query = includeProperties.Split(new[]
                {
                    ','
                }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="configureAwait">if set to <c>true</c> [configure await].</param>
        /// <returns></returns>
        public async Task<TEntity> GetByIdAsync(TK id, bool configureAwait = true)
        {
            return await Task.Factory.StartNew(() =>
                this.GetById(id)).ConfigureAwait(configureAwait);
        }

        /// <summary>
        /// Generic Method to get an Entity by Identity
        /// </summary>
        /// <param name="id">The Identity of the Entity</param>
        /// <returns>
        /// The Entity
        /// </returns>
        public virtual TEntity GetById(TK id)
        {
            CheckParameter(id);
            return this.dataContext.SetEntity<TEntity>().Find(id);
        }

        /// <summary>
        /// Generic method for add an Entity to the context
        /// </summary>
        /// <param name="entity">The Entity to Add</param>
        public virtual void Insert(TEntity entity)
        {
            this.dataContext.SetEntity<TEntity>().Add(entity);
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="configureAwait">if set to <c>true</c> [configure await].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task InsertAsync(TEntity entity, bool configureAwait = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generic method for deleting a method in the context by identity
        /// </summary>
        /// <param name="id">The Identity of the Entity</param>
        public virtual void Delete(TK id)
        {
            CheckParameter(id);
            TEntity entityToDelete = this.dataContext.SetEntity<TEntity>().Find(id);
            this.Delete(entityToDelete);
        }

        private static void CheckParameter(TK id)
        {
            if (!typeof(TK).IsValueType)
            {
                if (ReferenceEquals(null, id))
                {
                    throw new ArgumentNullException("id");
                }
            }
        }

        /// <summary>
        /// Generic method for deleting a method in the context pasing the Entity
        /// </summary>
        /// <param name="entityToDelete">Entity to Delete</param>
        /// <exception cref="ArgumentNullException">The value of 'entityToDelete' cannot be null. </exception>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null)
            {
                throw new ArgumentNullException("entityToDelete");
            }
            this.dataContext.Attach(entityToDelete);
            this.dataContext.SetEntity<TEntity>().Remove(entityToDelete);
        }

        /// <summary>
        /// Deletes the TEntity with given id asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="configureAwait">if set to <c>true</c> [configure await].</param>
        /// <returns></returns>
        public async Task DeleteAsync(TK id, bool configureAwait = true)
        {
            await Task.Factory.StartNew(() => this.Delete(id)).ConfigureAwait(configureAwait);
        }

        /// <summary>
        /// Generic method for updating an Entity in the context
        /// </summary>
        /// <param name="entityToUpdate">The story to Update</param>
        /// <exception cref="ArgumentNullException">The value of 'entityToUpdate' cannot be null. </exception>
        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null)
            {
                throw new ArgumentNullException("entityToUpdate");
            }
            this.dataContext.SetModified(entityToUpdate);
        }

        /// <summary>
        /// Updates the given TEntity asynchronously.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <param name="configureAwait">if set to <c>true</c> [configure await].</param>
        /// <returns></returns>
        public async Task UpdateAsync(TEntity entityToUpdate, bool configureAwait = true)
        {
            await Task.Factory.StartNew(() => this.Update(entityToUpdate)).ConfigureAwait(configureAwait);
        }

        /// <summary>
        /// Generic implementation for get Paged Entities
        /// </summary>
        /// <typeparam name="TKey">Key for order Expression</typeparam>
        /// <param name="pageIndex">Index of the Page</param>
        /// <param name="pageCount">Number of Entities to get</param>
        /// <param name="orderByExpression">Order expression</param>
        /// <param name="orderby">if set to <c>true</c> [orderby].</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// Enumerable of Entities matching the conditions
        /// </returns>
        /// <exception cref="ArgumentNullException">orderByExpression</exception>
        /// <exception cref="DbEntityValidationException">Condition. </exception>
        public IEnumerable<TEntity> GetPagedElements<TKey>(int pageIndex, int pageCount,
            Expression<Func<TEntity, TKey>> orderByExpression, bool orderby = true, string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dataContext.SetEntity<TEntity>();

            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var s in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(s);
                }
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (orderByExpression == null)
            {
                throw new ArgumentNullException("orderByExpression");
            }
            try
            {
                return (orderby)
                      ? query.OrderBy(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList()
                      : query.OrderByDescending(orderByExpression)
                            .Skip((pageIndex - 1) * pageCount)
                            .Take(pageCount)
                            .ToList();
            }
            catch (DbEntityValidationException dbe)
            {
                // if you override ToString on your story, if there is a validation error, 
                // this will log the whatever you put in ToString so you can identify 
                // the faulting story.

                if (!ReferenceEquals(null, dbe.EntityValidationErrors))
                {
                }
                throw;
            }
            catch (InvalidCastException)
            {
                // we never return null from our repository, just
                // an empty collection.
                return new List<TEntity>();
            }
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="sqlQuery">The Query to be executed</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>
        /// List of Entity
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of 'sqlQuery' cannot be null. </exception>
        public IEnumerable<TEntity> GetFromDatabaseWithQuery(string sqlQuery, params object[] parameters)
        {
            if (string.IsNullOrEmpty(sqlQuery))
            {
                throw new ArgumentNullException("sqlQuery");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            return this.dataContext.ExecuteQuery<TEntity>(sqlQuery, parameters);
        }

        /// <summary>
        /// Execute a command in database
        /// </summary>
        /// <param name="sqlCommand">The sql query</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>integer representing the sql code</returns>
        /// <exception cref="ArgumentNullException">The value of 'parameters' cannot be null. </exception>
        public int ExecuteInDatabaseByQuery(string sqlCommand, params object[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (string.IsNullOrEmpty(sqlCommand))
            {
                throw new ArgumentNullException("sqlCommand");
            }
            return this.dataContext.ExecuteCommand(sqlCommand, parameters);
        }

        /// <summary>
        /// Get count of Entities
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException">The number of elements in <paramref>
        ///     <name>source</name>
        ///   </paramref>
        ///   is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public int GetCount()
        {
            // return this.dataContext.Set<TEntity>().Count();
            var set = this.dataContext.SetEntity<TEntity>();
            // ReSharper disable once HeapView.ObjectAllocation
            return set.AsEnumerable().Count();
        }

        /// <summary>
        /// Gets or sets the default include properties. This needs to be a comma-delimited list of 
        /// properties to include in a query by default.
        /// </summary>
        /// <value>
        /// The default include properties.
        /// </value>
        public string DefaultIncludeProperties { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.Context.Dispose();
        }
    }
}