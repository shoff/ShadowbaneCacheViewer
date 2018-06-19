namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTexturesToMesh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TextureEntities", "MeshEntity_MeshEntityId", c => c.Int());
            CreateIndex("dbo.TextureEntities", "MeshEntity_MeshEntityId");
            AddForeignKey("dbo.TextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities", "MeshEntityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            DropIndex("dbo.TextureEntities", new[] { "MeshEntity_MeshEntityId" });
            DropColumn("dbo.TextureEntities", "MeshEntity_MeshEntityId");
        }
    }
}
