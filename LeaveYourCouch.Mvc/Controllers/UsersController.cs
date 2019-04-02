using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using LeaveYourCouch.Mvc.Business.Services;
using LeaveYourCouch.Mvc.Business.Services.Users;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IRelationsManager _relMan;
        private readonly IImageHelper _imgHelper;

        public UsersController(IRelationsManager relMan, IImageHelper imgHelper)
        {
            _relMan = relMan;
            _imgHelper = imgHelper;
        }
        public async Task<ActionResult> Profile(string id, UserInteractions? message)
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
                IsCurrentUser = applicationUser.Email == current,
                IsFriend = await _relMan.GetRelationStatus(RelationshipStatus.Accepted, applicationUser.Id),
                IsFriendRequestPending = await _relMan.GetRelationStatus(RelationshipStatus.Pending, applicationUser.Id),
                IsBalckListed = await _relMan.GetRelationStatus(RelationshipStatus.Blacklisted, applicationUser.Id, RelationDirection.IamRecipient),
                UserName = applicationUser.Pseudo,
                FirstName = applicationUser.FirstName,
                ProfilePicture = !string.IsNullOrEmpty(applicationUser.ProfilePictureName) ? _imgHelper.ToRatioImageDisplay(applicationUser.ProfilePictureName) : null,
                Description = applicationUser.Descrption,
            };
            ViewBag.StatusMessage =
                message == UserInteractions.FriendRequestSent ? "Friend request sent"
                : message == UserInteractions.FriendRequestAccepted ? "Friend request accepted"
                : message == UserInteractions.UserAddedToBlacklist ? "User blacklisted"
                    : message == UserInteractions.RequestCanceled ? "Friend request canceled"
                : string.Empty;
            return View(model);
        }

        public async Task<ActionResult> AddFriend(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _relMan.AddFriendRequest(id);


            return RedirectToAction("Profile", new { id, Message = UserInteractions.FriendRequestSent });
        }

        public async Task<ActionResult> RemoveFriend(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _relMan.RemoveFriend(id);

            return RedirectToAction("Profile", new { id });

        }

        public async Task<ActionResult> AddToBlackList(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            await _relMan.Blacklist(id);



            return RedirectToAction("Profile", new { id });

        }

        public async Task<ActionResult> RemoveFromBlackList(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            await _relMan.UnBlacklist(id);

            return RedirectToAction("Profile", new { id });

        }

        public async Task<ActionResult> Accept(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            await _relMan.AcceptFriendRequest(id);

            return RedirectToAction("Profile", new { id, Message = UserInteractions.FriendRequestAccepted });

        }

        public async Task<ActionResult> Reject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            await _relMan.RejectFriendRequest(id);

            return RedirectToAction("Profile", new { id });

        }

        public async Task<ActionResult> GetPendingRequests()
        {
            var pendinfRequests = await _relMan.GetNonSelfIssuingPendingRequest();
            return PartialView("_PendingRequestsDisplay", pendinfRequests);
        }

        public async Task<ActionResult> UserPseudoAsync(string id)
        {

            var user = await _relMan.UserPseudoAsync(id);
            string pseudo = string.Empty;
            if (user != null && !string.IsNullOrEmpty(user.Pseudo))
            {
                pseudo = user.Pseudo;


            }


            return PartialView("_UserLoginPart", pseudo);
        }

        public async Task<ActionResult> Cancel(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            await _relMan.CancelRequest(id);
            return RedirectToAction("Profile", new { id, Message = UserInteractions.RequestCanceled });

        }


    }
}