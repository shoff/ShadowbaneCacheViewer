namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenderTextureOffsetToBigInt : DbMigration
    {
        public override void Up()
        {
            this.AlterColumn("dbo.RenderTextures", "Offset", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            this.AlterColumn("dbo.RenderTextures", "Offset", c => c.Int(nullable: false));
        }
    }
}
