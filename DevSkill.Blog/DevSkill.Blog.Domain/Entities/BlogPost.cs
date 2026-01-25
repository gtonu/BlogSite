using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Entities
{
    public class BlogPost : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string? Url { get; private set; }
        public List<BlogPostCategory>? Categories { get; set; }
        public List<BlogPostTag>? Tags { get; set; }
        
        

        public void GenerateUrl()
        {
            Url = Title.Replace(' ', '-') + '-' + Id.ToString("N").Substring(0, 10);
        }

    }
}
