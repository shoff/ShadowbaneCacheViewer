namespace CacheViewer.Domain.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Factories;
    using Geometry;
    using Models;

    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public static class BinaryReaderExtensions
    {
        public static bool CanRead(this BinaryReader reader, uint bytesToRead)
        {
            return reader.BaseStream.Position + bytesToRead <= reader.BaseStream.Length;
        }

        public static Vector3 ReadToVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector2 ReadToVector2(this BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        public static DateTime? ReadToDate(this BinaryReader reader)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long seconds = reader.ReadInt32();
            DateTime? dateTime = null;
            if (seconds > 0)
            {
                dateTime = epoch + TimeSpan.FromSeconds(seconds);
            }

            return dateTime;
        }
        
        public static string ReadAsciiString(this BinaryReader reader, uint counter)
        {
            var byteArray = reader.ReadBytes((int) counter * 2);

            var enc = new ASCIIEncoding();

            var tvTemp = enc.GetString(byteArray);

            //remove all the \0 and trim the string
            return tvTemp.Replace("\0", "").Trim();
        }

        public static StructureValidationResult ValidateCobjectIdType4(this BinaryReader reader, int identity)
        {
            var result = new StructureValidationResult
            {
                InitialOffset = reader.BaseStream.Position,
                Id = reader.CanRead(12) ? reader.ReadInt32() : 0,
                BytesLeftInObject = (int)(reader.BaseStream.Length - reader.BaseStream.Position),
                IsValid = false,
                NullTerminatorRead = false,
                NullTerminator = StructureValidationResult.NOT_READ,
                Range = 0
            };

            if (result.Id <= 0 || identity > 999 && result.Id < 1000)
            {
                return result;
            }

            // range check to make sure that the id and identity aren't 
            // too far apart as they should be relatively close to each other.
            result.Range = result.Id > identity ?
                Math.Abs(identity - result.Id) :
                Math.Abs(result.Id - identity);

            if (Math.Abs(result.Range) > 5000)
            {
                return result;
            }

            if (!RenderInformationFactory.Instance.IsValidRenderId(result.Id))
            {
                return result;
            }

            result.IsValidRenderId = true;

            result.FirstInt = reader.ReadUInt32();
            result.SecondInt = reader.ReadUInt32();

            if (reader.CanRead(1))
            {
                result.NullTerminator = reader.ReadByte();
                result.NullTerminatorRead = true;
            }

            result.EndingOffset = reader.BaseStream.Position;
            result.BytesLeftInObject = (int)(reader.BaseStream.Length - reader.BaseStream.Position);
            result.IsValid = result.PaddingIsValid;
            return result;
        }

        public static int StructureRenderCount(this BinaryReader reader, StructureValidationResult result)
        {
            var distance = result.NullTerminatorRead ? 26 : 25;
            reader.BaseStream.Position -= distance;
            var count = reader.ReadInt32();
            reader.BaseStream.Position += distance - 4;
            return count;
        }


        public static int MobileRenderCount(this BinaryReader reader, StructureValidationResult result)
        {
            var distance = 8;
            reader.BaseStream.Position -= distance;
            var count = reader.ReadInt32();
            reader.BaseStream.Position += distance - 4;
            return count;
        }
    }
}