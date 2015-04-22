namespace TyoaikaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Testi6 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Bulletins", newName: "Bulletin");
            RenameTable(name: "dbo.TimesheetRows", newName: "TimesheetRow");
            RenameTable(name: "dbo.Timesheets", newName: "Timesheet");
            RenameColumn(table: "dbo.Bulletin", name: "ID", newName: "BulletinId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Bulletin", name: "BulletinId", newName: "ID");
            RenameTable(name: "dbo.Timesheet", newName: "Timesheets");
            RenameTable(name: "dbo.TimesheetRow", newName: "TimesheetRows");
            RenameTable(name: "dbo.Bulletin", newName: "Bulletins");
        }
    }
}
