namespace CacheViewer.Domain.Data
{
    using System.Data.Entity;
    using Entities;
    using Models;

    public partial class DataContext
    {
        private DbSet<CacheIndexEntity> cacheIndexes;
        private DbSet<CacheObjectEntity> cacheObjectEntities;
        private DbSet<Log> logs;
        private DbSet<MeshEntity> meshEntities;
        private DbSet<MobileEntity> mobiles;
        private DbSet<MotionEntity> motions;
        private DbSet<RenderAndOffset> renderAndOffset;
        private DbSet<RenderEntity> renderEntities;
        private DbSet<SkeletonEntity> skeletons;
        private DbSet<Texture> textures;

        public DbSet<CacheIndexEntity> CacheIndexes =>
            this.cacheIndexes ?? (this.cacheIndexes = this.Set<CacheIndexEntity>());

        public DbSet<SkeletonEntity> SkeletonEntities =>
            this.skeletons ?? (this.skeletons = this.Set<SkeletonEntity>());

        public DbSet<MotionEntity> MotionEntities => this.motions ?? (this.motions = this.Set<MotionEntity>());

        public DbSet<MobileEntity> MobileEntities => this.mobiles ?? (this.mobiles = this.Set<MobileEntity>());

        public DbSet<Log> Logs => this.logs ?? (this.logs = this.Set<Log>());

        public DbSet<CacheObjectEntity> CacheObjectEntities => this.cacheObjectEntities ??
            (this.cacheObjectEntities = this.SetEntity<CacheObjectEntity>());

        public DbSet<RenderEntity> RenderEntities =>
            this.renderEntities ?? (this.renderEntities = this.SetEntity<RenderEntity>());

        public DbSet<MeshEntity> MeshEntities =>
            this.meshEntities ?? (this.meshEntities = this.SetEntity<MeshEntity>());

        public DbSet<Texture> Textures => this.textures ?? (this.textures = this.SetEntity<Texture>());

        public DbSet<RenderAndOffset> RenderAndOffsets =>
            this.renderAndOffset ?? (this.renderAndOffset = this.SetEntity<RenderAndOffset>());
    }
}