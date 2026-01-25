using DevSkill.Blog.Domain;
using DevSkill.Blog.Domain.Repositories;
using DevSkill.Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IBlogPostRepository BlogPostRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public ITagRepository TagRepository { get; private set; }
        public ITermsAndConditionsRepository TermsAndConditionsRepository { get; private set; }
        public ApplicationUnitOfWork(ApplicationDbContext context, IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository,ITagRepository tagRepository,
            ITermsAndConditionsRepository termsAndConditionsRepository) 
            : base(context)
        {
            BlogPostRepository = blogPostRepository;
            CategoryRepository = categoryRepository;
            TagRepository = tagRepository;
            TermsAndConditionsRepository = termsAndConditionsRepository;
        }


    }
}
