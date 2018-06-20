namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddingCacheIndexId : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.RenderAndOffsets", "CacheIndexId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            this.DropColumn("dbo.RenderAndOffsets", "CacheIndexId");
        }
    }
}
