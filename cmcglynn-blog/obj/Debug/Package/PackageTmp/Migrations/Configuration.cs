namespace cmcglynn_blog.Migrations
{
    using cmcglynn_blog.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<cmcglynn_blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;  //MUST CHANGE AUTOMATIC MIGRATIONS ENABLED TO "TRUE"
        }

        protected override void Seed(cmcglynn_blog.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {

                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {

                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "qpc4ever@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "qpc4ever@gmail.com",
                    Email = "qpc4ever@gmail.com",
                    FirstName = "Chris",
                    LastName = "McGlynn",
                }, "Qpc4ever!");
            }
            var userId = userManager.FindByEmail("qpc4ever@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "Moderator@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Moderator@gmail.com",
                    Email = "Moderator@gmail.com",
                    FirstName = "Coder",
                    LastName = "Foundry",
                }, "Password1!");
            }
            var userId_cf = userManager.FindByEmail("Moderator@gmail.com").Id;
            userManager.AddToRole(userId_cf, "Moderator");
        }
    }
}
