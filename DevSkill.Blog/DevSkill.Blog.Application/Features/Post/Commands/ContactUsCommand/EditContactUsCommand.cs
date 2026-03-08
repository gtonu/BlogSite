using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand
{
    public class EditContactUsCommand : ICommand<ContactUs>
    {
        public Guid Id { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Message { get; set; }
        public string? Reply { get; set; }
        public bool MarkAsRead { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? RepliedDate { get; set; }
    }
}
