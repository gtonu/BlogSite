using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Entities
{
    public class ContactUs : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; } = null!;
        public string SenderEmail { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Reply { get; set; }
        public bool MarkAsRead { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? RepliedDate { get; set; }
    }
}
