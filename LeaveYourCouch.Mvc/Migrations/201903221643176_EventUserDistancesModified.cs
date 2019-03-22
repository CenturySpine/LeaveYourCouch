namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUserDistancesModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventRelativeToUserInformations", "Duration", c => c.Double(nullable: false));
            AddColumn("dbo.EventRelativeToUserInformations", "DirectionMode", c => c.Int(nullable: false));
            DropColumn("dbo.EventRelativeToUserInformations", "DurationDriving");
            DropColumn("dbo.EventRelativeToUserInformations", "DurationWalking");
            DropColumn("dbo.EventRelativeToUserInformations", "DurationCycling");
            DropColumn("dbo.EventRelativeToUserInformations", "DurationTransit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventRelativeToUserInformations", "DurationTransit", c => c.Double(nullable: false));
            AddColumn("dbo.EventRelativeToUserInformations", "DurationCycling", c => c.Double(nullable: false));
            AddColumn("dbo.EventRelativeToUserInformations", "DurationWalking", c => c.Double(nullable: false));
            AddColumn("dbo.EventRelativeToUserInformations", "DurationDriving", c => c.Double(nullable: false));
            DropColumn("dbo.EventRelativeToUserInformations", "DirectionMode");
            DropColumn("dbo.EventRelativeToUserInformations", "Duration");
        }
    }
}
