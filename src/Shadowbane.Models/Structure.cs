namespace Shadowbane.Models;

using System;
using System.IO;
using Cache;

public class Structure : ModelObject
{
    private const int VALID_RANGE = 5000;

    public Structure(uint identity, string name, uint offset, ReadOnlyMemory<byte> data, uint innerOffset)
        : base(identity, ObjectType.Structure, name, offset, data, innerOffset)
    {
    }

    public int BytesOfZeroData { get; set; }
    public bool BValue1 { get; set; }
    public bool BValue2 { get; set; }
    public bool BValue3 { get; set; }
    public bool BWalkData { get; set; }
    public uint InventoryTextureId { get; set; }
    public uint IUnk { get; set; }
    public uint MapTex { get; set; }
    public uint NumberOfMeshes { get; set; }
    public StructureValidationResult ValidationResult { get; set; }


    public override ICacheObject Parse()
    {
        using var reader = this.Data.CreateBinaryReaderUtf32(this.CursorOffset);
        // should be 12 since the trailing byte may not be there if we are at the end of the file
        // once we have found a valid id then we will jump forward more than a byte at a time.

        while (reader.CanRead(12) && this.RenderIds.Count == 0)
        {
            this.ValidationResult = ValidateCobjectIdType4(reader, this.Identity);

            if (this.ValidationResult.IsValid)
            {
                this.RenderIds.Add(this.ValidationResult.Id);
                this.RenderCount = (uint)StructureRenderCount(reader, this.ValidationResult);
            }
            else
            {
                reader.BaseStream.Position = (this.ValidationResult.InitialOffset + 1);
            }
        }

        while (reader.CanRead(12) && this.ValidationResult.IsValid &&
               this.ValidationResult.NullTerminator == 0)
        {
            this.ValidationResult = ValidateCobjectIdType4(reader, this.Identity);
            if (this.ValidationResult.IsValid)
            {
                this.RenderIds.Add(this.ValidationResult.Id);
            }
        }
        return this;
    }

    private int StructureRenderCount(BinaryReader reader, StructureValidationResult result)
    {
        var distance = result.NullTerminatorRead ? 26 : 25;
        reader.BaseStream.Position -= distance;
        var count = reader.ReadInt32();
        reader.BaseStream.Position += distance - 4;
        return count;
    }

    private StructureValidationResult ValidateCobjectIdType4(BinaryReader reader, uint identity)
    {
        var result = new StructureValidationResult
        {
            InitialOffset = reader.BaseStream.Position,
            Id = reader.SafeReadUInt32(),
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
        result.Range = (int)(result.Id > identity ?
            Math.Abs(identity - result.Id) :
            Math.Abs(result.Id - identity));

        if (Math.Abs(result.Range) > VALID_RANGE)
        {
            return result;
        }

        // TODO a registry of render ids at app start?
        //if (!RenderInformationFactory.Instance.IsValidRenderId(result.Id))
        //{
        //    return result;
        //}

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