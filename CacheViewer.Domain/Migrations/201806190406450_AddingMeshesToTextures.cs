namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMeshesToTextures : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            DropIndex("dbo.TextureEntities", new[] { "MeshEntity_MeshEntityId" });
            CreateTable(
                "dbo.TextureEntityMeshEntities",
                c => new
                    {
                        TextureEntity_TextureEntityId = c.Int(nullable: false),
                        MeshEntity_MeshEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TextureEntity_TextureEntityId, t.MeshEntity_MeshEntityId })
                .ForeignKey("dbo.TextureEntities", t => t.TextureEntity_TextureEntityId, cascadeDelete: true)
                .ForeignKey("dbo.MeshEntities", t => t.MeshEntity_MeshEntityId, cascadeDelete: true)
                .Index(t => t.TextureEntity_TextureEntityId)
                .Index(t => t.MeshEntity_MeshEntityId);
            
            DropColumn("dbo.TextureEntities", "MeshEntity_MeshEntityId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TextureEntities", "MeshEntity_MeshEntityId", c => c.Int());
            DropForeignKey("dbo.TextureEntityMeshEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            DropForeignKey("dbo.TextureEntityMeshEntities", "TextureEntity_TextureEntityId", "dbo.TextureEntities");
            DropIndex("dbo.TextureEntityMeshEntities", new[] { "MeshEntity_MeshEntityId" });
            DropIndex("dbo.TextureEntityMeshEntities", new[] { "TextureEntity_TextureEntityId" });
            DropTable("dbo.TextureEntityMeshEntities");
            CreateIndex("dbo.TextureEntities", "MeshEntity_MeshEntityId");
            AddForeignKey("dbo.TextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities", "MeshEntityId");
        }
    }
}
