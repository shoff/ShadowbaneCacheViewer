// ReSharper disable LocalizableElement
namespace CacheViewer.Domain.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class FileWriter
    {
        public static FileWriter Writer { get; } = new FileWriter();

        public async Task WriteAsync(ArraySegment<byte> data, string path, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(filename));
            }

            await this.WriteAsync(data, $"{path}\\{filename}");
        }

        public async Task WriteAsync(ArraySegment<byte> data, string path)
        {
            if (data.Count == 0)
            {
                throw new ArgumentException(DomainMessages.Cannot_Be_An_Empty_Collection, nameof(data));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            await Task.Run(() =>
            {
                FileStream fs = null;

                try
                {
                    fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                    using (var writer = new BinaryWriter(fs))
                    {
                        if (data.Array != null)
                        {
                            writer.Write(data.Array);
                        }
                    }
                }
                finally
                {
                    fs?.Dispose();
                }
            });
        }
        
        public void Write(ArraySegment<byte> data, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(fs))
                {
                    writer.Write(data.Array);
                }
            }
        }
    }
}