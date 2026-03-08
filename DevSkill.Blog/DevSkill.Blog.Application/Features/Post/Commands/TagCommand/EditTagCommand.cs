using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevSkill.Blog.Application.Features.Post.Commands.TagCommand
{
    public class EditTagCommand : ICommand<Tag>
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}
