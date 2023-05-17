namespace Arcane.Cache.Models;

using UnityEngine;

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