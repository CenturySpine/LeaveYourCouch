using System.Collections.Generic;

namespace LeaveYourCouch.Mvc.Models.Relations
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
}