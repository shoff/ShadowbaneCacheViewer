namespace CacheViewer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingChildIdToRenderChildren : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RenderChildren", "ChildId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RenderChildren", "ChildId");
        }
    }
}
