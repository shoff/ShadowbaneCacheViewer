namespace Shadowbane.Models;

using System;
using System.Numerics;
using Cache;

public abstract record AnimationRecord(uint Identity, ObjectType Flag, string? Name, uint CursorOffset,
    ReadOnlyMemory<byte> Data) : ModelRecord(Identity, Flag, Name, CursorOffset, Data)
{
    public ICacheRecord? Skeleton { get; set; }
    public Vector3 Scale {get;set;}
}