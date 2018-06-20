namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FixingTextures : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
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
            
            this.DropTable("dbo.Textures");
        }
        
        public override void Down()
        {
            this.CreateTable(
                "dbo.Textures",
                c => new
                    {
                        TextureId = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TextureId);
            
            this.DropTable("dbo.TextureEntities");
        }
    }
}
