namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validation2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "newsTitle", c => c.String(nullable: false));
            AlterColumn("dbo.News", "newsBody", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "newsBody", c => c.String());
            AlterColumn("dbo.News", "newsTitle", c => c.String());
        }
    }
}
