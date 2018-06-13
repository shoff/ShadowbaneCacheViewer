namespace CacheViewer.Domain.Utility
{
    using System.Collections.Generic;

    /// <summary>
    ///     Pools data buffers to prevent both frequent allocation and memory fragmentation
    ///     due to pinning in high volume scenarios.
    ///     See https://blogs.msdn.com/yunjin/archive/2004/01/27/63642.aspx
    /// </summary>
    public class BufferPool
    {
        private const int InitialPoolSize = 1024; // initial size of the pool
        private const int BufferSize = 1024; // size of the buffers
        private readonly Queue<byte[]> freeBuffers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BufferPool" /> class.
        /// </summary>
        private BufferPool()
        {
            this.freeBuffers = new Queue<byte[]>(InitialPoolSize);

            for (var i = 0; i < InitialPoolSize; i++)
            {
                this.freeBuffers.Enqueue(new byte[BufferSize]);
            }
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static BufferPool Instance => new BufferPool();

        /// <summary>
        ///     Checks the out.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public byte[] CheckOut(uint size)
        {
            //if (this.freeBuffers.Count > 0)
            //{
            if (this.freeBuffers.Count > 0)
            {
                return this.freeBuffers.Dequeue();
            }

            //}
            // instead of creating new buffer, 
            // blocking waiting or refusing request may be better

            return new byte[BufferSize];
        }

        /// <summary>
        ///     Check-ins the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public void CheckIn(byte[] buffer)
        {
            this.freeBuffers.Enqueue(buffer);
        }
    }
}