using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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

        [Display(Name = "A propos de vous")]

        public string Descrption { get; set; }
    }
}