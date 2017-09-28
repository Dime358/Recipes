namespace RecipesWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userImg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.userImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        image = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.userImages");
        }
    }
}
