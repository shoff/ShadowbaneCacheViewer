namespace CacheViewer.Domain.Services.Database
{
    using System.Threading.Tasks;
    using Data;

    public class ClearDatabaseService
    {
        public async Task ClearDataAsync()
        {
            using (var context = new DataContext())
            {
                context.ExecuteCommand("delete from dbo.RenderAndOffsets");
                context.ExecuteCommand("delete from dbo.CacheObjectEntities");
                context.ExecuteCommand("delete from dbo.RenderEntities");
                context.ExecuteCommand("delete from dbo.MeshEntities");
                context.ExecuteCommand("delete from dbo.TextureEntities");
                context.ExecuteCommand("delete from dbo.InvalidValues");
                context.ExecuteCommand("DBCC CHECKIDENT ('RenderAndOffsets', RESEED, 1)");
                context.ExecuteCommand("DBCC CHECKIDENT ('CacheObjectEntities', RESEED, 1)");
                context.ExecuteCommand("DBCC CHECKIDENT ('RenderEntities', RESEED, 1)");
                context.ExecuteCommand("DBCC CHECKIDENT ('MeshEntities', RESEED, 1)");
                context.ExecuteCommand("DBCC CHECKIDENT ('TextureEntities', RESEED, 1)");
                context.ExecuteCommand("DBCC CHECKIDENT ('InvalidValues', RESEED, 1)");
                await context.SaveChangesAsync();
            }
        }
    }
}