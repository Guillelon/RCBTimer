namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsActiveToWorkdays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Breaks", "DeActivationDate", c => c.DateTime());
            AddColumn("dbo.Breaks", "DeActivationBy", c => c.String());
            AddColumn("dbo.Workdays", "IsActive", c => c.Boolean(nullable: true));
            AddColumn("dbo.Workdays", "DeActivationDate", c => c.DateTime());
            AddColumn("dbo.Workdays", "DeActivationBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workdays", "DeActivationBy");
            DropColumn("dbo.Workdays", "DeActivationDate");
            DropColumn("dbo.Workdays", "IsActive");
            DropColumn("dbo.Breaks", "DeActivationBy");
            DropColumn("dbo.Breaks", "DeActivationDate");
        }
    }
}
