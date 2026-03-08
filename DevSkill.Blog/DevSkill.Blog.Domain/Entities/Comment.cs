using DevSkill.Blog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Entities
{
    public class Comment 
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Body { get; set; } = null!;
        public PostStatus CommentStatus { get; set; }
        public Guid PostId { get; set; }
        public BlogPost Post { get; set; } = null!;
        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public List<Comment>? Replies { get; set; } //self referencing technique used as reply is also a comment..
    }
}
