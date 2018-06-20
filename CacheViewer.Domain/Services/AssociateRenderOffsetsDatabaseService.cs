namespace CacheViewer.Domain.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Extensions;

    public class RenderOffsetEventArgs : EventArgs
    {
        public int Count { get; set; }
    }


    public class AssociateRenderOffsetsDatabaseService
    {
        public event EventHandler<RenderOffsetEventArgs> RenderOffsetsSaved;


        public async Task SaveToDatabase()
        {
            using (var context = new DataContext())
            {
                int total = 0;
                int count = 0;

                var renderOffsets = (from r in context.RenderAndOffsets select r).ToList();
                var cacheObjects = (from c in context.CacheObjectEntities select c).ToList();

                foreach (var ro in renderOffsets)
                {
                    count++;
                    total++;
                    var co = cacheObjects.Find(x => x.CacheIndexIdentity == ro.CacheIndexId);
                    co?.RenderAndOffsets.Add(ro);

                    if (count > 10)
                    {
                        count = 0;
                        var eventArgs = new RenderOffsetEventArgs
                        {
                            Count = total
                        };
                        this.RenderOffsetsSaved.Raise(this, eventArgs);
                        await context.SaveChangesAsync();
                    }
                }

                var eventArgs1 = new RenderOffsetEventArgs
                {
                    Count = total
                };
                this.RenderOffsetsSaved.Raise(this, eventArgs1);
                await context.SaveChangesAsync();
            }
        }
    }
}