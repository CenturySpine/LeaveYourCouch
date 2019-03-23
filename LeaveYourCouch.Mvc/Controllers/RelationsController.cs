using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
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

    static class RelationshipHelper
    {
        //internal static List<RelationViewModel> GetRelations(RelationshipStatus status)
        //{
        //    List<RelationViewModel> targetList= new List<RelationViewModel>();
        //    var usr = UserHelpers.UserName();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var usrRelations = db.Relations.Where(r => (r.Issuer.Email == usr || r.Recipient.Email == usr) && r.Status == status)
        //            .Include(r => r.Issuer)
        //            .Include(r => r.Recipient)
        //            .Select(r => r)
        //            .ToList();
        //        List<string> distinctids = new List<string>();
        //        foreach (var usrr in usrRelations)
        //        {
        //            if (!distinctids.Contains(usrr.Issuer.Id))
        //                distinctids.Add(usrr.Issuer.Id);
        //            if (!distinctids.Contains(usrr.Recipient.Id))
        //                distinctids.Add(usrr.Recipient.Id);
        //        }

        //        var friends2 = (from u in db.Users
        //            where u.Email != usr
        //            where distinctids.Contains(u.Id)
        //            select u).ToList();



        //        targetList.AddRange(friends2.Select(f=>new RelationViewModel{ Id=f.Id,FirstName=f.FirstName,UserName=f.Pseudo}));

        //    }

        //    return targetList;
        //}

    }

    [Authorize]
    public class RelationsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public RelationsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: Relations
        public ActionResult Index()
        {
            RelationShipsViewModel vm = new RelationShipsViewModel();
            vm.BlackList = _dbContext.GetRelations(RelationshipStatus.Blacklisted);
            vm.Friends = _dbContext.GetRelations(RelationshipStatus.Accepted);
            vm.Pendings = _dbContext.GetRelations(RelationshipStatus.Pending);
            return View(vm);
        }
    }
}