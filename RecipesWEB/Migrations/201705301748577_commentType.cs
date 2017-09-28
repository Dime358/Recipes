namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "type");
        }
    }
}
