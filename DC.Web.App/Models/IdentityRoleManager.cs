using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace DC.Web.App.Models
{
    public class IdentityRoleManager
    {
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }
        public string GetRole(string roleId)
        {
            var context = new ApplicationDbContext();
            var role = context.Roles.FirstOrDefault(t => t.Id == roleId);
            if (role != null)
                return role.Name;
            return "";
            //var rm = new RoleManager<IdentityRole>(
            //    new RoleStore<IdentityRole>(new ApplicationDbContext()));
            //var idResult = rm.(new IdentityRole(name));
            //return idResult.Succeeded;
        }


        public IdentityResult CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.UserValidator = new UserValidator<ApplicationUser>(um)
            {
                AllowOnlyAlphanumericUserNames = false,
            };
            um.PasswordHasher = new PasswordHasher();
            var idResult = um.Create(user, password);
            return idResult;
        }

        public bool DeleteUser(string userId)
        {
            var appDb = new ApplicationDbContext();
            var appUser = appDb.Users.FirstOrDefault(u => u.Id == userId);
            if (appUser != null)
            {
                appDb.Users.Remove(appUser);
                return appDb.SaveChanges() > 0;
            }
            return false;
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            um.UserValidator = new UserValidator<ApplicationUser>(um)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }
        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            um.UserValidator = new UserValidator<ApplicationUser>(um)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            // currentRoles.AddRange(user.Roles.ToList());
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.RoleId);
            }
        }
    }
   

}