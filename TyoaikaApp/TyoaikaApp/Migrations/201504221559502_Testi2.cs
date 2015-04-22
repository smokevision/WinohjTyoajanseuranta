namespace TyoaikaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Testi2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "FirstName");
            DropColumn("dbo.User", "LastName");
            DropColumn("dbo.User", "JobTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "JobTitle", c => c.String());
            AddColumn("dbo.User", "LastName", c => c.String());
            AddColumn("dbo.User", "FirstName", c => c.String());
        }
    }
}
