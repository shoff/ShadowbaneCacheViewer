namespace Shadowbane.Models;

using System.IO;

public class Sound
{
    public Sound(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
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
    public byte[] Buffer { get; }
    public int Frequency { get; }
    public int Bitrate { get; }
    public int SoundDataLength { get; }
    public int NumberOfChannels { get; }
}