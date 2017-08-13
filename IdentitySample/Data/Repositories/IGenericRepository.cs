namespace IdentitySample.Data.Repositories
{
    using IdentitySample.Data.Entities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IGenericRepository<TEntity>
        where TEntity : IEntity
    {
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> GetByIdAsNoTrackingAsync(object id);
        void Add(TEntity entity);
        Task DeleteAsync(object id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
