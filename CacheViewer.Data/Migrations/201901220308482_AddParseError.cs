namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParseError : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParseErrors",
                c => new
                    {
                        ParseErrorId = c.Long(nullable: false, identity: true),
                        CursorOffset = c.Int(nullable: false),
                        ObjectType = c.Int(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                        InnerOffset = c.Int(nullable: false),
                        RenderId = c.Int(nullable: false),
                        CacheIndexIdentity = c.Int(nullable: false),
                        CacheIndexOffset = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParseErrorId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ParseErrors");
        }
    }
}
