namespace CacheViewer.Domain.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class Messages : Dictionary<long, Message>
    {
        private long lastInsert;

        private Messages()
        {
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static Messages Instance { get; } = new Messages();

        /// <summary>
        ///     Adds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public async Task<long> AddAsync(string message)
        {
            var id = Interlocked.Increment(ref this.lastInsert);
            await Task.Run(() => this.Add(id, new Message {Text = message}));
            return id;
        }

        /// <summary>
        ///     Adds the specified message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public async Task<long> AddAsync(Exception exception)
        {
            var id = Interlocked.Increment(ref this.lastInsert);
            await Task.Run(() => this.Add(id, new Message {Exception = exception}));
            return id;
        }


        /// <summary>
        ///     Adds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public long Add(string message)
        {
            var id = Interlocked.Increment(ref this.lastInsert);
            this.Add(id, new Message {Text = message});
            return id;
        }

        /// <summary>
        ///     Adds the specified message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public long Add(Exception exception)
        {
            var id = Interlocked.Increment(ref this.lastInsert);
            this.Add(id, new Message {Exception = exception});
            return id;
        }
    }

    public class Message
    {
        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        /// <value>
        ///     The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        ///     Gets or sets the exception.
        /// </summary>
        /// <value>
        ///     The exception.
        /// </value>
        public Exception Exception { get; set; }
    }
}