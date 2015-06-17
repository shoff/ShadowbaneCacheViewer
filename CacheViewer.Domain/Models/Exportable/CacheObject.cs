
using System;
using CacheViewer.Domain.Archive;

namespace CacheViewer.Domain.Models.Exportable
{
    /// <summary>
    /// </summary>
    public abstract class CacheObject : ICacheObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheObject"/> class.
        /// </summary>
        /// <param name="cacheIndex">Index of the cache.</param>
        /// <param name="flag">The flag.</param>
        /// <param name="name">The name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="data">The data.</param>
        /// <param name="innerOffset">The inner offset.</param>
        protected CacheObject(CacheIndex cacheIndex, ObjectType flag, string name, 
            int offset, ArraySegment<byte> data, int innerOffset)
        {
            this.CacheIndex = cacheIndex;
            this.Flag = flag;
            this.Name = name;
            this.CursorOffset = offset;
            this.Data = data;
            this.InnerOffset = innerOffset;
        }

        /// <summary>
        /// Gets the index of the cache.
        /// </summary>
        /// <value>
        /// The index of the cache.
        /// </value>
        public CacheIndex CacheIndex { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
       
        /// <summary>
        /// Gets the render identifier.
        /// </summary>
        /// <value>
        /// The render identifier.
        /// </value>
        public uint RenderId { get; set; }

        /// <summary>
        /// Gets or sets the render.
        /// </summary>
        /// <value>
        /// The render.
        /// </value>
        public RenderInformation Render { get; set; }
        
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets the flag.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        public ObjectType Flag { get; set; }
        
        /// <summary>
        /// WTF is the CursorOffset?
        /// </summary>
        /// <value>
        /// The cursor offset.
        /// </value>
        public int CursorOffset { get; set; }
        
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public ArraySegment<byte> Data { get; set; }

        /// <summary>
        /// Gets the inner offset.
        /// </summary>
        /// <value>
        /// The inner offset.
        /// </value>
        public int InnerOffset { get; set; }

        /// <summary>
        /// Gets or sets the un parsed bytes.
        /// </summary>
        /// <value>
        /// The un parsed bytes.
        /// </value>
        public int UnParsedBytes { get; set; }

        /// <summary>
        /// Parses the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public abstract void Parse(ArraySegment<byte> data);

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has the following meanings: Value Meaning Less than zero 
        /// This object is less than the <paramref name="other" /> parameter.Zero This
        /// object is equal to <paramref name="other" />. Greater than zero This object 
        /// is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo(ICacheObject other)
        {
            if ((other == null)||(this.Flag > other.Flag))
            {
                return 1;
            }
            if (other.Flag == this.Flag)
            {
                return string.Compare(this.Name, other.Name, System.StringComparison.Ordinal);
            }
            return -1;
        }
    }
}