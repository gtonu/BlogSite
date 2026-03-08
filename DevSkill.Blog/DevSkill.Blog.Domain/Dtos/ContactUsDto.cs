using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Dtos
{
    public class ContactUsDto
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public bool MarkAsRead { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? RepliedDate { get; set; }
    }
}
