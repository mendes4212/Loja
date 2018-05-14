using Loja.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(Loja.Startup))]
namespace Loja
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Administrador"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Administrador";
                roleManager.Create(role);

                //Admin da aplicação                  

                Random rnd = new Random();
                var user = new ApplicationUser
                {
                    ClienteID = rnd.Next(1, 10000),
                    UserName = "mendes4212@gmail.com",
                    Email = "mendes4212@gmail.com",
                    Nome = "Vitor Augusto",
                    DataCadastro = DateTime.Now,
                    DataModificacao = DateTime.Now,
                    Ativo = true,
                    DataNascimento = new DateTime(1990, 12, 17, 0, 0, 0),
                };

                string userPWD = "senha1";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Administrador");
                }
            }

            if (!roleManager.RoleExists("Cliente"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Cliente";
                roleManager.Create(role);

            }
        }
    }
}
