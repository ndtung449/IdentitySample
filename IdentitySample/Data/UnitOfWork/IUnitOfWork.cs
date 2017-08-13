namespace IdentitySample.Data.UnitOfWork
{
    using IdentitySample.Data.Entities;
    using IdentitySample.Data.Repositories;
    using System;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Article> ArticleRepository { get; }
        Task SaveAsync();
    }
}
