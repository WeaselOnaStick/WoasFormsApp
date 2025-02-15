using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WoasFormsApp.Data;

namespace WoasFormsApp
{
    public class Seeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            string[] roleNames = { "Admin" };
            string[] adminUsernames = { };


            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetService<UserManager<WoasFormsAppUser>>();


            foreach (var roleName in roleNames)
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));

            foreach (var adminUsername in adminUsernames)
            {
                var adminUser = await userManager.FindByNameAsync(adminUsername);
                if ((adminUser != null) && (!await userManager.IsInRoleAsync(adminUser, "Admin")))
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        public static readonly string[] FieldTypeNames = { "SingleLine", "MultiLine","PositiveInt","CheckBox" };
        public static async Task SeedFieldTypes(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<WoasFormsDbContext>();
            foreach (var fieldTypeName in FieldTypeNames)
            {
                var fieldTypeEntity = await db.FieldTypes.FirstOrDefaultAsync(ft => ft.Name == fieldTypeName);
                if (fieldTypeEntity == null)
                {
                    await db.FieldTypes.AddAsync(new TemplateFieldType
                    {
                        Name = fieldTypeName,
                    });
                }
                await db.SaveChangesAsync();
            }
        }

        public static readonly string[] TopicNames = { "Other", "Quiz", "Education" };
        public static async Task SeedTopics(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<WoasFormsDbContext>();
            foreach (var topicName in TopicNames)
            {
                var tEntity = await db.TemplateTopics.FirstOrDefaultAsync(ft => ft.Title == topicName);
                if (tEntity == null)
                {
                    await db.TemplateTopics.AddAsync(new TemplateTopic
                    {
                        Title = topicName,
                    });
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
