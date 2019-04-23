using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.Models;
using LeaveYourCouch.Mvc.Models.Relations;

namespace LeaveYourCouch.Mvc.Business.Services.Users
{

    public enum RelationDirection
    {
        All,
        IamIssuer,
        IamRecipient,

    }
    public class RelationsManager : IRelationsManager
    {
        private readonly ApplicationDbContext _db;


        public RelationsManager(ApplicationDbContext db)
        {
            _db = db;

        }

        public async Task<List<RelationViewModel>> GetRelations(RelationshipStatus status, RelationDirection direction = RelationDirection.All)
        {
            List<RelationViewModel> targetList = new List<RelationViewModel>();
            var usr = UserHelpers.UserName();

            Func<UserRelationship, bool> selectorPredicate;

            switch (direction)
            {
                case RelationDirection.All:
                    selectorPredicate = r => (r.Issuer.Email == usr || r.Recipient.Email == usr);
                    break;
                case RelationDirection.IamIssuer:
                    selectorPredicate = r => (r.Issuer.Email == usr );

                    break;
                case RelationDirection.IamRecipient:
                    selectorPredicate = r => ( r.Recipient.Email == usr);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            var usrRelationsTmp = await _db.Relations
                .Include(r => r.Issuer)
                .Include(r => r.Recipient)
                .Select(r => r)
                .ToListAsync();

            var usrRelations = usrRelationsTmp.Where(r => selectorPredicate(r) && r.Status == status);
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

        public async Task<bool> GetRelationStatus(RelationshipStatus status, string applicationUserId, RelationDirection direction = RelationDirection.All)
        {
            var rels = await GetRelations(status, direction);
            return rels.Any(r => r.UserId == applicationUserId);
        }

        public async Task CancelRequest(string id)
        {
            await RemoveFriend(id);
        }

        public async Task Blacklist(string id)
        {
            var usrMe = UserHelpers.UserName();
            var targetFriend = await _db.Users.FirstOrDefaultAsync(r => r.Id == id);
            var me = await _db.Users.FirstOrDefaultAsync(r => r.Email == usrMe);
            _db.Relations.Add(new UserRelationship
            {
                Issuer = me,
                Recipient = targetFriend,
                Status = RelationshipStatus.Blacklisted
            });
            await _db.SaveChangesAsync();
        }

        public async Task UnBlacklist(string id)
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
         

            var targetRelation = await GetTargetFriendRequest(id);
            if (targetRelation != null)
            {
                _db.Relations.Remove(targetRelation);
                await _db.SaveChangesAsync();
            }
        }

        public async Task AcceptFriendRequest(string id)
        {
            var targetRelation = await GetTargetFriendRequest(id);
            if (targetRelation != null)
            {
                targetRelation.Status = RelationshipStatus.Accepted;
                _db.Entry(targetRelation).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

        private async Task<UserRelationship> GetTargetFriendRequest(string id)
        {
            var usrMe = UserHelpers.UserName();
            var targetFriend = await _db.Users.FirstOrDefaultAsync(r => r.Id == id);
            var me = await _db.Users.FirstOrDefaultAsync(r => r.Email == usrMe);
            var targetRelation = await _db.Relations.Include(r => r.Issuer).Include(r => r.Recipient).FirstOrDefaultAsync(r =>
                (r.Issuer.Id == me.Id && r.Recipient.Id == targetFriend.Id) ||
                (r.Issuer.Id == targetFriend.Id && r.Recipient.Id == me.Id));
            return targetRelation;
        }

        public async Task RejectFriendRequest(string id)
        {
            var targetRelation = await GetTargetFriendRequest(id);
            if (targetRelation != null)
            {
                targetRelation.Status = RelationshipStatus.Rejected;
                _db.Entry(targetRelation).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<ApplicationUser> UserPseudoAsync(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == username);
            return user;
        }
    }
}