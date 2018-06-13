namespace CacheViewer.Domain.Models
{
    using System.IO;

    public class Sound
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Sound" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Sound(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(ms))
                {
                    this.SoundDataLength = reader.ReadInt32();

                    reader.BaseStream.Position = 4;
                    this.Bitrate = reader.ReadInt32();

                    this.NumberOfChannels = reader.ReadInt32();

                    reader.BaseStream.Position = 12;
                    this.Frequency = reader.ReadInt32();

                    this.Buffer = new byte[data.Length - 16];

                    for (var i = 0; i < this.Buffer.Length; i++)
                    {
                        this.Buffer[i] = reader.ReadByte();
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the buffer.
        /// </summary>
        /// <value>
        ///     The buffer.
        /// </value>
        public byte[] Buffer { get; }

        /// <summary>
        ///     Gets the frequency.
        /// </summary>
        /// <value>
        ///     The frequency.
        /// </value>
        public int Frequency { get; }

        /// <summary>
        ///     Gets the bitrate.
        /// </summary>
        /// <value>
        ///     The bitrate.
        /// </value>
        public int Bitrate { get; }

        /// <summary>
        ///     Gets the unk1.
        /// </summary>
        /// <value>
        ///     The unk1.
        /// </value>
        public int SoundDataLength { get; }

        /// <summary>
        ///     Gets the unk2.
        /// </summary>
        /// <value>
        ///     The unk2.
        /// </value>
        public int NumberOfChannels { get; }
    }
}