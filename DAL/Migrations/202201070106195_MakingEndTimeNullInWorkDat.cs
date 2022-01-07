namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingEndTimeNullInWorkDat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Workdays", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Workdays", "EndTime", c => c.DateTime(nullable: false));
        }
    }
}
