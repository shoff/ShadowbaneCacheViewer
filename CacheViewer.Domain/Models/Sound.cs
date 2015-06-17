using System.IO;

namespace CacheViewer.Domain.Models
{
    public class Sound
    {
        private readonly int soundDataLength;
        private readonly int numberOfChannels;
        private readonly int bitrate;
        private readonly int frequency;
        private readonly byte[] buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Sound(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    this.soundDataLength = reader.ReadInt32();

                    reader.BaseStream.Position = 4;
                    this.bitrate = reader.ReadInt32();

                    this.numberOfChannels = reader.ReadInt32();

                    reader.BaseStream.Position = 12;
                    this.frequency = reader.ReadInt32();

                    this.buffer = new byte[data.Length - 16];

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        this.buffer[i] = reader.ReadByte();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        /// <value>
        /// The buffer.
        /// </value>
        public byte[] Buffer
        {
            get { return this.buffer; }
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public int Frequency
        {
            get { return this.frequency; }
        }

        /// <summary>
        /// Gets the bitrate.
        /// </summary>
        /// <value>
        /// The bitrate.
        /// </value>
        public int Bitrate
        {
            get { return this.bitrate; }
        }

        /// <summary>
        /// Gets the unk1.
        /// </summary>
        /// <value>
        /// The unk1.
        /// </value>
        public int SoundDataLength
        {
            get { return this.soundDataLength; }
        }

        /// <summary>
        /// Gets the unk2.
        /// </summary>
        /// <value>
        /// The unk2.
        /// </value>
        public int NumberOfChannels
        {
            get { return this.numberOfChannels; }
        }
    }
}