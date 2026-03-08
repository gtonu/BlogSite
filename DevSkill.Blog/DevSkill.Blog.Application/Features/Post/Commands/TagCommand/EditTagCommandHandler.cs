using Cortex.Mediator.Commands;
using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Entities;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Commands.TagCommand
{
    public class EditTagCommandHandler : ICommandHandler<EditTagCommand, Tag>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EditTagCommandHandler(IApplicationUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Tag> Handle(EditTagCommand command, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<Tag>(command);
            await _unitOfWork.TagRepository.EditAsync(tag);
            await _unitOfWork.SaveAsync();

            return tag;
        }
    }
}
