namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validation1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "PostsID", "dbo.Posts");
            DropIndex("dbo.Tags", new[] { "PostsID" });
            AlterColumn("dbo.Posts", "postTitle", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Posts", "postBody", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "postCategory", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "ingredients", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "difficulty", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "difficulty", c => c.String());
            AlterColumn("dbo.Posts", "ingredients", c => c.String());
            AlterColumn("dbo.Posts", "postCategory", c => c.String());
            AlterColumn("dbo.Posts", "postBody", c => c.String());
            AlterColumn("dbo.Posts", "postTitle", c => c.String());
            CreateIndex("dbo.Tags", "PostsID");
            AddForeignKey("dbo.Tags", "PostsID", "dbo.Posts", "Id", cascadeDelete: true);
        }
    }
}
