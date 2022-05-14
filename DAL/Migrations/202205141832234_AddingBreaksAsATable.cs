namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBreaksAsATable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Breaks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        BeginningTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
                        CommentsfromEmployee = c.String(),
                        WorkdayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workdays", t => t.WorkdayId, cascadeDelete: true)
                .Index(t => t.WorkdayId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Breaks", "WorkdayId", "dbo.Workdays");
            DropIndex("dbo.Breaks", new[] { "WorkdayId" });
            DropTable("dbo.Breaks");
        }
    }
}
