namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvalidValues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvalidValues",
                c => new
                    {
                        InvalidValueId = c.Int(nullable: false, identity: true),
                        RenderId = c.Int(nullable: false),
                        OffSet = c.Long(nullable: false),
                        CacheIndexId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvalidValueId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InvalidValues");
        }
    }
}
