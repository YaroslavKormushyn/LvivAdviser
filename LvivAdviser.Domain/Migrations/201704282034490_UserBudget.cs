namespace LvivAdviser.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserBudget : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Rating", new[] { "UserID" });
            DropIndex("dbo.Rating", new[] { "ContentID" });
            AddColumn("dbo.AspNetUsers", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Rating", "UserId");
            CreateIndex("dbo.Rating", "ContentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Rating", new[] { "ContentId" });
            DropIndex("dbo.Rating", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "Budget");
            CreateIndex("dbo.Rating", "ContentID");
            CreateIndex("dbo.Rating", "UserID");
        }
    }
}
