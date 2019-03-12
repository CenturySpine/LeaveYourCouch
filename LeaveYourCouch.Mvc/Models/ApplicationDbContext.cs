using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeaveYourCouch.Mvc.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
            SimpleLogger.Log("ApplicationDbContext.Ctor", "Db context contructor");

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public static string UserPseudo(string username)
        {
            using (var db = ApplicationDbContext.Create())
            {
                if (db.Users.Any())
                {
                    var user = db.Users.FirstOrDefault(u => u.Email == username);
                    if (user != null && !string.IsNullOrEmpty(user.Pseudo))
                    {
                        return user.Pseudo;
                    }
                }
            }

            return username;
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<UserRelationship> Relations { get; set; }
    }
}