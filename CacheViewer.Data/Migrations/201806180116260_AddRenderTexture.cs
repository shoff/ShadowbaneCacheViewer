namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddRenderTexture : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.RenderTextures", "MeshEntity_MeshEntityId", c => c.Int());
            this.CreateIndex("dbo.RenderTextures", "MeshEntity_MeshEntityId");
            this.AddForeignKey("dbo.RenderTextures", "MeshEntity_MeshEntityId", "dbo.MeshEntities", "MeshEntityId");
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.RenderTextures", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            this.DropIndex("dbo.RenderTextures", new[] { "MeshEntity_MeshEntityId" });
            this.DropColumn("dbo.RenderTextures", "MeshEntity_MeshEntityId");
        }
    }
}
