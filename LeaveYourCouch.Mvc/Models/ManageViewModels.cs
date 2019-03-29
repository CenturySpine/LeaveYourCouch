using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace LeaveYourCouch.Mvc.Models
{

    public class SharedPersonalInfos
    {
        [Required(), StringLength(100, MinimumLength = 3, ErrorMessage = "The {0} must be at least {2} characters long and maximum {1} characters long.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(), StringLength(30, MinimumLength = 3, ErrorMessage = "The {0} must be at least {2} characters long and maximum {1} characters long.")]
        [Display(Name = "User Name")]
        [Remote("DoesUserNameExist", "AccountValidation", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string Pseudo { get; set; }

        [Required(), StringLength(100, MinimumLength = 5, ErrorMessage = "The {0} must be at least {2} characters long and maximum {1} characters long.")]
        [Display(Name = "Address")]
        
        public string Address { get; set; }

        public bool EmailIsconfirmed { get; set; }
    }
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

    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}