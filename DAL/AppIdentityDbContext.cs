using DAL.Models;
using DAL.Users.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Users
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = RoleConstants.USER, NormalizedName = RoleConstants.USER.ToUpper() },
                new IdentityRole { Name = RoleConstants.PREMIUM_USER, NormalizedName = RoleConstants.PREMIUM_USER.ToUpper() },
                new IdentityRole { Name = RoleConstants.ADMIN, NormalizedName = RoleConstants.ADMIN.ToUpper() });
        }
    }
}