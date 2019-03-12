using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveYourCouch.Mvc.Models
{
    public class RelationShipsViewModel
    {
        public RelationShipsViewModel()
        {
            Friends = new List<RelationViewModel>();
            Pendings = new List<RelationViewModel>();
            BlackList = new List<RelationViewModel>();
        }

        public List<RelationViewModel> Friends { get; set; }

        public List<RelationViewModel> Pendings { get; set; }

        public List<RelationViewModel> BlackList { get; set; }
    }

    public class RelationViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
    }

  
}