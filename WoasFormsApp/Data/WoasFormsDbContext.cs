using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WoasFormsApp.Data
{
    public class WoasFormsDbContext(DbContextOptions<WoasFormsDbContext> options) : IdentityDbContext<WoasFormsAppUser>(options)
    {
        public DbSet<Template> Templates { get; set; }
        
        public DbSet<TemplateTag> TemplateTags { get; set; }
        public DbSet<TemplateTopic> TemplateTopics { get; set; }

        public DbSet<TemplateField> TemplateFields { get; set; }

        public DbSet<Response> Responses { get; set; }
        
        public DbSet<TemplateComment> TemplateComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<WoasFormsAppUser>().HasMany(u => u.OwnedTemplates).WithOne(t => t.Owner);

            

            builder.Entity<Template>().HasMany(t => t.Tags).WithMany();
            builder.Entity<Template>().HasMany(t => t.UsersWhoLiked).WithMany();
            builder.Entity<Template>().HasMany(t => t.Comments).WithOne(c => c.Template);
            builder.Entity<Template>().HasMany(t => t.AllowedUsers).WithMany();
            builder.Entity<Template>().HasMany(t => t.Responses).WithOne(r => r.Template);
            builder.Entity<Template>().HasMany(t => t.Fields).WithOne(f => f.Template);

            

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }
}
