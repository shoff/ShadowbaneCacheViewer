namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenderTextureOffsetToBigInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RenderTextures", "Offset", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RenderTextures", "Offset", c => c.Int(nullable: false));
        }
    }
}
