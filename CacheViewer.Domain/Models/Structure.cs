// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace CacheViewer.Domain.Models
{
    using System;
    using Archive;
    using Data;
    using Exportable;
    using Extensions;
    using Factories;
    using NLog;

    public class Structure : ModelObject
    {
        private const int ValidRange = 5000;
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private readonly CollisionInfo collisionData = new CollisionInfo();
        //private readonly CollisionInfo collisionData1 = new CollisionInfo();
        //protected readonly List<CollisionInfo> collisionInfo = new List<CollisionInfo>();
        //private readonly List<RenderEntity> renderEntities = new List<RenderEntity>();
        //public readonly List<uint> renderIds = new List<uint>();

        public Structure(CacheIndex cacheIndex, ObjectType flag,
            string name, int offset, ArraySegment<byte> data, int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
            logger?.Debug($"Creating new Structure with id {cacheIndex.Identity}");
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
    
        private bool ValidRenderId(int id)
        {
            if (id == 0)
            {
                return false;
            }

            if (!RenderInformationFactory.Instance.IsValidRenderId(id))
            {
                return false;
            }

            var range = id > this.CacheIndex.Identity ?
                Math.Abs(id - this.CacheIndex.Identity) :
                Math.Abs(this.CacheIndex.Identity - id);

            return range <= ValidRange;
        }

        public override void Parse()
        {
            using (var reader = this.Data.CreateBinaryReaderUtf32())
            {
                var tnlc = reader.ReadInt32();
                var flag = (ObjectType)reader.ReadInt32();
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
                    ValidationResult = reader.ValidateCobjectIdType4(this.CacheIndex.Identity);

                    if (ValidationResult.IsValid)
                    {
                        this.RenderIds.Add(ValidationResult.Id);
                        this.RenderCount = reader.StructureRenderCount(ValidationResult);
                    }
                    else
                    {
                        reader.BaseStream.Position = (ValidationResult.InitialOffset + 1);
                    }
                }

                while (reader.CanRead(12) && ValidationResult.IsValid &&
                    ValidationResult.NullTerminator == 0)
                {
                    ValidationResult = reader.ValidateCobjectIdType4(this.CacheIndex.Identity);
                    if (ValidationResult.IsValid)
                    {
                        this.RenderIds.Add(ValidationResult.Id);
                    }
                }
            }
        }

        public void ParseAndAssemble()
        {
            this.Parse();
            foreach (var render in this.RenderIds)
            {
                // TODO this doesn't handle duplicate ids
                var renderInformation = this.renderInformationFactory.Create(render, 0, true);
                this.Renders.Add(renderInformation);
            }
        }
    }
}