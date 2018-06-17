namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenderTextureOffset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RenderTextures", "Offset", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RenderTextures", "Offset");
        }
    }
}
