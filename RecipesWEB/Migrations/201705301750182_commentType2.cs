namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentType2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Comments", "type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "type", c => c.String());
        }
    }
}
