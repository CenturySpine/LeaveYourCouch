namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMapLinktoEventUserDirections : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventRelativeToUserInformations", "MapLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventRelativeToUserInformations", "MapLink");
        }
    }
}
