namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingMeshIdNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RenderEntities", "MeshId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RenderEntities", "MeshId", c => c.Int(nullable: false));
        }
    }
}
