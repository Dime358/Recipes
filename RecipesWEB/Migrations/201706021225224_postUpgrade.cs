namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postUpgrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "ingredients", c => c.String());
            AddColumn("dbo.Posts", "approveState", c => c.String());
            AddColumn("dbo.Posts", "prepTime", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "difficulty", c => c.String());
            AddColumn("dbo.Posts", "numPortions", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "numPortions");
            DropColumn("dbo.Posts", "difficulty");
            DropColumn("dbo.Posts", "prepTime");
            DropColumn("dbo.Posts", "approveState");
            DropColumn("dbo.Posts", "ingredients");
        }
    }
}
