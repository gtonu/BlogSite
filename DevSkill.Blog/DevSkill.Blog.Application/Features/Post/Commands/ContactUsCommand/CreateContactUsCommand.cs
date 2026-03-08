using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand
{
    public class CreateContactUsCommand : ICommand<ContactUs>
    {
        public string SenderName { get; set; } = null!;
        public string SenderEmail { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
