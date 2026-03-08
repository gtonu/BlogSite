using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain.Entities;

namespace DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand
{
    public class EditCategoryCommand : ICommand<Category>
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
    }
}
