using DotnetAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotnetAuthentication.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,
        AppRole, Guid,IdentityUserClaim<Guid>,AppUserRole,
        IdentityUserLogin<Guid>,IdentityRoleClaim<Guid>,IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

            

        }

       

    }
}
