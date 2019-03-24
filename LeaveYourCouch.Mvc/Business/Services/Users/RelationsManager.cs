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


        public RelationsManager(ApplicationDbContext db)
        {
            _db = db;

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
            List<ApplicationUser> distinctids = new List<ApplicationUser>();

            foreach (var usrr in usrRelations)
            {
                if (!distinctids.Contains(usrr.Issuer))
                    distinctids.Add(usrr.Issuer);
                if (!distinctids.Contains(usrr.Recipient))
                    distinctids.Add(usrr.Recipient);
            }

            var friends2 = _db.Users.ToList().Where(r => r.Email != usr && distinctids.Any(p => p.Id == r.Id)).ToList();


            foreach (var f in friends2)
            {
                var vm = new RelationViewModel { UserId = f.Id, FirstName = f.FirstName, UserName = f.Pseudo };
                if (status == RelationshipStatus.Pending)
                {
                    //je suis issuer de la demande en attente => je peux la canceler mais pas l'accepter ou la rejeter
                    var relationIssuer = usrRelations.FirstOrDefault(r => r.Issuer.Email == usr && r.Recipient.Id == f.Id);
                    if (relationIssuer != null)
                    {
                        vm.CanCancel = true;
                        vm.CanAcceptOrReject = false;
                    }


                    //je suis le destinataire de demande, je peux l'accepter, la rejeter mais pas la canceler
                    var relationDest = usrRelations.FirstOrDefault(r => r.Recipient.Email == usr && r.Issuer.Id == f.Id);
                    if (relationDest != null)
                    {
                        vm.CanCancel = false;
                        vm.CanAcceptOrReject = true;
                    }
                }
                targetList.Add(vm);
            }






            return targetList;
        }
        public async Task<int> GetNonSelfIssuingPendingRequest()
        {
            var usrMe = UserHelpers.UserName();

            var me = await _db.Users.FirstOrDefaultAsync(r => r.Email == usrMe);
            var rels = await GetRelations(RelationshipStatus.Pending);
            var pending = rels.Count(p => p.UserId != me.Id && p.CanAcceptOrReject);
            return pending;
        }

        public async Task<bool> IsFriend(RelationshipStatus status, string applicationUserId)
        {
            var rels = await GetRelations(status);
            return rels.Any(r => r.UserId == applicationUserId);
        }

        public async Task CancelRequest(string id)
        {
            await RemoveFriend(id);
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