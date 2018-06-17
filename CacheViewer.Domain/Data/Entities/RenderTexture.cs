namespace CacheViewer.Domain.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class RenderTexture
    {
        [Key]
        public int RenderTextureId { get; set; }
        public int RenderId { get; set; }
        public long Offset { get; set; }
        public int TextureId { get; set; }
        
    }
}