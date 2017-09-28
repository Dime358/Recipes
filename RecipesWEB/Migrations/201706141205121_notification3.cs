namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notification3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "forUserId", c => c.String());
            AddColumn("dbo.Notifications", "fromUserName", c => c.String());
            AddColumn("dbo.Notifications", "notificationType", c => c.String());
            DropColumn("dbo.Notifications", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "UserId", c => c.String());
            DropColumn("dbo.Notifications", "notificationType");
            DropColumn("dbo.Notifications", "fromUserName");
            DropColumn("dbo.Notifications", "forUserId");
        }
    }
}
