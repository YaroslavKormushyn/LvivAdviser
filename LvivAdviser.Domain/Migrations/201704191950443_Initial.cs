namespace LvivAdviser.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Content",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Rating",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Rate = c.Int(nullable: false),
                        Comment = c.String(),
                        UserID = c.Int(nullable: false),
                        ContentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Content", t => t.ContentID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ContentID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rating", "UserID", "dbo.User");
            DropForeignKey("dbo.Rating", "ContentID", "dbo.Content");
            DropIndex("dbo.Rating", new[] { "ContentID" });
            DropIndex("dbo.Rating", new[] { "UserID" });
            DropTable("dbo.User");
            DropTable("dbo.Rating");
            DropTable("dbo.Content");
        }
    }
}
