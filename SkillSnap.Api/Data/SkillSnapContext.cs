using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Models;

namespace SkillSnap.Api.Data
{
    public class SkillSnapContext : IdentityDbContext<ApplicationUser>
    {
        public SkillSnapContext(DbContextOptions<SkillSnapContext> options)
            : base(options)
        {
        }

        public DbSet<PortfolioUser> PortfolioUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // PortfolioUser → ApplicationUser (1:1)
            builder.Entity<PortfolioUser>()
                .HasOne(p => p.ApplicationUser)
                .WithOne()
                .HasForeignKey<PortfolioUser>(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
