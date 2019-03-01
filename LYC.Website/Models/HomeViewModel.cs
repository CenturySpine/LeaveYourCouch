using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
namespace LYC.Website.Models
{
    public class HomeViewModel
    {

        public HomeViewModel(IPrincipal user)
        {
            ConnectedUsers = new List<string>();
            using (var identdbc = new ApplicationDbContext())
            {
                foreach (var usr in identdbc.Users)
                {
                    if (usr.Email == user.Identity.GetUserName())
                    {
                        CurrentUser = usr.Pseudonyme;
                    }
                }
            }

            if (HttpRuntime.Cache["LoggedInUsers"] != null)
            {
                
            }


        }

        public List<string> ConnectedUsers { get; set; }

        public string CurrentUser { get; set; }
    }
}