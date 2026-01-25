using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Application.Features.Post.Commands
{
    public class CreateCategoryCommand : ICommand<Category>
    {
        public string CategoryName { get; set; } = null!;
    }
}
