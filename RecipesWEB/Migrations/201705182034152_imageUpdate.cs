namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "imagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "imagePath");
        }
    }
}
