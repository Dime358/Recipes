namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        tagName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagsPosts",
                c => new
                    {
                        Tags_Id = c.Int(nullable: false),
                        Posts_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tags_Id, t.Posts_Id })
                .ForeignKey("dbo.Tags", t => t.Tags_Id, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Posts_Id, cascadeDelete: true)
                .Index(t => t.Tags_Id)
                .Index(t => t.Posts_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagsPosts", "Posts_Id", "dbo.Posts");
            DropForeignKey("dbo.TagsPosts", "Tags_Id", "dbo.Tags");
            DropIndex("dbo.TagsPosts", new[] { "Posts_Id" });
            DropIndex("dbo.TagsPosts", new[] { "Tags_Id" });
            DropTable("dbo.TagsPosts");
            DropTable("dbo.Tags");
        }
    }
}
