namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notifications2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "CreatedDate");
        }
    }
}
