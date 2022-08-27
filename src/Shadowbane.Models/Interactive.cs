namespace Shadowbane.Models;

using System;
using System.Collections.Generic;
using Cache;

public record Interactive : AnimationObject
{
    // ReSharper disable once InconsistentNaming
    protected readonly List<CollisionInfo> collisionInfo = new();
    // ReSharper disable once InconsistentNaming
    public readonly List<uint> renderIds = new();
    private bool bWalkData;

    public Interactive(uint identity, string? name, uint offset, ReadOnlyMemory<byte> data,
        uint innerOffset)
        : base(identity, ObjectType.Interactive, name, offset, data, innerOffset)
    {
    }
    
    public override ICacheObject Parse()
    {
        // TODO need to figure out the below commented out code.
        using var reader = this.Data.CreateBinaryReaderUtf32();
        _ = reader.ReadInt32();
        _ = (ObjectType)reader.ReadInt32();
        var nameLength = reader.ReadUInt32();
        this.Name = reader.AsciiString(nameLength) ?? string.Empty;

  
        // should be 12 since the trailing byte may not be there if we are at the end of the file
        // once we have found a valid id then we will jump forward more than a byte at a time.

        //while (reader.CanRead(12) && this.RenderIds.Count == 0)
        //{
        //    this.ValidationResult = reader.ValidateCobjectIdType4(this.CacheIndex.identity);

        //    if (this.ValidationResult.IsValid)
        //    {
        //        this.RenderIds.Add(this.ValidationResult.Id);
        //        this.RenderCount = (uint) reader.StructureRenderCount(this.ValidationResult);
        //    }
        //    else
        //    {
        //        reader.BaseStream.Position = (this.ValidationResult.InitialOffset + 1);
        //    }
        //}

        //while (reader.CanRead(12) && this.ValidationResult.IsValid &&
        //    this.ValidationResult.NullTerminator == 0)
        //{
        //    this.ValidationResult = reader.ValidateCobjectIdType4(this.CacheIndex.identity);
        //    if (this.ValidationResult.IsValid)
        //    {
        //        this.RenderIds.Add(this.ValidationResult.Id);
        //    }
        //}
        return this;
    }


    //public void ParseAndAssemble()
    //{
    //    this.Parse();
    //    foreach (var render in this.RenderIds)
    //    {
    //        var asset = ArchiveLoader.RenderArchive[render];
    //        var renderInformation = RenderableObjectBuilder.Create(asset.CacheIndex);
    //        this.Renders.Add(renderInformation);
    //    }
    //}
}