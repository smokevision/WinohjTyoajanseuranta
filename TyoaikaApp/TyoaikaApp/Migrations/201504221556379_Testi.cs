namespace TyoaikaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Testi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        JobTitle = c.String(),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Timesheets",
                c => new
                    {
                        TimesheetID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Information = c.String(),
                        LunchBreak = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TimesheetID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.TimesheetRows",
                c => new
                    {
                        TimesheetRowID = c.Int(nullable: false, identity: true),
                        TimesheetID = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        StopTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.TimesheetRowID)
                .ForeignKey("dbo.Timesheets", t => t.TimesheetID, cascadeDelete: true)
                .Index(t => t.TimesheetID);
            
            AddColumn("dbo.User", "EmployeeID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimesheetRows", "TimesheetID", "dbo.Timesheets");
            DropForeignKey("dbo.Timesheets", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Employees", "ApplicationUser_Id", "dbo.User");
            DropIndex("dbo.TimesheetRows", new[] { "TimesheetID" });
            DropIndex("dbo.Timesheets", new[] { "EmployeeID" });
            DropIndex("dbo.Employees", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.User", "EmployeeID");
            DropTable("dbo.TimesheetRows");
            DropTable("dbo.Timesheets");
            DropTable("dbo.Employees");
        }
    }
}
