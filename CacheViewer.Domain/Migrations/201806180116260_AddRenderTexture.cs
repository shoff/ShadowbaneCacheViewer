namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRenderTexture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RenderTextures", "MeshEntity_MeshEntityId", c => c.Int());
            CreateIndex("dbo.RenderTextures", "MeshEntity_MeshEntityId");
            AddForeignKey("dbo.RenderTextures", "MeshEntity_MeshEntityId", "dbo.MeshEntities", "MeshEntityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RenderTextures", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            DropIndex("dbo.RenderTextures", new[] { "MeshEntity_MeshEntityId" });
            DropColumn("dbo.RenderTextures", "MeshEntity_MeshEntityId");
        }
    }
}
