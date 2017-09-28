namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notifTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "postTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "postTitle");
        }
    }
}
