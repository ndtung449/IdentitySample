namespace IdentitySample.Data.Entities
{
    public class Article : IEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public bool IsActivated { get; set; }
    }
}
