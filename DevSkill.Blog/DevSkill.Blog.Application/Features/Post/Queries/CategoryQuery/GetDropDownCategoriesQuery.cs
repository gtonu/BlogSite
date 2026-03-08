using Cortex.Mediator.Queries;
using DevSkill.Blog.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Blog.Application.Features.Post.Queries.CategoryQuery
{
    public class GetDropDownCategoriesQuery : IQuery<IList<Category>>
    {
    }
}
