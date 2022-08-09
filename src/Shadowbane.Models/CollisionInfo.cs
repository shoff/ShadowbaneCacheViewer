namespace Shadowbane.Models;

using System.Collections.Generic;
using System.Numerics;

public class CollisionInfo
{
    public List<Vector3> bounds = new(); // make it a static array of 4 for now,  if all 4 aren't used - who cares.
    public uint nVectors; // Number of of vectors per polygon
    public List<ushort> order = new(); // The order in which the vectors are to be rendered ?
    public Vector3 unknown; // Unknown
    public Vector3 upVector; // Unknown - looks like an Up-Vector, will assume it is for now
}