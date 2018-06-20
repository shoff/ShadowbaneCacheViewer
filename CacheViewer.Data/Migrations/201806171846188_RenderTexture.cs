namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenderTexture : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.RenderTextures",
                c => new
                    {
                        RenderTextureId = c.Int(nullable: false, identity: true),
                        RenderId = c.Int(nullable: false),
                        TextureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RenderTextureId);
            
        }
        
        public override void Down()
        {
            this.DropTable("dbo.RenderTextures");
        }
    }
}
