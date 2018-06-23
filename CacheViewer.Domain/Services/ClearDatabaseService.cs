namespace CacheViewer.Domain.Services
{
    using System.Threading.Tasks;
    using Data;

    public class ClearDatabaseService
    {
        public async Task ClearDataAsync()
        {
            using (var context = new DataContext())
            {
                context.ExecuteCommand("delete from dbo.CacheObjectEntities");
                context.ExecuteCommand("delete from dbo.RenderEntities");
                context.ExecuteCommand("delete from dbo.MeshEntities");
                context.ExecuteCommand("delete from dbo.TextureEntities");
                context.ExecuteCommand("delete from dbo.RenderAndOffsets");
                context.ExecuteCommand("delete from dbo.InvalidValues");
                await context.SaveChangesAsync();
            }
        }
    }
}