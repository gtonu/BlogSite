using Cortex.Mediator;
using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand
{
    public class CreateContactUsCommandHandler : ICommandHandler<CreateContactUsCommand, ContactUs>
    {
        private readonly ILogger<CreateContactUsCommandHandler> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateContactUsCommandHandler(ILogger<CreateContactUsCommandHandler> logger,IApplicationUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        

        public async Task<ContactUs> Handle(CreateContactUsCommand command, CancellationToken cancellationToken)
        {
            var contactUs = _mapper.Map<ContactUs>(command);
            contactUs.Id = Guid.NewGuid();
            contactUs.ReceivedDate = DateTime.UtcNow;

            await _unitOfWork.ContactUsRepository.AddAsync(contactUs);
            await _unitOfWork.SaveAsync();

            return contactUs;
        }
    }
}
