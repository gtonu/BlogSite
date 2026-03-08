using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Dtos
{
    public class BlogPostsWithCategoriesDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? ThumbnailName { get; set;}
        public string? Url { get; set; }
        public DateTime PublishedAt { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
    }
}
