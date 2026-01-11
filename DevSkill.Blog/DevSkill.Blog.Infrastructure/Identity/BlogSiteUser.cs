using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;

namespace Demo.Infrastructure.Identity
{
    public class BlogSiteUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
