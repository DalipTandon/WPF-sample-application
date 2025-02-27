namespace MyApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CityId = c.Int(nullable: false),
                        City_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Cities", t => t.City_Id)
                .Index(t => t.CityId)
                .Index(t => t.City_Id);
            
            CreateTable(
                "dbo.Streams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SchoolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        SchoolId = c.Int(nullable: false),
                        StreamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Schools", t => t.SchoolId)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.Streams", t => t.StreamId)
                .Index(t => t.StateId)
                .Index(t => t.CityId)
                .Index(t => t.SchoolId)
                .Index(t => t.StreamId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.Cities", "StateId", "dbo.States");
            DropForeignKey("dbo.Schools", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.Students", "StreamId", "dbo.Streams");
            DropForeignKey("dbo.Students", "StateId", "dbo.States");
            DropForeignKey("dbo.Students", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.Students", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Streams", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.Schools", "CityId", "dbo.Cities");
            DropIndex("dbo.Students", new[] { "StreamId" });
            DropIndex("dbo.Students", new[] { "SchoolId" });
            DropIndex("dbo.Students", new[] { "CityId" });
            DropIndex("dbo.Students", new[] { "StateId" });
            DropIndex("dbo.Streams", new[] { "SchoolId" });
            DropIndex("dbo.Schools", new[] { "City_Id" });
            DropIndex("dbo.Schools", new[] { "CityId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropTable("dbo.UserAuths");
            DropTable("dbo.States");
            DropTable("dbo.Students");
            DropTable("dbo.Streams");
            DropTable("dbo.Schools");
            DropTable("dbo.Cities");
        }
    }
}
