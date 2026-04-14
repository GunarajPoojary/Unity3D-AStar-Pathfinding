using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A Grid of tiles which uses 2D array to store tiles and uses Unity's XZ plane for Grid coordinate.
/// </summary>
public class TileGrid
{
    // Fields
    private readonly Tile[,] _tilesGrid;
    private Vector3 _originPosition; // Starting local position in the world or pivot.

    // Properties
    public int Width { get; } // No. of Columns in the Grid.
    public int Height { get; } // No. of Rows in the Grid.
    public float TileSize { get; } // Size of the tile, assuming the object has equal side lengths. Which is true in our case.

    public TileGrid(int width, int height, float tileSize, Vector3 originPosition, Func<TileGrid, int, int, Tile> createTile)
    {
        Width = width;
        Height = height;
        TileSize = tileSize;
        _originPosition = originPosition;

        _tilesGrid = new Tile[width, height];

        for (int gridX = 0; gridX < _tilesGrid.GetLength(0); gridX++) // Iterates through each tile in Row.
        {
            for (int gridZ = 0; gridZ < _tilesGrid.GetLength(1); gridZ++) // Iterates through each tile in Column.
            {
                _tilesGrid[gridX, gridZ] = createTile(this, gridX, gridZ); // Create and assign tile.
            }
        }
    }

    /// <summary>
    /// Converts Grid XZ Coordinates to World Position XYZ plane.
    /// </summary>
    /// <param name="gridX">The grid X coordinate/column index.</param>
    /// <param name="gridZ">The grid Z coordinate/Row index.</param>
    /// <returns>World position.</returns>
    public Vector3 GetWorldPosition(int gridX, int gridZ) // TODO: Move this method to a static Grid Utility class
                        => new Vector3(gridX, 0, gridZ) * TileSize + _originPosition;

    /// <summary>
    /// Converts World Position to Grid Coordinates.
    /// </summary>
    /// <param name="worldPosition">World position of the tile.</param>
    /// <param name="gridX">The grid X coordinate/column index.</param>
    /// <param name="gridZ">The grid Z coordinate/Row index.</param>
    public void GetGridCoord(Vector3 worldPosition, out int gridX, out int gridZ) // TODO: Move this method to a static Grid Utility class
    {
        Vector3 localPos = worldPosition - _originPosition;

        gridX = Mathf.RoundToInt(localPos.x / TileSize); // RoundToInt gives 2 if value is 1.5, for 1.2 value becomes 1.  
        gridZ = Mathf.RoundToInt(localPos.z / TileSize);// RoundToInt gives -2 if value is -1.5, for -1.2 value becomes -1.

        // Make sure the grid coordinate values don't exceed bounds
        gridX = Mathf.Clamp(gridX, 0, Width - 1);
        gridZ = Mathf.Clamp(gridZ, 0, Height - 1);
    }

    /// <summary>
    /// Sets the Tile at given grid coordinate values
    /// </summary>
    /// <param name="gridX">The grid X coordinate/column index.</param>
    /// <param name="gridZ">The grid Z coordinate/Row index.</param>
    /// <param name="value">grid object</param>
    public void SetTile(int gridX, int gridZ, Tile value) => _tilesGrid[gridX, gridZ] = value;

    /// <summary>
    /// Gets the Tile present at the provided grid coordinates
    /// </summary>
    /// <param name="gridX">The grid X coordinate/column index.</param>
    /// <param name="gridZ">The grid Z coordinate/Row index.</param>
    /// <returns>
    /// Tile present at the provided grid coordinates
    /// </returns>
    public Tile GetTile(int gridX, int gridZ) => _tilesGrid[gridX, gridZ];

    public List<Tile> GetEmptyTiles()
    {
        List<Tile> emptyTiles = new List<Tile>();

        for (int gridX = 0; gridX < _tilesGrid.GetLength(0); gridX++) 
        {
            for (int gridZ = 0; gridZ < _tilesGrid.GetLength(1); gridZ++) 
            {
                Tile tile = GetTile(gridX, gridZ);
                if (!tile.IsObstacle)
                {
                    emptyTiles.Add(tile);
                }
            }
        }

        return emptyTiles;
    }
}