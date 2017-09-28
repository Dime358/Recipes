using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RecipesWEB.Models;

[assembly: OwinStartupAttribute(typeof(RecipesWEB.Startup))]
namespace RecipesWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // kreacija i proverka za admin    
            if (!roleManager.RoleExists("Admin"))
            {

                // admin role  
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //osnoven admin               
                var user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "diabloe358@yahoo.com";
                user.EmailConfirmed = true;

                string userPWD = "Dime333.";

                var chkUser = UserManager.Create(user, userPWD);

                //dodavanje na ulogata
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creiranje Editor role 
            if (!roleManager.RoleExists("Editor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Editor";
                roleManager.Create(role);

            }

            // loged in user  
            if (!roleManager.RoleExists("RegisteredUser"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "RegisteredUser";
                roleManager.Create(role);

            }
        }



    }
}
