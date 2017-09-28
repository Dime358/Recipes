namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsImg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "imagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "imagePath");
        }
    }
}
