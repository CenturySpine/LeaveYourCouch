using System.Linq;
using System.Web.Mvc;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc.Controllers
{
    public class AccountValidationController : Controller
    {
        /// <summary>
        /// Unobtrusive javascript call to Check if userner (pseudonym) already in use
        /// </summary>
        /// <param name="Email">WARNING == MUST BE SAME NAME/CASING than the checked property</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult DoesUserNameExist(string Pseudo)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u=>u.Pseudo== Pseudo);

                return Json(user == null);
            }

        }

        /// <summary>
        /// Unobtrusive javascript call to Check if email already in use
        /// </summary>
        /// <param name="Email">WARNING == MUST BE SAME NAME/CASING than the checked property</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        
        public JsonResult DoesUserEmailExist(string Email)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == Email);

                return Json(user == null);
            }

        }
    }
}