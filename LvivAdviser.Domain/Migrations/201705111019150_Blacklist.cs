namespace LvivAdviser.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Blacklist : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserContents", newName: "ContentUsers");
            DropPrimaryKey("dbo.ContentUsers");
            CreateTable(
                "dbo.Blacklists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateStart = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        Reason = c.String(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddPrimaryKey("dbo.ContentUsers", new[] { "Content_Id", "User_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blacklists", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Blacklists", new[] { "UserId" });
            DropPrimaryKey("dbo.ContentUsers");
            DropTable("dbo.Blacklists");
            AddPrimaryKey("dbo.ContentUsers", new[] { "User_Id", "Content_Id" });
            RenameTable(name: "dbo.ContentUsers", newName: "UserContents");
        }
    }
}
