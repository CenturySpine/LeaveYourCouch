using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity;

namespace LeaveYourCouch.Mvc.Models
{
    public class ManageIndexViewModel : SharedPersonalInfos
    {

        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }

        public string UserId { get; set; }

        public HttpPostedFileBase ProfilePictureUpload { get; set; }

        public byte[] ProfilePicture { get; set; }


       

        public Gender Gender { get; set; }
        public string Email { get; set; }
    }
}