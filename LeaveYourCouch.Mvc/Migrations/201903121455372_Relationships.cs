namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relationships : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRelationships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Issuer_Id = c.String(maxLength: 128),
                        Recipient_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Issuer_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Recipient_Id)
                .Index(t => t.Issuer_Id)
                .Index(t => t.Recipient_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRelationships", "Recipient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRelationships", "Issuer_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserRelationships", new[] { "Recipient_Id" });
            DropIndex("dbo.UserRelationships", new[] { "Issuer_Id" });
            DropTable("dbo.UserRelationships");
        }
    }
}
