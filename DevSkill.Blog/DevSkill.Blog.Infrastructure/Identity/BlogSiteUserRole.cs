using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demo.Infrastructure.Identity
{
    public class BlogSiteUserRole
        : IdentityUserRole<Guid>
    {
       
    }
}
