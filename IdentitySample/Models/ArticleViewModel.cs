namespace IdentitySample.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ArticleViewModel : IViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public bool IsActivated { get; set; }
    }
}
