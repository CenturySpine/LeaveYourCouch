using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
    public class UsersControler : Controller
    {

    }
}