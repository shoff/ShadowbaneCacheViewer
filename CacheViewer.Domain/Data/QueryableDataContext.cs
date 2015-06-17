using System.Data.Entity;
using CacheViewer.Domain.Data.Entities;
using CacheViewer.Domain.Models;

namespace CacheViewer.Domain.Data
{
    public partial class DataContext
    {
        private IDbSet<CacheObjectEntity> cacheObjectEntities;
        private IDbSet<RenderEntity> renderEntities;
        private IDbSet<MeshEntity> meshEntities;
        private IDbSet<Texture> textures;
        private IDbSet<RenderAndOffset> renderAndOffset;
        private IDbSet<Log> logs;
        private IDbSet<MobileEntity> mobiles;
        private IDbSet<MotionEntity> motions;
        private IDbSet<SkeletonEntity> skeletons;

        /// <summary>
        /// Gets the skeleton entities.
        /// </summary>
        /// <value>
        /// The skeleton entities.
        /// </value>
        public IDbSet<SkeletonEntity> SkeletonEntities
        {
            get { return this.skeletons ?? (this.skeletons = this.Set<SkeletonEntity>()); }
        }

        /// <summary>
        /// Gets the motion entities.
        /// </summary>
        /// <value>
        /// The motion entities.
        /// </value>
        public IDbSet<MotionEntity> MotionEntities
        {
            get { return this.motions ?? (this.motions = this.Set<MotionEntity>()); }
        }
        
        /// <summary>
        /// Gets the mobile entities.
        /// </summary>
        /// <value>
        /// The mobile entities.
        /// </value>
        public IDbSet<MobileEntity> MobileEntities
        {
            get { return this.mobiles ?? (this.mobiles = this.Set<MobileEntity>()); }
        }

        /// <summary>
        /// Gets the logs.
        /// </summary>
        /// <value>
        /// The logs.
        /// </value>
        public IDbSet<Log> Logs
        {
            get { return this.logs ?? (this.logs = this.Set<Log>()); }
        }
        
        /// <summary>
        /// Gets the cache object entities.
        /// </summary>
        /// <value>
        /// The cache object entities.
        /// </value>
        public IDbSet<CacheObjectEntity> CacheObjectEntities
        {
            get { return this.cacheObjectEntities ?? (this.cacheObjectEntities = this.SetEntity<CacheObjectEntity>()); }
        }

        /// <summary>
        /// Gets the render entities.
        /// </summary>
        /// <value>
        /// The render entities.
        /// </value>
        public IDbSet<RenderEntity> RenderEntities
        {
            get { return this.renderEntities ?? (this.renderEntities = this.SetEntity<RenderEntity>()); }
        }

        /// <summary>
        /// Gets the mesh entities.
        /// </summary>
        /// <value>
        /// The mesh entities.
        /// </value>
        public IDbSet<MeshEntity> MeshEntities
        {
            get { return this.meshEntities ?? (this.meshEntities = this.SetEntity<MeshEntity>()); }
        }

        /// <summary>
        /// Gets the vector2 entities.
        /// </summary>
        /// <value>
        /// The vector2 entities.
        /// </value>
        public IDbSet<Texture>Textures
        {
            get { return this.textures ?? (this.textures = this.SetEntity<Texture>()); }
        }

        /// <summary>
        /// Gets the render and offsets.
        /// </summary>
        /// <value>
        /// The render and offsets.
        /// </value>
        public IDbSet<RenderAndOffset> RenderAndOffsets
        {
            get { return this.renderAndOffset ?? (this.renderAndOffset = this.SetEntity<RenderAndOffset>()); }
        }

    }
}