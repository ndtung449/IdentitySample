namespace IdentitySample.Data.Services
{
    using IdentitySample.Data.Entities;
    using IdentitySample.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IArticleService : IBaseService<Article, ArticleViewModel>
    {
    }
}
