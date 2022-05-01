namespace Shadowbane.Cache.Exporter.File;

using System;
using System.IO;
using System.Threading.Tasks;

public class FileWriter
{
    public static FileWriter Writer { get; } = new();

    public async Task WriteAsync(ReadOnlyMemory<byte> data, string folder, string filename)
    {
        if (data.Length == 0)
        {
            throw new ArgumentException("Cannot write an empty collection", nameof(data));
        }

        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(filename));
        }

        if (string.IsNullOrWhiteSpace(folder))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(folder));
        }

        this.EnsureDirectory(folder);
        await Task.Run(() =>
        {
            FileStream fs = null;

            try
            {
                var file = $"{folder}\\{filename}";
                fs = new FileStream($"{folder}\\{filename}", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                using var writer = new BinaryWriter(fs);
                writer.Write(data.Span);
            }
            finally
            {
                fs?.Dispose();
            }
        });
    }

    public void Write(ReadOnlySpan<byte> data, string folder, string filename)
    {
        if (data.Length == 0)
        {
            throw new ArgumentException("Cannot write an empty collection", nameof(data));
        }

        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(filename));
        }

        if (string.IsNullOrWhiteSpace(folder))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(folder));
        }

        this.EnsureDirectory(folder);
        using var fs = new FileStream($"{folder}\\{filename}", FileMode.Create, FileAccess.Write);
        using var writer = new BinaryWriter(fs);
        writer.Write(data);
    }

    private void EnsureDirectory(string directory)
    {
        // TODO move to it's own object, this doesn't belong here.
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}