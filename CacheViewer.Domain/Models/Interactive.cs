// ReSharper disable RedundantAssignment
// ReSharper disable UnusedVariable
namespace CacheViewer.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using Archive;
    using Data;
    using Exportable;
    using Extensions;
    using Factories;
    using NLog;

    public class Interactive : AnimationObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // ReSharper disable once InconsistentNaming
        protected readonly List<CollisionInfo> collisionInfo = new List<CollisionInfo>();
        // ReSharper disable once InconsistentNaming
        public readonly List<uint> renderIds = new List<uint>();
        private bool bWalkData;
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;

        public StructureValidationResult ValidationResult { get; private set; }

        public Interactive(CacheIndex cacheIndex, ObjectType flag, string name, int offset, ArraySegment<byte> data,
            int innerOffset)
            : base(cacheIndex, flag, name, offset, data, innerOffset)
        {
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

// ReSharper restore UnusedVariable
// ReSharper restore RedundantAssignment