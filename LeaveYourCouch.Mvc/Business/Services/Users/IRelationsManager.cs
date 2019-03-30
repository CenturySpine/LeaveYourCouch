using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc.Business.Services.Users
{
    public interface IRelationsManager
    {
        Task<List<RelationViewModel>> GetRelations(RelationshipStatus status, RelationDirection direction = RelationDirection.All);
        Task<ApplicationUser> GetProfile(string userid);

        Task AddFriendRequest(string id);
        Task RemoveFriend(string id);
        Task AcceptFriendRequest(string id);
        Task<int> GetNonSelfIssuingPendingRequest();
        Task<bool> IsFriend(RelationshipStatus status, string applicationUserId);
        Task CancelRequest(string id);
        Task Blacklist(string id);
        Task RejectFriendRequest(string id);
        Task UnBlacklist(string id);
    }
}