using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A* Pathfinding algorthim on 2D grid of tiles which uses four Directional movement while avoiding obstacles
/// Selects one with low fCost(Distance Cost from start to desired point) and gives out path points to the desired point
/// </summary>
public class Pathfinder
{
    private readonly TileGrid _grid; // Tiles/Nodes
    private List<Tile> _openList; // Tiles/Nodes queued up for searching
    private List<Tile> _closedList; // Tiles/Nodes that have already been searched

    public Pathfinder(TileGrid grid)
    {
        _grid = grid;
    }

    /// <summary>
    /// Finds a path between two world-space positions.
    /// Converts world positions to grid coordinates,
    /// then converts the resulting tile path back to world-space points.
    /// </summary>
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        // Convert world positions to grid coordinates
        _grid.GetGridCoord(startWorldPosition, out int startX, out int startY);
        _grid.GetGridCoord(endWorldPosition, out int endX, out int endY);

        List<Tile> path = FindPath(startX, startY, endX, endY);

        if (path != null)
        {
            List<Vector3> pathPoints = new List<Vector3>();

            // Convert list of tiles to world position
            for (int i = 1; i < path.Count; i++)
            {
                pathPoints.Add(new Vector3(path[i].GridX, 0f, path[i].GridZ) * _grid.TileSize);
            }

            return pathPoints;
        }

        return null;
    }

    /// <summary>
    /// A* algorithm using grid coordinates.
    /// Returns a list of tiles from start to end.
    /// </summary>
    private List<Tile> FindPath(int startX, int startY, int endX, int endY)
    {
        Tile startTile = _grid.GetTile(startX, startY);
        Tile endTile = _grid.GetTile(endX, endY);

        if (startTile == null || endTile == null || startTile.IsObstacle || endTile.IsObstacle)
            return null;

        _openList = new List<Tile> { startTile };
        _closedList = new List<Tile>();

        // Reset all Tile data
        // This ensures previous searches do not interfere
        for (int gridX = 0; gridX < _grid.Width; gridX++)
        {
            for (int gridZ = 0; gridZ < _grid.Height; gridZ++)
            {
                Tile tile = _grid.GetTile(gridX, gridZ);
                tile.GCost = int.MaxValue;
                tile.PreviousTile = null;
                tile.HCost = 0;
            }
        }

        startTile.GCost = 0;
        startTile.HCost = CalculateDistanceCost(startTile, endTile);

        while (_openList.Count > 0)
        {
            // Pick tile with lowest F cost
            Tile currentTile = GetLowestFCostTile(_openList);
            if (currentTile == endTile)
            {
                // Reached final node, reconstruct the path
                return CalculatePath(endTile);
            }

            _openList.Remove(currentTile);
            _closedList.Add(currentTile);

            // Evaluate all neighbors
            foreach (Tile neighbourTile in GetNeighbourTiles(currentTile))
            {
                // Skip obstacles and already evaluated tiles
                if (neighbourTile.IsObstacle) continue;
                if (_closedList.Contains(neighbourTile)) continue;

                // Movement cost from start to this neighbor
                int neighbourGCost = currentTile.GCost + 1;
                if (neighbourGCost < neighbourTile.GCost)
                {
                    // If this path to the neighbor is better then update it
                    neighbourTile.PreviousTile = currentTile;
                    neighbourTile.GCost = neighbourGCost;
                    neighbourTile.HCost = CalculateDistanceCost(neighbourTile, endTile);

                    // Add to open list if not already present
                    if (!_openList.Contains(neighbourTile))
                    {
                        _openList.Add(neighbourTile);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    /// <summary>
    /// Returns all valid neighboring tiles in 4-directions.
    /// </summary>
    private List<Tile> GetNeighbourTiles(Tile currentTile)
    {
        List<Tile> neighbourTiles = new List<Tile>();

        // Left
        if (currentTile.GridX - 1 >= 0)
            neighbourTiles.Add(GetTile(currentTile.GridX - 1, currentTile.GridZ));

        // Right
        if (currentTile.GridX + 1 < _grid.Width)
            neighbourTiles.Add(GetTile(currentTile.GridX + 1, currentTile.GridZ));

        // Down
        if (currentTile.GridZ - 1 >= 0)
            neighbourTiles.Add(GetTile(currentTile.GridX, currentTile.GridZ - 1));

        // Up
        if (currentTile.GridZ + 1 < _grid.Height)
            neighbourTiles.Add(GetTile(currentTile.GridX, currentTile.GridZ + 1));

        return neighbourTiles;
    }

    private Tile GetTile(int x, int y) => _grid.GetTile(x, y);

    /// <summary>
    /// Reconstructs the path by walking backward from the end tile to start tile 
    /// using the PreviousTile references.
    /// </summary>
    private List<Tile> CalculatePath(Tile endTile)
    {
        List<Tile> pathTiles = new List<Tile>();

        Tile currentTile = endTile;

        pathTiles.Add(endTile);

        // Walk backward until start tile is reached
        while (currentTile.PreviousTile != null)
        {
            currentTile = currentTile.PreviousTile;
            pathTiles.Add(currentTile);
        }

        // Reverse the path tiles to get start to end order
        pathTiles.Reverse();

        return pathTiles;
    }

    /// <summary>
    /// Manhattan distance heuristic.
    /// </summary>
    private int CalculateDistanceCost(Tile a, Tile b)
    {
        int xDistance = Mathf.Abs(a.GridX - b.GridX);
        int zDistance = Mathf.Abs(a.GridZ - b.GridZ);
        return xDistance + zDistance;
    }

    /// <summary>
    /// Returns the tile with the lowest F cost from a list.
    /// </summary>
    private Tile GetLowestFCostTile(List<Tile> tiles)
    {
        Tile lowestFCostTile = tiles[0];

        for (int i = 1; i < tiles.Count; i++)
        {
            if (tiles[i].FCost < lowestFCostTile.FCost)
            {
                lowestFCostTile = tiles[i];
            }
        }
        return lowestFCostTile;
    }
}