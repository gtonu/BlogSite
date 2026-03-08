using DevSkill.Blog.Domain.Enums;
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
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ThumbnailName { get; set; }
        public string? Url { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public PostStatus Status { get; set; }
        public List<BlogPostCategory>? Categories { get; set; }
        public List<BlogPostTag>? Tags { get; set; }
        public List<Comment>? Comments { get; set; }
        
        

        public void GenerateUrlFromTitle()
        {
            Url = Title?.Replace(' ', '-') + '-' + Id.ToString("N").Substring(0, 10);
        }
        public void GenerateUrlFromBody()
        {
            Url = Body?.Replace(' ', '-').Substring(0, 50) + '-' + Id.ToString("N").Substring(0, 10);
        }

    }
}
