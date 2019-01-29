namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoClue : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TextureEntityMeshEntities", newName: "MeshEntityTextureEntities");
            DropPrimaryKey("dbo.MeshEntityTextureEntities");
            AddColumn("dbo.TextureEntities", "RenderEntity_RenderEntityId", c => c.Int());
            AddPrimaryKey("dbo.MeshEntityTextureEntities", new[] { "MeshEntity_MeshEntityId", "TextureEntity_TextureEntityId" });
            CreateIndex("dbo.TextureEntities", "RenderEntity_RenderEntityId");
            AddForeignKey("dbo.TextureEntities", "RenderEntity_RenderEntityId", "dbo.RenderEntities", "RenderEntityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextureEntities", "RenderEntity_RenderEntityId", "dbo.RenderEntities");
            DropIndex("dbo.TextureEntities", new[] { "RenderEntity_RenderEntityId" });
            DropPrimaryKey("dbo.MeshEntityTextureEntities");
            DropColumn("dbo.TextureEntities", "RenderEntity_RenderEntityId");
            AddPrimaryKey("dbo.MeshEntityTextureEntities", new[] { "TextureEntity_TextureEntityId", "MeshEntity_MeshEntityId" });
            RenameTable(name: "dbo.MeshEntityTextureEntities", newName: "TextureEntityMeshEntities");
        }
    }
}
