namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsUse2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "UserName");
        }
    }
}
