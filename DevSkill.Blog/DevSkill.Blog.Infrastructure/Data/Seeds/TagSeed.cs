using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Infrastructure.Data.Seeds
{
    public class TagSeed
    {
        public static Tag[] GetTags()
        {
            return new Tag[]
            {
                new Tag
                {
                    Id = new Guid("d478015c-b35b-41c7-8dde-07699be0f1d3"),
                    TagName = "mvc"
                },
                new Tag
                {
                    Id = new Guid("03f0eb39-ba69-4a83-9e3a-9606cdfb7104"),
                    TagName = "asp.net"
                },
                new Tag
                {
                    Id = new Guid("c8560ef7-c037-4b2f-8ce9-b82fc89eb7d4"),
                    TagName = "Ajax"
                }
            };
        }
    }
}
