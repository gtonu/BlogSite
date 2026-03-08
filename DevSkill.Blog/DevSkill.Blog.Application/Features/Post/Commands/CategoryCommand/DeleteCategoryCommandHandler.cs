using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Guid>
    {
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;
        public DeleteCategoryCommandHandler(ILogger<DeleteCategoryCommandHandler> logger,
            IApplicationUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.CategoryRepository.RemoveAsync(command.Id);
                await _unitOfWork.SaveAsync();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong!");
            }
            return command.Id;
        }
    }
}
