namespace Shadowbane.Models;

using System;
using System.Numerics;

public class Bone
{
    private readonly ReadOnlyMemory<byte> data;

    public Bone(ReadOnlyMemory<byte> data)
    {
        this.data = data;
        this.Id = 0;
        this.MeshId = 0;
        this.NumberOfChildren = 0;
        this.ParentId = 0;
        this.Children = 0;

        this.Direction = Vector3.Zero;
        this.Axis = Vector3.Zero;
        this.Length = 0.0f;

        this.Pos = Vector3.Zero;
        this.Rot = Quaternion.Identity;
        this.Scale = new Vector3(1, 1, 1);

        this.Mat = Matrix4x4.Identity;// not sure here
        this.RMat = Matrix4x4.Identity;
        this.Setup = false;
        this.Flip = false;
    }
    public bool Setup { get; set; }
    public Matrix4x4 RMat { get; set; }
    public Matrix4x4 Mat { get; set; }
    public Vector3 Scale { get; set; }
    public Quaternion Rot { get; set; }
    public Vector3 Pos { get; set; }
    public bool Flip { get; set; }
    public float Length { get; set; }
    public Vector3 Axis { get; set; }
    public Vector3 Direction { get; set; }
    public int Children { get; set; }
    public int ParentId { get; set; }
    public int NumberOfChildren { get; set; }
    public int MeshId { get; set; }
    public int Id { get; set; }
}