namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUserDistances3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events");
            DropIndex("dbo.AspNetUsers", new[] { "Event_Id" });
            CreateTable(
                "dbo.EventParticipations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        SubscriptionTime = c.DateTime(nullable: false),
                        Event_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.User_Id);
            
            DropColumn("dbo.AspNetUsers", "Event_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Event_Id", c => c.Int());
            DropForeignKey("dbo.EventParticipations", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventParticipations", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventParticipations", new[] { "User_Id" });
            DropIndex("dbo.EventParticipations", new[] { "Event_Id" });
            DropTable("dbo.EventParticipations");
            CreateIndex("dbo.AspNetUsers", "Event_Id");
            AddForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events", "Id");
        }
    }
}
