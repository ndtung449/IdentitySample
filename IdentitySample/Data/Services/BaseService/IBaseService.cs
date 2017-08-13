namespace IdentitySample.Data.Services
{
    using IdentitySample.Data.Entities;
    using IdentitySample.Models;
    using Microsoft.AspNetCore.Authorization;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IBaseService<TEntity, TViewModel>
        where TEntity : IEntity
        where TViewModel : IViewModel
    {
        Task<PaginatedList<TEntity, TViewModel>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int page = 1);
        Task<TViewModel> GetByIdAsync(object id);
        Task<TViewModel> AddAsync(TViewModel model);
        Task<TViewModel> UpdateAsync(TViewModel model);
        Task DeleteAsync(object id);
        Task<bool> AuthorizeAsync(ClaimsPrincipal user, string resourceId, IAuthorizationRequirement requirement);
    }
}
