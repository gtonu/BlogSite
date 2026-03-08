using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand
{
    public class DeleteContactUsCommandHandler : ICommandHandler<DeleteContactUsCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public DeleteContactUsCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteContactUsCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.ContactUsRepository.RemoveAsync(command.Id);
            await _unitOfWork.SaveAsync();

            return command.Id;
        }
    }
}
