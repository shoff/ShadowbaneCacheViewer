namespace Shadowbane.Models
{
    using System;
    using Cache;
    using Cache.IO;
    using Cache.IO.Models;

    public class Structure : ModelObject
    {
        private const int ValidRange = 5000;

        public Structure(CacheIndex cacheIndex, ObjectType flag,
            string name, uint offset, ArraySegment<byte> data, uint innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
        }

        public int BytesOfZeroData { get; private set; }
        public bool BValue1 { get; private set; }
        public bool BValue2 { get; private set; }
        public bool BValue3 { get; private set; }
        public bool BWalkData { get; private set; }
        public uint InventoryTextureId { get; private set; }
        public uint IUnk { get; private set; }
        public uint MapTex { get; private set; }
        public uint NumberOfMeshes { get; private set; }
        public StructureValidationResult ValidationResult { get; private set; }

        private bool ValidRenderId(uint id)
        {
            if (id == 0)
            {
                return false;
            }

            if (ArchiveLoader.RenderArchive[id] == null)
            {
                return false;
            }

            var range = id > this.CacheIndex.identity ?
                Math.Abs(id - this.CacheIndex.identity) :
                Math.Abs(this.CacheIndex.identity - id);

            return range <= ValidRange;
        }

        public override void Parse()
        {
            using var reader = this.Data.CreateBinaryReaderUtf32(0);
            _ = reader.ReadInt32();
            _ = (ObjectType)reader.ReadInt32();
            var nameLength = reader.ReadUInt32();
            var name = reader.ReadAsciiString(nameLength);

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.Name = name;
            }

            // should be 12 since the trailing byte may not be there if we are at the end of the file
            // once we have found a valid id then we will jump forward more than a byte at a time.

            while (reader.CanRead(12) && this.RenderIds.Count == 0)
            {
                this.ValidationResult = reader.ValidateCobjectIdType4(this.CacheIndex.identity);

                if (this.ValidationResult.IsValid)
                {
                    this.RenderIds.Add(this.ValidationResult.Id);
                    this.RenderCount = (uint) reader.StructureRenderCount(this.ValidationResult);
                }
                else
                {
                    reader.BaseStream.Position = (this.ValidationResult.InitialOffset + 1);
                }
            }

            while (reader.CanRead(12) && this.ValidationResult.IsValid &&
                this.ValidationResult.NullTerminator == 0)
            {
                this.ValidationResult = reader.ValidateCobjectIdType4(this.CacheIndex.identity);
                if (this.ValidationResult.IsValid)
                {
                    this.RenderIds.Add(this.ValidationResult.Id);
                }
            }
        }

        public void ParseAndAssemble()
        {
            this.Parse();
            foreach (var render in this.RenderIds)
            {
                // TODO this doesn't handle duplicate ids
                var asset = ArchiveLoader.RenderArchive[render];
                var renderInformation = RenderableObjectBuilder.Create(asset.CacheIndex);
                this.Renders.Add(renderInformation);
            }
        }

        public class StructureValidationResult
        {
            public const int NOT_READ = -1;
            public uint Id { get; set; }
            public long InitialOffset { get; set; }
            public long EndingOffset { get; set; }
            public bool NullTerminatorRead { get; set; }
            public int NullTerminator { get; set; }
            public uint FirstInt { get; set; }
            public uint SecondInt { get; set; }
            public uint ThirdInt { get; set; }
            public int Range { get; set; }
            public bool IsValidRenderId { get; set; }
            public int BytesLeftInObject { get; set; }
            public bool IsValid { get; set; }
            public bool PaddingIsValid =>
                (FirstInt < 51) &&
                (SecondInt == 0 || SecondInt == 1 || SecondInt == 2 || SecondInt == 3 || SecondInt == 4);

        }
    }
}