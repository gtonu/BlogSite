using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Entities
{
    public class TermsAndConditions : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string Version { get; set; } = null!;
        public DateTime DateTime { get; set; }
    }
}
