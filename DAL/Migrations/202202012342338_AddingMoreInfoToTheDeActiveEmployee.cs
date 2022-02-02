namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMoreInfoToTheDeActiveEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "DeActivationDate", c => c.DateTime());
            AddColumn("dbo.Employees", "DeActivationBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "DeActivationBy");
            DropColumn("dbo.Employees", "DeActivationDate");
        }
    }
}
