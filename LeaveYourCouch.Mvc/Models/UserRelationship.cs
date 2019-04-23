namespace LeaveYourCouch.Mvc.Models
{
    public class UserRelationship
    {
        public int Id { get; set; }
        public ApplicationUser Issuer { get; set; }
        public ApplicationUser Recipient { get; set; }
        public RelationshipStatus Status { get; set; }
    }
}