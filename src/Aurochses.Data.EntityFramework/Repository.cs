﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Aurochses.Data.Query;

namespace Aurochses.Data.EntityFramework
{
    /// <summary>
    /// Repository for data layer
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TType">The type of the T type.</typeparam>
    /// <seealso>
    ///     <cref>Aurochses.Data.IRepository{TEntity, TType}</cref>
    /// </seealso>
    public class Repository<TEntity, TType> : IRepository<TEntity, TType>
        where TEntity : class, IEntity<TType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity, TType}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>The database context.</value>
        protected DbContext DbContext { get; }

        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <value>The database set.</value>
        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected virtual IQueryable<TEntity> Query(QueryParameters<TEntity, TType> queryParameters = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (queryParameters == null) return query;

            if (queryParameters.Filter?.Expression != null)
            {
                query = query.Where(queryParameters.Filter.Expression);
            }

            if (queryParameters.Sort?.Expression != null)
            {
                query = queryParameters.Sort.SortOrder == SortOrder.Descending
                    ? query.OrderByDescending(queryParameters.Sort.Expression)
                    : query.OrderBy(queryParameters.Sort.Expression);
            }

            if (queryParameters.Page != null && queryParameters.Page.IsValid)
            {
                query = query
                    .Skip(queryParameters.Page.Size * queryParameters.Page.Index)
                    .Take(queryParameters.Page.Size);
            }

            return query;
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected IQueryable<TModel> Query<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters = null)
        {
            return dataMapper.Map<TModel>(Query(queryParameters));
        }

        /// <summary>
        /// Gets entity of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual TEntity Get(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query(queryParameters).FirstOrDefault();
        }

        /// <summary>
        /// Gets model of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>TModel</cref></returns>
        public virtual TModel Get<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query<TModel>(dataMapper, queryParameters).FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously gets entity of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{TEntity}</cref>.</returns>
        public virtual async Task<TEntity> GetAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query(queryParameters).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Asynchronously gets model of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{TModel}</cref>.</returns>
        public virtual async Task<TModel> GetAsync<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query<TModel>(dataMapper, queryParameters).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TEntity}</cref>.</returns>
        public virtual IList<TEntity> GetList(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query(queryParameters).ToList();
        }

        /// <summary>
        /// Gets models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TModel}</cref>.</returns>
        public virtual IList<TModel> GetList<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query<TModel>(dataMapper, queryParameters).ToList();
        }

        /// <summary>
        /// Asynchronously gets entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{IList{TModel}}</cref>.</returns>
        public virtual async Task<IList<TEntity>> GetListAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query(queryParameters).ToListAsync();
        }

        /// <summary>
        /// Asynchronously gets models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{IList{TModel}}</cref>.</returns>
        public virtual async Task<IList<TModel>> GetListAsync<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query<TModel>(dataMapper, queryParameters).ToListAsync();
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters for paged result.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected virtual IQueryable<TEntity> PagedResultQuery(QueryParameters<TEntity, TType> queryParameters)
        {
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters), "Query Parameters can't be null.");

            if (queryParameters.Page == null) throw new ArgumentNullException(nameof(queryParameters.Page), "Query Parameters Page can't be null.");

            if (!queryParameters.Page.IsValid) throw new ArgumentException("Query Parameters Page is not valid.", nameof(queryParameters.Page));

            return Query(queryParameters);
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters for paged result.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected IQueryable<TModel> PagedResultQuery<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters)
        {
            return dataMapper.Map<TModel>(PagedResultQuery(queryParameters));
        }

        /// <summary>
        /// Gets paged list of entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>PagedResult{TEntity}</cref>.</returns>
        public virtual PagedResult<TEntity> GetPagedList(QueryParameters<TEntity, TType> queryParameters)
        {
            var items = PagedResultQuery(queryParameters).ToList();

            var totalCount = Count(queryParameters);

            return new PagedResult<TEntity>
            {
                PageIndex = queryParameters.Page.Index,
                PageSize = queryParameters.Page.Size,
                Items = items,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>PagedResult{TModel}</cref>.</returns>
        public virtual PagedResult<TModel> GetPagedList<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters)
        {
            var items = PagedResultQuery<TModel>(dataMapper, queryParameters).ToList();

            var totalCount = Count(queryParameters);

            return new PagedResult<TModel>
            {
                PageIndex = queryParameters.Page.Index,
                PageSize = queryParameters.Page.Size,
                Items = items,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Asynchronously gets paged list of entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{PagedResult{TEntity}}</cref>.</returns>
        public virtual async Task<PagedResult<TEntity>> GetPagedListAsync(QueryParameters<TEntity, TType> queryParameters)
        {
            var items = await PagedResultQuery(queryParameters).ToListAsync();

            var totalCount = await CountAsync(queryParameters);

            return new PagedResult<TEntity>
            {
                PageIndex = queryParameters.Page.Index,
                PageSize = queryParameters.Page.Size,
                Items = items,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Asynchronously gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{PagedResult{TModel}}</cref>.</returns>
        public virtual async Task<PagedResult<TModel>> GetPagedListAsync<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters)
        {
            var items = await PagedResultQuery<TModel>(dataMapper, queryParameters).ToListAsync();

            var totalCount = await CountAsync(queryParameters);

            return new PagedResult<TModel>
            {
                PageIndex = queryParameters.Page.Index,
                PageSize = queryParameters.Page.Size,
                Items = items,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Checks if any entity of type T satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual bool Exists(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query(queryParameters).Any();
        }

        /// <summary>
        /// Asynchronously checks if any entity of type T satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual async Task<bool> ExistsAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query(queryParameters).AnyAsync();
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters for count.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected virtual IQueryable<TEntity> CountQuery(QueryParameters<TEntity, TType> queryParameters = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (queryParameters == null) return query;

            if (queryParameters.Filter?.Expression != null)
            {
                query = query.Where(queryParameters.Filter.Expression);
            }

            return query;
        }

        /// <summary>
        /// Returns a number that represents how many entities in repository satisfy a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns>A number that represents how many entities in repository satisfy a query parameters.</returns>
        public virtual int Count(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return CountQuery(queryParameters).Count();
        }

        /// <summary>
        /// Asynchronously returns a number that represents how many entities in repository satisfy a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns>A number that represents how many entities in repository satisfy a query parameters.</returns>
        public virtual async Task<int> CountAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await CountQuery(queryParameters).CountAsync();
        }

        /// <summary>
        /// Marks entity as modified.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected void MarkAsModified(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Inserts entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Insert(TEntity entity)
        {
            return DbSet.Add(entity);
        }

        /// <summary>
        /// Asynchronously inserts entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await Task.Run(() => DbSet.Add(entity));
        }

        /// <summary>
        /// Updates entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="startTrackProperties">if set to <c>true</c> marks entity as modified.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Update(TEntity entity, bool startTrackProperties = false)
        {
            DbSet.Attach(entity);

            if (!startTrackProperties)
            {
                MarkAsModified(entity);
            }

            return entity;
        }

        /// <summary>
        /// Determines whether the specified entity is detached.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if the specified entity is detached; otherwise, <c>false</c>.</returns>
        protected bool IsDetached(TEntity entity)
        {
            return DbContext.Entry(entity).State == EntityState.Detached;
        }

        /// <summary>
        /// Marks entity as deleted.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected void MarkAsDeleted(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(TEntity entity)
        {
            if (IsDetached(entity))
            {
                DbSet.Attach(entity);
            }

            MarkAsDeleted(entity);

            DbSet.Remove(entity);
        }
    }
}