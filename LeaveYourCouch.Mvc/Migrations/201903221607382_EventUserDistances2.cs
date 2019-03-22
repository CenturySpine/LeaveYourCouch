namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUserDistances2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Event_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Event_Id");
            AddForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events");
            DropIndex("dbo.AspNetUsers", new[] { "Event_Id" });
            DropColumn("dbo.AspNetUsers", "Event_Id");
        }
    }
}
