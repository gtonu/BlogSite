using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand
{
    public class ReplyContactUsCommandHandler : ICommandHandler<ReplyContactUsCommand, ContactUs>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReplyContactUsCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ContactUs> Handle(ReplyContactUsCommand command, CancellationToken cancellationToken)
        {
            var existingContactUs = await _unitOfWork.ContactUsRepository.GetByIdAsync(command.Id);
            //var contactUs = _mapper.Map<ContactUs>(command);
            //contactUs.RepliedDate = DateTime.UtcNow;
            existingContactUs.Reply = command.Reply;
            existingContactUs.RepliedDate = DateTime.UtcNow;
            existingContactUs.MarkAsRead = command.MarkAsRead;
            await _unitOfWork.ContactUsRepository.EditAsync(existingContactUs);
            await _unitOfWork.SaveAsync();

            return existingContactUs;
        }
    }
}
