namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comments2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
            "dbo.Comments",
            c => new
            {
                Id = c.Int(nullable: false, identity: true),
                CommentBody = c.String(nullable: false),
                CreatedDate = c.DateTime(nullable: false),
                UserId = c.String(nullable: false),
                UserName = c.String(nullable: false),
                PostsID = c.Int(nullable: false)
            })
            .PrimaryKey(t => t.Id);
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Id", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "Id" });
            DropTable("dbo.Comments");
        }
    }
}
