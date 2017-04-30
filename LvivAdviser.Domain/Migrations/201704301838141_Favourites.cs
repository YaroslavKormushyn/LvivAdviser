namespace LvivAdviser.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Favourites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserContents",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Content_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Content_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Content", t => t.Content_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Content_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserContents", "Content_Id", "dbo.Content");
            DropForeignKey("dbo.UserContents", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserContents", new[] { "Content_Id" });
            DropIndex("dbo.UserContents", new[] { "User_Id" });
            DropTable("dbo.UserContents");
        }
    }
}
