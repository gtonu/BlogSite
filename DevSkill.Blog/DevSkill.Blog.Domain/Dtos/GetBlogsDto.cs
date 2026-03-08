using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Domain.Dtos
{
    public class GetBlogsDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int PostCount { get; set; }
    }
}
