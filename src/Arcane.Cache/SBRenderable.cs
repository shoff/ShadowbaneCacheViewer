namespace Arcane.Cache;

using UnityEngine;

public class SBRenderable
{
    public uint RederableId { get; set; }
    public SBRenderTemplate? RenderTemplate { get; set; }
    public string RenderTargetBone { get; set; } = string.Empty;
    public float[] RenderScale { get; set; } = Array.Empty<float>();
    public int RenderHasLoc { get; set; }
    public float[] RenderLoc { get; set; } = Array.Empty<float>();
    public SBRenderable[] RenderChildren { get; set; } = Array.Empty<SBRenderable>();
    public bool RenderHasTextureSet { get; set; }
    public SBRenderTextureSet[] RenderTextureSets { get; set; } = Array.Empty<SBRenderTextureSet>();
    public bool RenderCollides { get; set; }
    public bool RenderCalculateBoundingBox { get; set; }
    public bool RenderNationCrest { get; set; }
    public bool RenderGuildCrest { get; set; }
    public bool RenderBumped { get; set; }
    public bool RenderVpActive { get; set; }
    public bool RenderHasLightEffects { get; set; }
}

public class SBRenderTemplate
{
    public bool TemplateObjectCanFade { get; set; }
    public string TemplateTracker { get; set; } = string.Empty;
    public bool TemplateIlluminated { get; set; }
    public float TemplateBoneLength { get; set; }
    public int TemplateClipMap { get; set; }
    public int TemplateLightTwoSide { get; set; }
    public int TemplateCullFace { get; set; }
    public int TemplateSpecularMap { get; set; }
    public float TemplateShininess { get; set; }
    public bool TemplateHasMesh { get; set; }
    public SBTemplateMesh? TemplateMesh { get; set; }
}

public class SBRenderTextureSet
{
    public string TextureType { get; set; } = string.Empty;
    public SBTextureData? TextureData { get; set; }
}

public class SBTemplateMesh
{
    public ICollection<SBMeshSet> MeshSet { get; set; } = new List<SBMeshSet>();
}

public class SBTextureData
{
    public uint TextureId { get; set; }
    public string TextureTransparent { get; set; } = string.Empty;
    public bool TextureCompress { get; set; }
    public bool TextureNormalMap { get; set; }
    public bool TextureDetailNormalMap { get; set; }
    public bool TextureCreateMipMaps { get; set; }
    public bool TextureWrap { get; set; }
    public string TexturePath { get; set; } = string.Empty;
}

public class SBMeshSet
{
    public uint PolymeshId { get; set; }
    public bool PolymeshDecal { get; set; }
    public bool PolymeshDoubleSided { get; set; }
    public SBMesh? Mesh { get; set; }

}

public class SBMesh
{
    public uint MeshId { get; set; }
    public float MeshDistance { get; set; }
    public float[] MeshStartPoint { get; set; } = Array.Empty<float>();
    public float[] MeshEndPoint { get; set; } = Array.Empty<float>();
    public float[] MeshRefPoint { get; set; } = Array.Empty<float>();
    public bool MeshUseFaceNormals { get; set; }
    public bool MeshUseTangentBasis { get; set; }
    public List<Vector3> MeshVertices { get; set; } = new();
    public List<Vector3> MeshNormals { get; set; } = new();
    public List<Vector2> MeshUv { get; set; } = new();
    public float[][] MeshTangentVertices { get; set; } = Array.Empty<float[]>(); // not used?
    public int[] MeshIndices { get; set; } = Array.Empty<int>();
    public List<(int, int, int[])> MeshExtraIndices { get; set; } = new();
}
