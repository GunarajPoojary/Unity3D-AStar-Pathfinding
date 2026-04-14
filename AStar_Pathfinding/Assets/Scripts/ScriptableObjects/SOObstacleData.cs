using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stores data about obstacles on Tiles Grid
/// </summary>
[CreateAssetMenu(fileName = "ObstacleData", menuName = "ObstacleData")]
public class SOObstacleData : ScriptableObject
{
    // Fields
    [Tooltip("Grid Coordinate settings")]
    [SerializeField] private SOGridConfig _gridConfig;

    [SerializeField]
    [HideInInspector] // Avoid setting in inspector
    private bool[] _obstacles; // Use 1D array instead of 2D array because 2D array is not serializable in Unity.

    // Properties
    public int GridWidth => _gridConfig != null ? _gridConfig.GridWidth
    : throw new InvalidOperationException("SOGridConfig field is not assigned to ObstacleData ScriptableObject");
    public int GridHeight => _gridConfig != null ? _gridConfig.GridHeight
    : throw new InvalidOperationException("SOGridConfig field is not assigned to ObstacleData ScriptableObject");

    public void Init()
    {
        int arrayLength = _gridConfig.GridWidth * _gridConfig.GridHeight;

        if (_obstacles == null || _obstacles.Length != arrayLength) // This makes sure that array is not created 
                                                                    // over and over agian in the Editor Window.
            _obstacles = new bool[arrayLength];
    }

    /// <summary>
    /// Gets isObstacle flag from the given grid coordinate.
    /// </summary>
    /// <param name="gridX">Grid X Coordinate/Column index.</param>
    /// <param name="gridZ">Grid Z Coordinate/Row Index.</param>
    /// <returns>Obstacle flag</returns>
    public bool IsObstacleAt(int gridX, int gridZ) => _obstacles[ToArrayIndex(gridX, gridZ)];

    /// <summary>
    /// Sets the obstacle flag at given grid coordinate.
    /// </summary>
    /// <param name="gridX">Grid X Coordinate/Column index.</param>
    /// <param name="gridZ">Grid Z Coordinate/Row Index.</param>
    /// <param name="isObstacle">Value</param>
    public void SetObstacleAt(int gridX, int gridZ, bool isObstacle) => _obstacles[ToArrayIndex(gridX, gridZ)] = isObstacle;

    /// <summary>
    /// Converts grid coordinate to 1D array index.
    /// </summary>
    /// <param name="gridX">Grid X Coordinate/Column index.</param>
    /// <param name="gridZ">Grid Z Coordinate/Row Index.</param>
    /// <returns>1D array index.</returns>
    private int ToArrayIndex(int gridX, int gridZ) // TODO: Move this method to a static Grid Utility class
    {
        // Converts grid coordinates into a 1D array index
        return gridZ * _gridConfig.GridWidth + gridX;
    }
}