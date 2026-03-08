using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.UserReportCommand
{
    public class DeleteUserReportCommandHandler : ICommandHandler<DeleteUserReportCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public DeleteUserReportCommandHandler(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(DeleteUserReportCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserReportRepository.RemoveAsync(command.ReportId);
            await _unitOfWork.SaveAsync();
            return command.ReportId;
        }
    }
}
