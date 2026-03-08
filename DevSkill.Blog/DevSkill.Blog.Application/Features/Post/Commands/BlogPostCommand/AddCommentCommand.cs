using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand
{
    public class AddCommentCommand : ICommand<BlogPost>
    {
        public Guid PostId { get; set; }
        public Comment PostComment { get; set; }
    }
}
