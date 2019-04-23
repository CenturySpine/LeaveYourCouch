using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LeaveYourCouch.Mvc.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;

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

 


    }
}