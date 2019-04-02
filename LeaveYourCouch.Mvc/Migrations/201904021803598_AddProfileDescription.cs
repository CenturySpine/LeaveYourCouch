namespace LeaveYourCouch.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Descrption", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Descrption");
        }
    }
}
