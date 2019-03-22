namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUserDistances : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventRelativeToUserInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(),
                        EventId = c.Int(nullable: false),
                        Unit = c.String(),
                        Distance = c.Double(nullable: false),
                        DurationDriving = c.Double(nullable: false),
                        DurationWalking = c.Double(nullable: false),
                        DurationCycling = c.Double(nullable: false),
                        DurationTransit = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "UserSpecificData_Id", c => c.Int());
            CreateIndex("dbo.Events", "UserSpecificData_Id");
            AddForeignKey("dbo.Events", "UserSpecificData_Id", "dbo.EventRelativeToUserInformations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "UserSpecificData_Id", "dbo.EventRelativeToUserInformations");
            DropIndex("dbo.Events", new[] { "UserSpecificData_Id" });
            DropColumn("dbo.Events", "UserSpecificData_Id");
            DropTable("dbo.EventRelativeToUserInformations");
        }
    }
}
