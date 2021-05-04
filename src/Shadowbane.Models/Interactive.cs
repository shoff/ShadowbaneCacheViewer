namespace Shadowbane.Models
{
    using System;
    using System.Collections.Generic;
    using Cache;
    using Cache.IO;
    using Cache.IO.Models;

    public class Interactive : AnimationObject
    {
        // ReSharper disable once InconsistentNaming
        protected readonly List<CollisionInfo> collisionInfo = new List<CollisionInfo>();
        // ReSharper disable once InconsistentNaming
        public readonly List<uint> renderIds = new List<uint>();
        private bool bWalkData;

        public Structure.StructureValidationResult ValidationResult { get; private set; }

        public Interactive(CacheIndex cacheIndex, ObjectType flag, string name, uint offset, ReadOnlyMemory<byte> data,
            uint innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
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
                var asset = ArchiveLoader.RenderArchive[render];
                var renderInformation = RenderableObjectBuilder.Create(asset.CacheIndex);
                this.Renders.Add(renderInformation);
            }
        }
    }
}