using System.Data.Entity;
using CacheViewer.Domain.Data.Entities;
using CacheViewer.Domain.Models;

namespace CacheViewer.Domain.Data
{
    public partial class DataContext
    {
        private DbSet<CacheObjectEntity> cacheObjectEntities;
        private DbSet<RenderEntity> renderEntities;
        private DbSet<MeshEntity> meshEntities;
        private DbSet<Texture> textures;
        private DbSet<RenderAndOffset> renderAndOffset;
        private DbSet<Log> logs;
        private DbSet<MobileEntity> mobiles;
        private DbSet<MotionEntity> motions;
        private DbSet<SkeletonEntity> skeletons;
        private DbSet<CacheIndexEntity> cacheIndexes;
        
        public DbSet<CacheIndexEntity> CacheIndexes
        {
            get { return this.cacheIndexes ?? (this.cacheIndexes = this.Set<CacheIndexEntity>()); }
        }
        
        public DbSet<SkeletonEntity> SkeletonEntities
        {
            get { return this.skeletons ?? (this.skeletons = this.Set<SkeletonEntity>()); }
        }
        
        public DbSet<MotionEntity> MotionEntities
        {
            get { return this.motions ?? (this.motions = this.Set<MotionEntity>()); }
        }
        
        public DbSet<MobileEntity> MobileEntities
        {
            get { return this.mobiles ?? (this.mobiles = this.Set<MobileEntity>()); }
        }
        
        public DbSet<Log> Logs
        {
            get { return this.logs ?? (this.logs = this.Set<Log>()); }
        }
        
        public DbSet<CacheObjectEntity> CacheObjectEntities
        {
            get { return this.cacheObjectEntities ?? (this.cacheObjectEntities = this.SetEntity<CacheObjectEntity>()); }
        }
        
        public DbSet<RenderEntity> RenderEntities
        {
            get { return this.renderEntities ?? (this.renderEntities = this.SetEntity<RenderEntity>()); }
        }
        
        public DbSet<MeshEntity> MeshEntities
        {
            get { return this.meshEntities ?? (this.meshEntities = this.SetEntity<MeshEntity>()); }
        }
        
        public DbSet<Texture>Textures
        {
            get { return this.textures ?? (this.textures = this.SetEntity<Texture>()); }
        }
        
        public DbSet<RenderAndOffset> RenderAndOffsets
        {
            get { return this.renderAndOffset ?? (this.renderAndOffset = this.SetEntity<RenderAndOffset>()); }
        }

    }
}