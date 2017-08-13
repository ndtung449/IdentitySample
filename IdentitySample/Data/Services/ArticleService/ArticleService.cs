namespace IdentitySample.Data.Services
{
    using IdentitySample.Data.Entities;
    using IdentitySample.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using IdentitySample.Data.UnitOfWork;
    using IdentitySample.Data.Repositories;
    using Microsoft.AspNetCore.Authorization;

    public class ArticleService : BaseService<Article, ArticleViewModel>, IArticleService
    {
        public ArticleService(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
            : base(unitOfWork, authorizationService)
        {
        }

        protected override IGenericRepository<Article> Repository => UnitOfWork.ArticleRepository;
    }
}
