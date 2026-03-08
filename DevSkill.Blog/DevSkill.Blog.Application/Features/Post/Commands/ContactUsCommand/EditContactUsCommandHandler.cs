using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;

namespace DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand
{
    public class EditContactUsCommandHandler : ICommandHandler<EditContactUsCommand, ContactUs>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EditContactUsCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ContactUs> Handle(EditContactUsCommand command, CancellationToken cancellationToken)
        {
            //var contactUs = _mapper.Map<ContactUs>(command);
            var existingContactUs = await _unitOfWork.ContactUsRepository.GetByIdAsync(command.Id);
            existingContactUs.MarkAsRead = command.MarkAsRead;
            await _unitOfWork.ContactUsRepository.EditAsync(existingContactUs);
            await _unitOfWork.SaveAsync();

            return existingContactUs;
        }
    }
}
