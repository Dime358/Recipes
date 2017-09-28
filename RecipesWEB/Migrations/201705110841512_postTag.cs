namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postTag : DbMigration
    {
        public override void Up()
        {

            CreateTable(
            "dbo.Tags",
            c => new
            {
                Id = c.Int(nullable: false, identity: true),
                tagName = c.String(nullable: false),
                PostsID = c.Int(nullable: false)
            })
            .PrimaryKey(t => t.Id);

        }
        
        public override void Down()
        {

            DropForeignKey("dbo.Tags", "Id", "dbo.Posts");
            DropIndex("dbo.Tags", new[] { "Id" });
            DropTable("dbo.Tags");


        }
    }
}
