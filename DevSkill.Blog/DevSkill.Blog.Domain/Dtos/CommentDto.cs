using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Body { get; set; } = null!;
        public PostStatus CommentStatus { get; set; }
        public Guid PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public List<CommentDto>? Replies { get; set; } //self referencing technique used as reply is also a comment..
    }
}
