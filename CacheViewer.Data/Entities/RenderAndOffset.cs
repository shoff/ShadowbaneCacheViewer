namespace CacheViewer.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class RenderAndOffset
    {
        [Key]
        public int RenderAndOffsetId { get; set; }
        public int RenderId { get; set; }
        public long OffSet { get; set; }
        public int CacheIndexId { get; set; }
    }


    public class InvalidValue
    {
        [Key]
        public int InvalidValueId { get; set; }
        public int RenderId { get; set; }
        public long OffSet { get; set; }
        public int CacheIndexId { get; set; }
    }

}