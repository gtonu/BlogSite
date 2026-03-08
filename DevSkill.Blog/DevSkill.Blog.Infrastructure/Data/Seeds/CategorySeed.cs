using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Data.Seeds
{
    public class CategorySeed
    {
        public static Category[] GetCategories()
        {
            return new Category[]
            {
                new Category
                {
                    Id = new Guid("ab05a84d-ffe6-4125-81c4-1ced18cf95e1"),
                    CategoryName = "Web Development"
                },
                new Category
                {
                    Id = new Guid("6e9532ad-9883-4fc3-a0ed-992683c179d8"),
                    CategoryName = "Networking"
                },
                new Category
                {
                    Id = new Guid("31be253f-245c-4653-8bbc-f3fa46c14071"),
                    CategoryName = "JavaScript"
                },
                new Category
                {
                    Id = new Guid("9d9b8e74-d010-432f-9203-5eed53f7b611"),
                    CategoryName = "Software engineering"
                }
            };
        }
    }
}
