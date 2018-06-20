namespace CacheViewer.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CorrectingRenderData : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.RenderEntities", "HasTexture", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            this.DropColumn("dbo.RenderEntities", "HasTexture");
        }
    }
}
