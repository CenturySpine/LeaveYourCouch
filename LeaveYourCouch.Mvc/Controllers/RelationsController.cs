using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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
        public ActionResult Index()
        {
            RelationShipsViewModel vm = new RelationShipsViewModel();
            vm.BlackList = _relman.GetRelations(RelationshipStatus.Blacklisted);
            vm.Friends = _relman.GetRelations(RelationshipStatus.Accepted);
            vm.Pendings = _relman.GetRelations(RelationshipStatus.Pending);
            return View(vm);
        }



    }

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IRelationsManager _relMan;

        public UsersController(IRelationsManager relMan)
        {
            _relMan = relMan;
        }
        public async Task<ActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await _relMan.GetProfile(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var current = UserHelpers.UserName();
            UserProfileModel model = new UserProfileModel
            {
                UserId = applicationUser.Id,
                IsCurrentUser=applicationUser.Email== current,
            };

            return View(model);
        }

        public ActionResult AddFriend(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction("Profile", new { id });
        }

        public ActionResult RemoveFriend(string id)
        {
            throw new NotImplementedException();
        }

        public ActionResult AddToBlackList(string id)
        {
            throw new NotImplementedException();
        }
    }

    public class UserProfileModel
    {
        public string UserId { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}