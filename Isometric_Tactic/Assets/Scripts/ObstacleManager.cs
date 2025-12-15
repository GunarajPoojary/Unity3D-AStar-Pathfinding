using System;
using UnityEngine;

/// <summary>
/// Responsible for placing obstacles on the tile grid.
/// </summary>
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private SOObstacleData _obstacleData;
    [SerializeField] private GridMap _gridMap;

    [SerializeField] private GameObject[] _obstaclePrefabs;

    private void Start()
    {
        const int ROTATION_STEPS = 3; // forward, right, left and backward starts from 0 step.
        const float ANGLE = 90f;

        if (_obstacleData == null) throw new InvalidOperationException("ObstacleData field is not assigned in Obstacle Manager");
        if (_gridMap == null) throw new InvalidOperationException("GridObject field is not assigned in Obstacle Manager");

        for (int gridX = 0; gridX < _obstacleData.GridWidth; gridX++)
        {
            for (int gridZ = 0; gridZ < _obstacleData.GridHeight; gridZ++)
            {
                Tile tile = _gridMap.Grid.GetTile(gridX, gridZ); // Get the corresponding tile from the grid.

                if (tile == null) throw new NullReferenceException($"The tile is null at grid coords ({gridX}, {gridZ})");

                bool isObstacle = _obstacleData.IsObstacleAt(gridX, gridZ);
                tile.IsObstacle = isObstacle; // Mark the tile as obstacle.

                // Instantiate random obstacle at the tile's position.
                if (isObstacle)
                    Instantiate(_obstaclePrefabs[UnityEngine.Random.Range(0, _obstaclePrefabs.Length)], tile.transform.position, Quaternion.Euler(0f, UnityEngine.Random.Range(0, ROTATION_STEPS) * ANGLE, 0f), transform);
            }
        }
    }
}