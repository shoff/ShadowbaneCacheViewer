namespace Shadowbane.Cache.IO.Models
{
    public class Index
    {
        public Index(int position, int textureCoordinate, int normal)
        {
            this.Position = position;
            this.TextureCoordinate = textureCoordinate;
            this.Normal = normal;
        }

        public int Position { get; set; }
        public int Normal { get; set; }
        public int TextureCoordinate { get; set; }
    }
}