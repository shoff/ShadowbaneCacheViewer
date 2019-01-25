namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingErrorFlagToRenderEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RenderEntities", "InvalidData", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RenderEntities", "InvalidData");
        }
    }
}
