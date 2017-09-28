namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "reportReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "reportReason");
        }
    }
}
