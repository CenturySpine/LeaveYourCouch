using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using LeaveYourCouch.Mvc.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleInjector;

namespace LeaveYourCouch.Mvc.Models
{
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
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
            
        }
        internal static ApplicationDbContext _instance;
        //public ApplicationDbContext()
        //    : base("DefaultConnection", throwIfV1Schema: false)
        //{
            
        //    SimpleLogger.Log("ApplicationDbContext.Ctor", "Db context contructor");

        //}
        public ApplicationDbContext(string cnString) : base(cnString, throwIfV1Schema: false)
        {
            _instance = this;
        }
        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}



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



        public DbSet<Event> Events { get; set; }
        public DbSet<UserRelationship> Relations { get; set; }

        internal  List<RelationViewModel> GetRelations(RelationshipStatus status)
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

        public List<Event> GetEvents()
        {
            return Events.Include(e=>e.Owner).ToList();
        }

        public void CreateEvent(CreateEventViewModel viewmodel)
        {
            var owner = UserHelpers.UserName();
            Event ev = new Event
            {
                Date = viewmodel.Date,
                Time = viewmodel.Time,
                Description = viewmodel.Description,
                Owner = Users.FirstOrDefault(u => u.Email == owner),
                Title = viewmodel.Title,
                Address = viewmodel.Address,
                IsPrivate = viewmodel.IsPrivate,
                MaxSeats = viewmodel.MaxParticipants,
                MeetingDetails = viewmodel.MeetingPoint
            };

            Events.Add(ev);

            SaveChanges();

        }

        public Event GetEvent(int id)
        {
            return Events.FirstOrDefault(e => e.Id == id);
        }
    }
}