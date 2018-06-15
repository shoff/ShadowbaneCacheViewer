namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingRenderAndOffset : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RenderAndOffsets");
            AddColumn("dbo.RenderAndOffsets", "RenderAndOffsetId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.RenderAndOffsets", "RenderAndOffsetId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RenderAndOffsets");
            DropColumn("dbo.RenderAndOffsets", "RenderAndOffsetId");
            AddPrimaryKey("dbo.RenderAndOffsets", "RenderId");
        }
    }
}
