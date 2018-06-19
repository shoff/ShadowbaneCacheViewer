namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCacheIndexId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RenderAndOffsets", "CacheIndexId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RenderAndOffsets", "CacheIndexId");
        }
    }
}
