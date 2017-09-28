namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "UserId");
        }
    }
}
