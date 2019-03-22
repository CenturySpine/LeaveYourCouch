namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUserDistances1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "UserSpecificData_Id", "dbo.EventRelativeToUserInformations");
            DropIndex("dbo.Events", new[] { "UserSpecificData_Id" });
            AddColumn("dbo.EventRelativeToUserInformations", "Event_Id", c => c.Int());
            AddColumn("dbo.EventRelativeToUserInformations", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.EventRelativeToUserInformations", "Event_Id");
            CreateIndex("dbo.EventRelativeToUserInformations", "User_Id");
            AddForeignKey("dbo.EventRelativeToUserInformations", "Event_Id", "dbo.Events", "Id");
            AddForeignKey("dbo.EventRelativeToUserInformations", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Events", "UserSpecificData_Id");
            DropColumn("dbo.EventRelativeToUserInformations", "ApplicationUserId");
            DropColumn("dbo.EventRelativeToUserInformations", "EventId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventRelativeToUserInformations", "EventId", c => c.Int(nullable: false));
            AddColumn("dbo.EventRelativeToUserInformations", "ApplicationUserId", c => c.String());
            AddColumn("dbo.Events", "UserSpecificData_Id", c => c.Int());
            DropForeignKey("dbo.EventRelativeToUserInformations", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventRelativeToUserInformations", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventRelativeToUserInformations", new[] { "User_Id" });
            DropIndex("dbo.EventRelativeToUserInformations", new[] { "Event_Id" });
            DropColumn("dbo.EventRelativeToUserInformations", "User_Id");
            DropColumn("dbo.EventRelativeToUserInformations", "Event_Id");
            CreateIndex("dbo.Events", "UserSpecificData_Id");
            AddForeignKey("dbo.Events", "UserSpecificData_Id", "dbo.EventRelativeToUserInformations", "Id");
        }
    }
}
