namespace IdentitySample.Data.UnitOfWork
{
    using System;
    using System.Threading.Tasks;
    using IdentitySample.Data.Entities;
    using IdentitySample.Data.Repositories;

    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            InitRepositories();
        }

        public IGenericRepository<Article> ArticleRepository { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        private void InitRepositories()
        {
            ArticleRepository = new GenericRepository<Article>(_context);
        }
    }
}
