namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        MaxSeats = c.Int(nullable: false),
                        Address = c.String(),
                        MeetingDetails = c.String(),
                        IsPrivate = c.Boolean(nullable: false),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "Owner_Id" });
            DropTable("dbo.Events");
        }
    }
}
