namespace IdentitySample.Data
{
    using AutoMapper;
    using IdentitySample.Data.Entities;
    using IdentitySample.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PaginatedList<TEntity, TViewModel> : List<TViewModel>
        where TEntity : IEntity
        where TViewModel : IViewModel
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<TViewModel> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<TEntity, TViewModel>> CreateAsync(IQueryable<TEntity> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var entities = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var models = Mapper.Map<List<TViewModel>>(entities);
            return new PaginatedList<TEntity, TViewModel>(models, count, pageIndex, pageSize);
        }
    }
}
