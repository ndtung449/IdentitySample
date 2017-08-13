namespace IdentitySample.Data.Services
{
    using AutoMapper;
    using IdentitySample.Data.Repositories;
    using IdentitySample.Data.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Claims;
    using IdentitySample.Data.Entities;
    using IdentitySample.Models;

    public abstract class BaseService<TEntity, TViewModel> : IBaseService<TEntity, TViewModel>
        where TEntity : class, IEntity
        where TViewModel : class, IViewModel
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IAuthorizationService _authorizationService;

        protected abstract IGenericRepository<TEntity> Repository { get; }

        public BaseService(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            _authorizationService = authorizationService;
        }

        public async Task<TViewModel> AddAsync(TViewModel model)
        {
            TEntity entity = Mapper.Map<TEntity>(model);
            Repository.Add(entity);
            await UnitOfWork.SaveAsync();
            return Mapper.Map<TViewModel>(entity);
        }

        public async Task DeleteAsync(object id)
        {
            await Repository.DeleteAsync(id);
            await UnitOfWork.SaveAsync();
        }

        public async Task<PaginatedList<TEntity, TViewModel>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int page = 1)
        {
            var query = Repository.Get(filter, orderBy, includeProperties);
            const int PageSize = 10;
            return await PaginatedList<TEntity, TViewModel>.CreateAsync(query.AsNoTracking(), page, PageSize);
        }

        public async Task<TViewModel> GetByIdAsync(object id)
        {
            //var entity = await Repository.GetByIdAsync(id);
            var entity = await Repository.GetByIdAsNoTrackingAsync(id);
            return Mapper.Map<TViewModel>(entity);
        }

        public async Task<TViewModel> UpdateAsync(TViewModel model)
        {
            var entity = Mapper.Map<TEntity>(model);
            Repository.Update(entity);
            await UnitOfWork.SaveAsync();
            return Mapper.Map<TViewModel>(entity);
        }

        public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, string resourceId, IAuthorizationRequirement requirement)
        {
            var entity = await Repository.GetByIdAsNoTrackingAsync(resourceId);
            if (entity == null)
            {
                return false;
            }

            return await _authorizationService.AuthorizeAsync(user, entity, requirement);
        }
    }
}
