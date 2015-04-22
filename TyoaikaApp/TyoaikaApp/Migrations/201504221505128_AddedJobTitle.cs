namespace TyoaikaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedJobTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "JobTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "JobTitle");
        }
    }
}
