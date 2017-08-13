namespace IdentitySample.DataMapper
{
    using AutoMapper;

    public static class AutoMapperConfiguration
    {
        public static void Config()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new EntityModelMapper());
            });
        }
    }
}
