namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenderTextureOffset : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.RenderTextures", "Offset", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            this.DropColumn("dbo.RenderTextures", "Offset");
        }
    }
}
