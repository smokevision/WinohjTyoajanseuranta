namespace TyoaikaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Testi5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "ApplicationUser_Id", "dbo.User");
            DropForeignKey("dbo.Timesheets", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.Employees", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Timesheets", new[] { "EmployeeID" });
            CreateTable(
                "dbo.Bulletins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(maxLength: 128),
                        Header = c.String(),
                        Content = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            AddColumn("dbo.User", "FirstName", c => c.String());
            AddColumn("dbo.User", "LastName", c => c.String());
            AddColumn("dbo.User", "JobTitle", c => c.String());
            AddColumn("dbo.Timesheets", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Timesheets", "ApplicationUserID");
            AddForeignKey("dbo.Timesheets", "ApplicationUserID", "dbo.User", "UserId");
            DropColumn("dbo.User", "EmployeeID");
            DropColumn("dbo.Timesheets", "EmployeeID");
            DropTable("dbo.Employees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        JobTitle = c.String(),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Timesheets", "EmployeeID", c => c.Int(nullable: false));
            AddColumn("dbo.User", "EmployeeID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Timesheets", "ApplicationUserID", "dbo.User");
            DropForeignKey("dbo.Bulletins", "ApplicationUserID", "dbo.User");
            DropIndex("dbo.Timesheets", new[] { "ApplicationUserID" });
            DropIndex("dbo.Bulletins", new[] { "ApplicationUserID" });
            DropColumn("dbo.Timesheets", "ApplicationUserID");
            DropColumn("dbo.User", "JobTitle");
            DropColumn("dbo.User", "LastName");
            DropColumn("dbo.User", "FirstName");
            DropTable("dbo.Bulletins");
            CreateIndex("dbo.Timesheets", "EmployeeID");
            CreateIndex("dbo.Employees", "ApplicationUser_Id");
            AddForeignKey("dbo.Timesheets", "EmployeeID", "dbo.Employees", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "ApplicationUser_Id", "dbo.User", "UserId");
        }
    }
}
