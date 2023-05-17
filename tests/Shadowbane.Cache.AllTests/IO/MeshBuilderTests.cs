﻿namespace Shadowbane.Cache.AllTests.IO;

using Cache.IO;
using Shadowbane.Cache.CacheTypes;
using Xunit;

public class MeshBuilderTests : CacheLoaderBaseTest
{
    private readonly MeshCache meshCache;
    private readonly MeshBuilder meshBuilder;


    public MeshBuilderTests()
    {
        this.meshCache = ArchiveLoader.MeshArchive;
        this.meshBuilder = new MeshBuilder();
    }

    [Fact]
    public void Meshes_100_6120_Are_Valid()
    {
        for (uint i = 0; i < 200; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }

    [Fact]
    public void Meshes_6136_6584_Are_Valid()
    {
        for (uint i = 201; i < 400; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }

    [Fact]
    public void Meshes_6586_17521_Are_Valid()
    {
        for (uint i = 401; i < 600; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }

    [Fact]
    public void Meshes_17541_19334_Are_Valid()
    {
        for (uint i = 601; i < 800; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }
        
    [Fact]
    public void Meshes_19338_20685_Are_Valid()
    {
        for (uint i = 801; i < 1200; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }
        
    [Fact]
    public void Meshes_20690_25112_Are_Valid()
    {
        for (uint i = 1201; i < 2000; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }
        
    [Fact]
    public void Meshes_25122_452108_Are_Valid()
    {
        for (uint i = 2001; i < 8000; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }

    [Fact]
    public void Meshes_452118_702724_Are_Valid()
    {
        for (uint i = 8001; i < 16000; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }

    [Fact]
    public void Meshes_702726_77000103_Are_Valid()
    {
        for (uint i = 16001; i < 24386; i++)
        {
            var identity = this.meshCache.IdentityAt(i);
            var asset = this.meshCache[identity];
            if (asset != null)
            {
                _ = this.meshBuilder.SaveRawMeshData(asset.Asset, identity);
            }
        }
    }
}