using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class SuspendBlogPostCommand : ICommand<Guid>
    {
        public Guid PostId { get; set; }
    }
}
