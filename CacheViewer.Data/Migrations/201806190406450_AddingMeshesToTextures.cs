namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddingMeshesToTextures : DbMigration
    {
        public override void Up()
        {
            this.DropForeignKey("dbo.TextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            this.DropIndex("dbo.TextureEntities", new[] { "MeshEntity_MeshEntityId" });
            this.CreateTable(
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
            
            this.DropColumn("dbo.TextureEntities", "MeshEntity_MeshEntityId");
        }
        
        public override void Down()
        {
            this.AddColumn("dbo.TextureEntities", "MeshEntity_MeshEntityId", c => c.Int());
            this.DropForeignKey("dbo.TextureEntityMeshEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            this.DropForeignKey("dbo.TextureEntityMeshEntities", "TextureEntity_TextureEntityId", "dbo.TextureEntities");
            this.DropIndex("dbo.TextureEntityMeshEntities", new[] { "MeshEntity_MeshEntityId" });
            this.DropIndex("dbo.TextureEntityMeshEntities", new[] { "TextureEntity_TextureEntityId" });
            this.DropTable("dbo.TextureEntityMeshEntities");
            this.CreateIndex("dbo.TextureEntities", "MeshEntity_MeshEntityId");
            this.AddForeignKey("dbo.TextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities", "MeshEntityId");
        }
    }
}
