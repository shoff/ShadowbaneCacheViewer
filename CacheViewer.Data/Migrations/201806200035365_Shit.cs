namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Shit : DbMigration
    {
        public override void Up()
        {
            this.DropTable("dbo.CacheIndexEntities");
        }
        
        public override void Down()
        {
            this.CreateTable(
                "dbo.CacheIndexEntities",
                c => new
                    {
                        CacheIndexEntityId = c.Int(nullable: false, identity: true),
                        Offset = c.Int(nullable: false),
                        UnCompressedSize = c.Int(nullable: false),
                        CompressedSize = c.Int(nullable: false),
                        File = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CacheIndexEntityId);
            
        }
    }
}
