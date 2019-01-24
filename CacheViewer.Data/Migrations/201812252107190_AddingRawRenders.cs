namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRawRenders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RenderRaws",
                c => new
                    {
                        RenderRawId = c.Int(nullable: false, identity: true),
                        Identity = c.Int(nullable: false),
                        Junk1 = c.Int(nullable: false),
                        Offset = c.Int(nullable: false),
                        UnCompressedSize = c.Int(nullable: false),
                        CompressedSize = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.RenderRawId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RenderRaws");
        }
    }
}
