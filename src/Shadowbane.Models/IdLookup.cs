namespace Shadowbane.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChaosMonkey.Guards;
using Shadowbane.Cache.CacheTypes;

public static class IdLookup
{
    static IdLookup()
    {
        RenderCache renderArchive = new();
        ValidObjectIds = renderArchive.CacheIndices.Select(cacheIndex => cacheIndex.identity).ToArray();
        HighestObjectId = ValidObjectIds.Max();

        MeshCache meshCache = new();
        ValidMeshIds = meshCache.CacheIndices.Select(cacheIndex => cacheIndex.identity).ToArray();
        HighestMeshId = ValidMeshIds.Max();
    }

    public static uint HighestMeshId { get; set; }

    public static uint[] ValidMeshIds { get; set; }

    public static uint[] ValidObjectIds { get; }

    public static uint HighestObjectId { get; }

    public static bool IsValidObjectId(uint identity)
    {
        return Array.IndexOf(ValidObjectIds, identity) > -1;
    }
    
    public static bool IsValidMeshId(uint identity)
    {
        return Array.IndexOf(ValidMeshIds, identity) > -1;
    }

}