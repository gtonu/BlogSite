using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands
{
    public class CreateBlogPostCommand : ICommand<Guid>
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
