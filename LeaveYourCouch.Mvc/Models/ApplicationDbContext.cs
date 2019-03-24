using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using LeaveYourCouch.Mvc.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleInjector;

namespace LeaveYourCouch.Mvc.Models
{


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        internal static ApplicationDbContext _instance;

        public ApplicationDbContext()
        {

        }


        public ApplicationDbContext(string cnString) : base(cnString, throwIfV1Schema: false)
        {
            _instance = this;
        }
        
        public DbSet<Event> Events { get; set; }

        public DbSet<UserRelationship> Relations { get; set; }

        public DbSet<EventRelativeToUserInformation> UserToEventsData { get; set; }

        public DbSet<EventParticipation> Participations { get; set; }

        public static string UserPseudo(string username)
        {
            //using (var db = ApplicationDbContext.Create())
            //{
            if (_instance.Users.Any())
            {
                var user = _instance.Users.FirstOrDefault(u => u.Email == username);
                if (user != null && !string.IsNullOrEmpty(user.Pseudo))
                {
                    return user.Pseudo;
                }
            }
            //}

            return username;
        }


    }

    public class MigrationsContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        private readonly Container _container;

        public MigrationsContextFactory(Container container)
        {
            _container = container;
        }
        public ApplicationDbContext Create()
        {
            return _container.GetInstance<ApplicationDbContext>();
        }
    }
}