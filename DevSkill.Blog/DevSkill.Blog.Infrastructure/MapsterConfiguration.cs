using DevSkill.Blog.Application.Features.Post.Commands.BlogCommand;
using DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand;
using DevSkill.Blog.Application.Features.Post.Commands.TagCommand;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Infrastructure.Identity;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateBlogPostCommand, BlogPost>();
            config.NewConfig<CreateCategoryCommand, Category>();
            config.NewConfig<CreateTagCommand, Tag>();
        }
    }
}
