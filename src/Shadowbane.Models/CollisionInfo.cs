namespace Shadowbane.Models;

using System.Collections.Generic;
using Geometry;

public class CollisionInfo
{
    /// <summary>
    /// </summary>
    public List<Vector3> bounds = new(); // make it a static array of 4 for now,  if all 4 aren't used - who cares.

    /// <summary>
    /// </summary>
    public uint nVectors; // Number of of vectors per polygon

    /// <summary>
    /// </summary>
    public List<ushort> order = new(); // The order in which the vectors are to be rendered ?

    /// <summary>
    /// </summary>
    public Vector3 unknown; // Unknown

    /// <summary>
    /// </summary>
    public Vector3 upVector; // Unknown - looks like an Up-Vector, will assume it is for now
}