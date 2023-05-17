namespace Arcane.Cache.Models;

public class SBTemplateMesh
{
    public ICollection<SBMeshSet> MeshSet { get; set; } = new List<SBMeshSet>();
}