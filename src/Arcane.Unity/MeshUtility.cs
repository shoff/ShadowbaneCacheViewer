namespace Arcane.Unity;

using UnityEngine;
using Cache.Models;
using ArcaneMesh = Arcane.Cache.Json.Mesh.Mesh;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

public class CustomGameObject
{
    public GameObject CreateGameObject(UnityEngine.Mesh mesh, Texture2D texture, AnimationClip clip, AudioClip audioClip)
    {
        // Create a new GameObject
        GameObject obj = new GameObject("CustomObject");

        // Add a MeshFilter and set its mesh
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Add a MeshRenderer and create a new Material with the given texture
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshRenderer.material.mainTexture = texture;

        // Add an Animator
        Animator animator = obj.AddComponent<Animator>();

        // Create an Animation Controller "on-the-fly"
        var controller = CreateAnimationController(clip);
        animator.runtimeAnimatorController = controller;

        // Add an AudioSource and set its AudioClip
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        return obj;
    }

    private RuntimeAnimatorController CreateAnimationController(AnimationClip clip)
    {
        // Create an Animation Controller to play our clip
        var controller = new UnityEditor.Animations.AnimatorController();

        // Add a parameter named "Play" of type Trigger
        controller.AddParameter("Play", UnityEngine.AnimatorControllerParameterType.Trigger);

        // Create a state in the root layer named "Play" which plays the given clip
        var rootStateMachine = controller.layers[0].stateMachine;
        var state = rootStateMachine.AddState("Play");
        state.motion = clip;

        // Set our "Play" state as the default state
        rootStateMachine.defaultState = state;

        // Create a transition from "Any State" to "Play" triggered by "Play"
        var transition = rootStateMachine.AddAnyStateTransition(state);
        transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "Play");

        return controller;
    }
}

public static class MeshUtility
{
    public static Mesh CreateUnityMesh(SBMesh sourceMesh)
    {
        var unityMesh = new UnityEngine.Mesh
        {
            name = sourceMesh.MeshId.ToString() ?? string.Empty
        };

        Vector3[] vertices = new Vector3[sourceMesh.MeshVertices.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vertex = sourceMesh.MeshVertices[i];
            vertices[i] = new Vector3(vertex[0], vertex[1], vertex[2]);
        }

        var normals = new Vector3[sourceMesh.MeshNormals.Count];
        for (int i = 0; i < normals.Length; i++)
        {
            var normal = sourceMesh.MeshNormals[i];
            normals[i] = new Vector3(normal[0], normal[1], normal[2]);
        }

        var uvs = new Vector2[sourceMesh.MeshUv.Count];
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

    public static Mesh JsonToMesh(ArcaneMesh data)
    {
        Mesh mesh = new Mesh();

        // Convert vertices
        List<Vector3> vertices = new List<Vector3>();
        foreach (var vertex in data.MeshVertices)
        {
            vertices.Add(new Vector3(vertex[0], vertex[1], vertex[2]));
        }

        // Convert normals and normalize
        List<Vector3> normals = new List<Vector3>();
        foreach (var normal in data.MeshNormals)
        {
            Vector3 tempNormal = new Vector3(normal[0], normal[1], normal[2]);
            normals.Add(tempNormal.normalized);
        }

        // Convert UVs and clamp to 0.0 - 1.0 range
        List<Vector2> uvs = new List<Vector2>();
        foreach (var uv in data.MeshUv)
        {
            Vector2 tempUV = new Vector2(Mathf.Clamp(uv[0], 0.0f, 1.0f), Mathf.Clamp(uv[1], 0.0f, 1.0f));
            uvs.Add(tempUV);
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetIndices(data.MeshIndices.ToArray(), MeshTopology.Triangles, 0);

        return mesh;
    }

}
