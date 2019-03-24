using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc.Business.Services.Users
{
    public class RelationsManager : IRelationsManager
    {
        private readonly ApplicationDbContext _db;

        public RelationsManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<RelationViewModel> GetRelations(RelationshipStatus status)
        {
            List<RelationViewModel> targetList = new List<RelationViewModel>();
            var usr = UserHelpers.UserName();

            //using (var db = new ApplicationDbContext())
            //{

            var usrRelations = _db.Relations.Where(r => (r.Issuer.Email == usr || r.Recipient.Email == usr) && r.Status == status)
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

            var friends2 = (from u in _db.Users
                where u.Email != usr
                where distinctids.Contains(u.Id)
                select u).ToList();



            targetList.AddRange(friends2.Select(f => new RelationViewModel { Id = f.Id, FirstName = f.FirstName, UserName = f.Pseudo }));

            //}

            return targetList;
        }
    }
}