namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectingRenderData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RenderEntities", "HasTexture", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RenderEntities", "HasTexture");
        }
    }
}
