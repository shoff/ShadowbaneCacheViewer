namespace CacheViewer.Domain.Services.Database
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Extensions;
    using NLog;

    public class RenderOffsetEventArgs : EventArgs
    {
        public int Count { get; set; }
    }


    public class AssociateRenderOffsetsDatabaseService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public event EventHandler<RenderOffsetEventArgs> RenderOffsetsSaved;

        public async Task AssociateRenderAndOffsets()
        {
            using (var context = new SbCacheViewerContext())
            {
                int total = 0;
                int count = 0;

                var renderOffsets = (from r in context.RenderAndOffsets select r).ToList();

                var cacheObjects = (from c in context.CacheObjectEntities select c).ToList();

                // fuck am I doing lol I need to go to bed
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