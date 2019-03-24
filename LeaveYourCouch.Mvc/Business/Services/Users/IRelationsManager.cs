using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc.Business.Services.Users
{
    public interface IRelationsManager
    {
        List<RelationViewModel> GetRelations(RelationshipStatus status);
        Task<ApplicationUser> GetProfile(string userid);
    }
}