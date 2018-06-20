namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTexturesToMesh : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.TextureEntities", "MeshEntity_MeshEntityId", c => c.Int());
            this.CreateIndex("dbo.TextureEntities", "MeshEntity_MeshEntityId");
            this.AddForeignKey("dbo.TextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities", "MeshEntityId");
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.TextureEntities", "MeshEntity_MeshEntityId", "dbo.MeshEntities");
            this.DropIndex("dbo.TextureEntities", new[] { "MeshEntity_MeshEntityId" });
            this.DropColumn("dbo.TextureEntities", "MeshEntity_MeshEntityId");
        }
    }
}
