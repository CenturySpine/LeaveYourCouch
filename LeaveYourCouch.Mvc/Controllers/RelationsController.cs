using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeaveYourCouch.Mvc.Business.Services.Users;
using LeaveYourCouch.Mvc.Models;
using Microsoft.AspNet.Identity;

namespace LeaveYourCouch.Mvc.Controllers
{
    public static class UserHelpers
    {
        public static string UserName()
        {
            var usr = HttpContext.Current.User.Identity.GetUserName();
            return usr;
        }
    }



    [Authorize]
    public class RelationsController : Controller
    {
        private readonly IRelationsManager _relman;

        public RelationsController(IRelationsManager relman)
        {
            _relman = relman;
        }
        // GET: Relations
        public async Task<ActionResult> Index()
        {
            RelationShipsViewModel vm = new RelationShipsViewModel();
            vm.BlackList = await _relman.GetRelations(RelationshipStatus.Blacklisted,RelationDirection.IamIssuer);
            vm.Friends = await _relman.GetRelations(RelationshipStatus.Accepted);
            vm.Pendings = await _relman.GetRelations(RelationshipStatus.Pending);
            return View(vm);
        }



    }


    public enum UserInteractions
    {
        FriendRequestSent,
        FriendRequestAccepted,
        FriendRemoved,
        UserAddedToBlacklist,
        RequestCanceled
    }

    public class UserProfileModel
    {
        public string UserId { get; set; }
        public bool IsCurrentUser { get; set; }
        public bool IsFriend { get; set; }
        public bool IsFriendRequestPending { get; set; }
        public bool IsBalckListed { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public byte[] ProfilePicture { get; internal set; }
        public string Description { get; internal set; }
    }
}