using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DC.Web.App.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public DateTime? DOB { get; set; } 
        public string MobNo { get; set; }
        public string EmailId { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string eUserName { get; set; }
        public string ePassword { get; set; }
        public bool MobValidate { get; set; }
        public bool EmailValidate { get; set; }
        public int LoginMode { get; set; }
        public bool LoginThirdParty { get; set; }
        public int LoginSourse { get; set; }
        public DateTime? LastLogin { get; set; }

        public string Image { get; set; }
        public DateTime? dtmCreate { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public DateTime? dtmDelete { get; set; }
        public int Status { get; set; }
        public string TimeZone { get; set; }
        public string AccessLevels { get; set; }
        public int DisplayOrder { get; set; }

        public string UserExcelMetaDataId { get; set; }
        public string UniqueId { get; set; }
        public int? FromUpload { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}