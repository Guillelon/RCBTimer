namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        NationalId = c.String(),
                        NationalIdType = c.String(),
                        Position = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Workdays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BeginningTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Comments = c.String(),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Workdays", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Workdays", new[] { "EmployeeId" });
            DropTable("dbo.Workdays");
            DropTable("dbo.Employees");
        }
    }
}
