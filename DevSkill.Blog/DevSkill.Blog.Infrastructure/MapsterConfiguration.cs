using DevSkill.Blog.Application.Features.Post.Commands.BlogPostCommand;
using DevSkill.Blog.Application.Features.Post.Commands.CategoryCommand;
using DevSkill.Blog.Application.Features.Post.Commands.ContactUsCommand;
using DevSkill.Blog.Application.Features.Post.Commands.TagCommand;
using DevSkill.Blog.Application.Features.Post.Commands.TermsAndConditionsCommand;
using DevSkill.Blog.Application.Features.Post.Commands.UserReportCommand;
using DevSkill.Blog.Domain.Dtos;
using DevSkill.Blog.Domain.Entities;
using DevSkill.Blog.Infrastructure.Identity;
using Mapster;


namespace DevSkill.Blog.Infrastructure
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateBlogPostCommand, BlogPost>();
            config.NewConfig<PublishBlogPostCommand, BlogPost>();
            config.NewConfig<EditBlogPostCommand, BlogPost>();
            config.NewConfig<CreateCategoryCommand, Category>();
            config.NewConfig<EditCategoryCommand, Category>();
            config.NewConfig<CreateContactUsCommand, ContactUs>();
            config.NewConfig<EditContactUsCommand, ContactUs>();
            config.NewConfig<ReplyContactUsCommand, ContactUs>();
            config.NewConfig<CreateTagCommand, Tag>();
            config.NewConfig<EditTagCommand, Tag>();
            config.NewConfig<CreateTermsAndConditionsCommand, TermsAndConditions>();
            config.NewConfig<CreateUserReportCommand, UserReport>();
            config.NewConfig<BlogSiteUser, BlogSiteUserDto>();
            config.NewConfig<BlogPost, BlogPostDto>().MaxDepth(2);
            config.NewConfig<Comment, CommentDto>().MaxDepth(2);
        }
    }
}
