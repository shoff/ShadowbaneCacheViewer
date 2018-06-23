namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRenderEntityToCacheObjects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RenderEntities", "CacheObjectEntity_CacheObjectEntityId", c => c.Int());
            CreateIndex("dbo.RenderEntities", "CacheObjectEntity_CacheObjectEntityId");
            AddForeignKey("dbo.RenderEntities", "CacheObjectEntity_CacheObjectEntityId", "dbo.CacheObjectEntities", "CacheObjectEntityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RenderEntities", "CacheObjectEntity_CacheObjectEntityId", "dbo.CacheObjectEntities");
            DropIndex("dbo.RenderEntities", new[] { "CacheObjectEntity_CacheObjectEntityId" });
            DropColumn("dbo.RenderEntities", "CacheObjectEntity_CacheObjectEntityId");
        }
    }
}
