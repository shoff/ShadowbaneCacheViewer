namespace Arcane.Cache.Models;

public class SBMeshSet
{
    public uint PolymeshId { get; set; }
    public bool PolymeshDecal { get; set; }
    public bool PolymeshDoubleSided { get; set; }
    public SBMesh? Mesh { get; set; }

}