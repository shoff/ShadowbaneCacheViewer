namespace Shadowbane.Models;

using System;
using System.Numerics;
using Cache;

public abstract record AnimationObject(uint Identity, ObjectType Flag, string? Name, uint CursorOffset,
    ReadOnlyMemory<byte> Data) : ModelObject(Identity, Flag, Name, CursorOffset, Data)
{
    public ICacheObject? Skeleton { get; set; }
    public Vector3 Scale {get;set;}
}