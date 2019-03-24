using System.Collections.Generic;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc.Business.Services.Users
{
    public interface IRelationsManager
    {
        List<RelationViewModel> GetRelations(RelationshipStatus status);
    }
}