namespace TyoaikaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Testi4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "ApplicationUserID", c => c.String());
            DropColumn("dbo.Employees", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "UserID", c => c.String());
            DropColumn("dbo.Employees", "ApplicationUserID");
        }
    }
}
