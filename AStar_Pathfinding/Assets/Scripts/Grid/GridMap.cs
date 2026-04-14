using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Grid Map game object in 3D world space
/// </summary>
public class GridMap : MonoBehaviour
{
    [Tooltip("Grid Coordinate settings")]
    [SerializeField] private SOGridConfig _gridConfig;
    [SerializeField] private Tile _tilePrefab;

    [Tooltip("Size of the Tile Cube")]
    [SerializeField] private float _tileSize = 1f;

    private TileGrid _grid;
    private Pathfinder _pathfinder;

    // Property
    public TileGrid Grid => _grid;

    /// <summary>
    /// Initialize Grid and Pathfinding algorithm
    /// </summary>
    public void Init()
    {
        _grid = new TileGrid(_gridConfig.GridWidth, _gridConfig.GridHeight, _tileSize, transform.position, CreateTile);
        _pathfinder = new Pathfinder(_grid);
    }

    private Tile CreateTile(TileGrid grid, int gridX, int gridZ)
    {
        Tile tile = Instantiate(_tilePrefab, grid.GetWorldPosition(gridX, gridZ), Quaternion.identity, transform);
        tile.GridX = gridX;
        tile.GridZ = gridZ;
        return tile;
    }

    /// <summary>
    /// Gives out list of vector3 points in the world space.
    /// </summary>
    /// <param name="startPoint">Starting position of the object in the world space</param>
    /// <param name="endPoint">Desired position in the world space</param>
    /// <returns></returns>
    public List<Vector3> GetPathPoints(Vector3 startPoint, Vector3 endPoint) => _pathfinder.FindPath(startPoint, endPoint);

    /// <summary>
    /// Converts World Position to Grid Coordinates.
    /// </summary>
    /// <param name="worldPosition">World position of the tile.</param>
    /// <param name="gridX">The grid X coordinate/column index.</param>
    /// <param name="gridZ">The grid Z coordinate/Row index.</param>
    public void GetGridCoord(Vector3 worldPosition, out int gridX, out int gridZ) => _grid.GetGridCoord(worldPosition, out gridX, out gridZ);

    /// <summary>
    /// Gets the Tile present at the provided grid coordinates.
    /// </summary>
    /// <param name="gridX">The grid X coordinate/column index.</param>
    /// <param name="gridZ">The grid Z coordinate/Row index.</param>
    /// <returns>
    /// Tile present at the provided grid coordinates.
    /// </returns>
    public Tile GetTile(int gridX, int gridZ) => _grid.GetTile(gridX, gridZ);

    public List<Tile> GetEmptyTiles() => _grid.GetEmptyTiles();

    public void SetOccupiedAt(bool isOccupied, Vector3 position)
    {
        _grid.GetGridCoord(position, out int gridX, out int gridZ);
        _grid.GetTile(gridX, gridZ).IsOccupied = isOccupied;
    }
}