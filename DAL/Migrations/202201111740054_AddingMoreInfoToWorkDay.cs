namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMoreInfoToWorkDay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workdays", "BreakBeginningTime", c => c.DateTime());
            AddColumn("dbo.Workdays", "BreakEndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workdays", "BreakEndTime");
            DropColumn("dbo.Workdays", "BreakBeginningTime");
        }
    }
}
