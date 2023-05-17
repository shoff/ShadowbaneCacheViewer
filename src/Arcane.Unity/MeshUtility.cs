using Arcane.Cache.Json.Mesh;

namespace Arcane.Unity;


using Mesh = Mesh;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

public static class MeshUtility
{
    public static UnityEngine.Mesh CreateUnityMesh(Mesh sourceMesh)
    {
        var unityMesh = new UnityEngine.Mesh
        {
            name = sourceMesh.MeshName
        };

        Vector3[] vertices = new Vector3[sourceMesh.MeshVertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vertex = sourceMesh.MeshVertices[i];
            vertices[i] = new Vector3(vertex[0], vertex[1], vertex[2]);
        }

        var normals = new Vector3[sourceMesh.MeshNormals.Length];
        for (int i = 0; i < normals.Length; i++)
        {
            var normal = sourceMesh.MeshNormals[i];
            normals[i] = new Vector3(normal[0], normal[1], normal[2]);
        }

        var uvs = new Vector2[sourceMesh.MeshUv.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            var uv = sourceMesh.MeshUv[i];
            uvs[i] = new Vector2(uv[0], uv[1]);
        }

        unityMesh.vertices = vertices;
        unityMesh.normals = normals;
        unityMesh.uv = uvs;

        if(sourceMesh.MeshUseTangentBasis)
        {
            var tangents = new Vector4[sourceMesh.MeshTangentVertices.Length];
            for (int i = 0; i < tangents.Length; i++)
            {
                var tangent = sourceMesh.MeshTangentVertices[i];
                tangents[i] = new Vector4(tangent[0], tangent[1], tangent[2], tangent[3]);
            }
            unityMesh.tangents = tangents;
        }

        unityMesh.triangles = sourceMesh.MeshIndices;

        return unityMesh;
    }
}
