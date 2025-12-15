using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class GridManager : MonoBehaviour, IGridService
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private GridMap _gridMap;

    #region Unity Method
    private void Awake()
    {
        Instance = this;

        _gridMap.Init();
    }
    #endregion

    /// <summary>
    /// Gives out list of vector3 points in the world space.
    /// </summary>
    /// <param name="startPoint">Starting position of the object in the world space.</param>
    /// <param name="endPoint">Desired position in the world space.</param>
    /// <returns></returns>
    public List<Vector3> GetPathPoints(Vector3 startPoint, Vector3 endPoint) => _gridMap.GetPathPoints(startPoint, endPoint);
}