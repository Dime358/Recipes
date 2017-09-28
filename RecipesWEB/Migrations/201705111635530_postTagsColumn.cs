namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postTagsColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "postTags", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "postTags");
        }
    }
}
