using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Repositories;
using DevSkill.Blog.Infrastructure.Data;
using DevSkill.Blog.Infrastructure.Identity.Interfaces;

namespace DevSkill.Blog.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IBlogPostRepository BlogPostRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IContactUsRepository ContactUsRepository { get; private set; }
        public ITagRepository TagRepository { get; private set; }
        public ITermsAndConditionsRepository TermsAndConditionsRepository { get; private set; }
        public IBlogSiteUserRepository BlogSiteUserRepository { get; private set; }
        public IUserReportRepository UserReportRepository { get; private set; }
        public ApplicationUnitOfWork(ApplicationDbContext context, IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository,
            IContactUsRepository contactUsRepository,
            ITagRepository tagRepository,
            ITermsAndConditionsRepository termsAndConditionsRepository,
            IBlogSiteUserRepository blogSiteUserRepository,
            IUserReportRepository userReportRepository) 
            : base(context)
        {
            BlogPostRepository = blogPostRepository;
            CategoryRepository = categoryRepository;
            ContactUsRepository = contactUsRepository;
            TagRepository = tagRepository;
            TermsAndConditionsRepository = termsAndConditionsRepository;
            BlogSiteUserRepository = blogSiteUserRepository;
            UserReportRepository = userReportRepository;
        }


    }
}
