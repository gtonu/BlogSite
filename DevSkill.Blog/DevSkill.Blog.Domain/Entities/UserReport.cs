using DevSkill.Blog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Entities
{
    public class UserReport : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string PostUrl { get; set; }
        public PostStatus PostStatus { get; set; }
        public PostStatus BlogStatus { get; set; }
        public DateTime ReportedAt { get; set; }
        public string Report { get; set; } = null!;
    }
}
