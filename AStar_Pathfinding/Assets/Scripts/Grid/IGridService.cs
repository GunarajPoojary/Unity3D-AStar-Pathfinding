using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Must be inherited by GridManager 
/// </summary>
public interface IGridService
{
    List<Vector3> GetPathPoints(Vector3 startPoint, Vector3 endPoint);
}