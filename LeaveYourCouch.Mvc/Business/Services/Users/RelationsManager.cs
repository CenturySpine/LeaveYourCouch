using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc.Business.Services.Users
{
    public class RelationsManager : IRelationsManager
    {
        private readonly ApplicationDbContext _db;
        public static RelationsManager _instance;

        public RelationsManager(ApplicationDbContext db)
        {
            _db = db;
            _instance = this;
        }

        public async Task<List<RelationViewModel>> GetRelations(RelationshipStatus status)
        {
            List<RelationViewModel> targetList = new List<RelationViewModel>();
            var usr = UserHelpers.UserName();



            var usrRelations = await _db.Relations.Where(r => (r.Issuer.Email == usr || r.Recipient.Email == usr) && r.Status == status)
                .Include(r => r.Issuer)
                .Include(r => r.Recipient)
                .Select(r => r)
                .ToListAsync();
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



            targetList.AddRange(friends2.Select(f => new RelationViewModel { UserId = f.Id, FirstName = f.FirstName, UserName = f.Pseudo }));



            return targetList;
        }
        public async Task<int> GetNonSelfIssuingPendingREquest()
        {
            var usrMe = UserHelpers.UserName();
            
            var me = await _db.Users.FirstOrDefaultAsync(r => r.Email == usrMe);
            var rels = await GetRelations(RelationshipStatus.Pending);
            var pending = rels.Count(p => p.UserId != me.Id);
            return pending;
        }

        public async Task<bool> IsFriend(RelationshipStatus status,string applicationUserId)
        {
          var rels=  await GetRelations(RelationshipStatus.Accepted);
          return rels.Any(r => r.UserId == applicationUserId);
        }

        public async Task<ApplicationUser> GetProfile(string userid)
        {


            var targetuser = await _db.Users.FirstOrDefaultAsync(u => u.Id == userid);

            return targetuser;
        }

        public async Task AddFriendRequest(string id)
        {
            var usrMe = UserHelpers.UserName();
            var targetFriend = await _db.Users.FirstOrDefaultAsync(r => r.Id == id);
            var me = await _db.Users.FirstOrDefaultAsync(r => r.Email == usrMe);
            _db.Relations.Add(new UserRelationship
            {
                Issuer = me,
                Recipient = targetFriend,
                Status = RelationshipStatus.Pending
            });
            await _db.SaveChangesAsync();
        }

        public async Task RemoveFriend(string id)
        {
            var usrMe = UserHelpers.UserName();
            var targetFriend = await _db.Users.FirstOrDefaultAsync(r => r.Id == id);
            var me = await _db.Users.FirstOrDefaultAsync(r => r.Email == usrMe);

            var targetRelation = await _db.Relations.Include(r => r.Issuer).Include(r => r.Recipient).FirstOrDefaultAsync(r =>
                (r.Issuer.Id == me.Id && r.Recipient.Id == targetFriend.Id) ||
                (r.Issuer.Id == targetFriend.Id && r.Recipient.Id == me.Id));

            if (targetRelation != null)
            {
                _db.Relations.Remove(targetRelation);
                await _db.SaveChangesAsync();
            }
        }

        public async Task AcceptFriendRequest(string id)
        {
            var usrMe = UserHelpers.UserName();
            var targetFriend = await _db.Users.FirstOrDefaultAsync(r => r.Id == id);
            var me = await _db.Users.FirstOrDefaultAsync(r => r.Email == usrMe);
            var targetRelation = await _db.Relations.Include(r => r.Issuer).Include(r => r.Recipient).FirstOrDefaultAsync(r =>
                (r.Issuer.Id == me.Id && r.Recipient.Id == targetFriend.Id) ||
                (r.Issuer.Id == targetFriend.Id && r.Recipient.Id == me.Id));
            if (targetRelation != null)
            {
                targetRelation.Status = RelationshipStatus.Accepted;
                _db.Entry(targetRelation).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

     
    }
}