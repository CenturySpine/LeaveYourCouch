namespace LeaveYourCouch.Mvc.Models.Relations
{
    public class RelationViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public bool CanAcceptOrReject { get; set; }
        public bool CanCancel { get; set; }
    }
}