namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsActiveAndMorecommentstoemployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Workdays", "CommentsfromEmployee", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workdays", "CommentsfromEmployee");
            DropColumn("dbo.Employees", "IsActive");
        }
    }
}
