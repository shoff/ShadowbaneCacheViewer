namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FixingRenderAndOffset : DbMigration
    {
        public override void Up()
        {
            this.DropPrimaryKey("dbo.RenderAndOffsets");
            this.AddColumn("dbo.RenderAndOffsets", "RenderAndOffsetId", c => c.Int(nullable: false, identity: true));
            this.AddPrimaryKey("dbo.RenderAndOffsets", "RenderAndOffsetId");
        }
        
        public override void Down()
        {
            this.DropPrimaryKey("dbo.RenderAndOffsets");
            this.DropColumn("dbo.RenderAndOffsets", "RenderAndOffsetId");
            this.AddPrimaryKey("dbo.RenderAndOffsets", "RenderId");
        }
    }
}
