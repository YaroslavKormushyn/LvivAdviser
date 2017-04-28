namespace LvivAdviser.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentImageByteArray : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Content", "MainPhoto", c => c.Binary());
            DropColumn("dbo.Content", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Content", "Rating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Content", "MainPhoto");
        }
    }
}
