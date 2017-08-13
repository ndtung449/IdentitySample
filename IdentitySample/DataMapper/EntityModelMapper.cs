namespace IdentitySample.DataMapper
{
    using AutoMapper;
    using IdentitySample.Data.Entities;
    using IdentitySample.Models;

    public class EntityModelMapper : Profile
    {
        public EntityModelMapper()
        {
            #region Enity To Model

            CreateMap<Article, ArticleViewModel>();

            #endregion

            #region Model to Entity

            CreateMap<ArticleViewModel, Article>();

            #endregion
        }
    }
}
