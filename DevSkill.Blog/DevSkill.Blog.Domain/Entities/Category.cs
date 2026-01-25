using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Entities
{
    public class Category : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public List<BlogPostCategory>? BlogPosts { get; set; }

    }
}
