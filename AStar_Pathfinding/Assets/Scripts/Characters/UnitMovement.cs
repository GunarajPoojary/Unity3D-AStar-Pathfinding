using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Responsible for movement of unit (Player/Enemy). Must be inherited.
/// </summary>
public abstract class UnitMovement : IUnitMovement
{
    protected readonly Transform _transform;
    protected readonly float _moveSpeed;
    protected IGridService _gridService;
    protected const float DISTANCE_THROSHOLD = 0.05f;

    public UnitMovement(IGridService gridService, Transform transform, float moveSpeed)
    {
        _gridService = gridService;
        _transform = transform;
        _moveSpeed = moveSpeed;
    }

    #region  IUnitMovement Members
    public abstract void MoveTowards(Vector3 targetPoint, MonoBehaviour monoBehaviour);
    #endregion

    #region Reusable Methods
    protected abstract void RaiseMoveStarted();
    protected abstract void RaiseMoveEnd();

    /// <summary>
    /// Moves the unit toward the taergt point at a fixed value.
    /// </summary>
    /// <param name="target">The target world position to reach</param>
    protected void MoveTowards(Vector3 target) => _transform.position = Vector3.MoveTowards(_transform.position, target, _moveSpeed * Time.deltaTime);

    /// <summary>
    /// Makes the unit's forward direction face given direction.
    /// </summary>
    /// <param name="lookDirection"></param>
    protected void FaceDirection(Vector3 lookDirection)
    {
        if (lookDirection != Vector3.zero)
            _transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    protected bool TryGetPathPoints(Vector3 start, Vector3 end, out List<Vector3> pathPoints)
    {
        pathPoints = null;

        // check if target tile's position and player's position is same.
        if (Mathf.Abs(Vector3.Distance(start, end)) < DISTANCE_THROSHOLD) return false;

        // Get pathpoints from pathfinder.
        pathPoints = _gridService.GetPathPoints(start, end);

        return true;
    }

    /// <summary>
    /// Returns the cardinal facing direction.
    /// </summary>
    /// <param name="targetToFace">Target's position in the world</param>
    /// <returns></returns>
    protected Vector3 GetFaceDirection(Vector3 targetToFace)
    {
        Vector3 distance = targetToFace - _transform.position;

        // Snap to cardinal direction.
        return Mathf.Abs(distance.x) > Mathf.Abs(distance.z) ? new Vector3(Mathf.Sign(distance.x),
                                                                     0f,
                                                                     0f) : new Vector3(0f, 0f, Mathf.Sign(distance.z));
    }
    #endregion
}