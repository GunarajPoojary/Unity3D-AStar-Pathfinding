using System;
using UnityEngine;

/// <summary>
/// A grid tile object in the world.
/// </summary>
public class Tile : MonoBehaviour
{
    // Properties
    public int GridX { get; set; }  // X position on the grid/column index.
    public int GridZ { get; set; } // Z position on the grid/row index.

    public bool IsObstacle { get; set; }

    public int GCost { get; set; } // Walking cost from the start node.
    public int HCost { get; set; } // Heuristic Cost to reach End Node.
    public int FCost => GCost + HCost; // Final Cost.

    public Tile PreviousTile { get; set; }

    public override string ToString() => $"(X = {GridX}, Z = {GridZ})";
}