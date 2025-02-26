namespace MyApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthtbales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAuths",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserAuths");
        }
    }
}
