namespace CacheViewer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingTextures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TextureEntities",
                c => new
                    {
                        TextureEntityId = c.Int(nullable: false, identity: true),
                        TextureId = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TextureEntityId);
            
            DropTable("dbo.Textures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Textures",
                c => new
                    {
                        TextureId = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TextureId);
            
            DropTable("dbo.TextureEntities");
        }
    }
}
