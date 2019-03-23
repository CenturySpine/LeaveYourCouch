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

        internal List<RelationViewModel> GetRelations(RelationshipStatus status)
        {
            List<RelationViewModel> targetList = new List<RelationViewModel>();
            var usr = UserHelpers.UserName();

            //using (var db = new ApplicationDbContext())
            //{

            var usrRelations = Relations.Where(r => (r.Issuer.Email == usr || r.Recipient.Email == usr) && r.Status == status)
                .Include(r => r.Issuer)
                .Include(r => r.Recipient)
                .Select(r => r)
                .ToList();
            List<string> distinctids = new List<string>();
            foreach (var usrr in usrRelations)
            {
                if (!distinctids.Contains(usrr.Issuer.Id))
                    distinctids.Add(usrr.Issuer.Id);
                if (!distinctids.Contains(usrr.Recipient.Id))
                    distinctids.Add(usrr.Recipient.Id);
            }

            var friends2 = (from u in Users
                            where u.Email != usr
                            where distinctids.Contains(u.Id)
                            select u).ToList();



            targetList.AddRange(friends2.Select(f => new RelationViewModel { Id = f.Id, FirstName = f.FirstName, UserName = f.Pseudo }));

            //}

            return targetList;
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