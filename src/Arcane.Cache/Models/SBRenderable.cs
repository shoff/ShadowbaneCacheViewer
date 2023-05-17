namespace Arcane.Cache.Models;

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