namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationEventObject1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Time", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Time");
        }
    }
}
