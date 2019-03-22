namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePostalCodeToAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            DropColumn("dbo.AspNetUsers", "PostalCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "PostalCode", c => c.String());
            DropColumn("dbo.AspNetUsers", "Address");
        }
    }
}
