using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Responsible for movement of the player.
/// </summary>
public class PlayerMovement : UnitMovement
{
    public PlayerMovement(IGridService gridService, Transform transform, float moveSpeed) : base(
        gridService,
        transform,
        moveSpeed)
    { }

    /// <summary>
    /// Checks whehether the target position is same as player position. 
    /// If true do nothing, otherwise get pathPoints to reach the target and starts movement
    /// </summary>
    /// <param name="endPoint">Target tile position in the world</param>
    /// <param name="monoBehaviour">Runs movement coroutine</param>
    public override void MoveTowards(Vector3 endPoint, MonoBehaviour monoBehaviour)
    {
        Vector3 start = _transform.position;

        // Check if target tile's position and player's position is same.
        if (Mathf.Abs(Vector3.Distance(start, endPoint)) < DISTANCE_THROSHOLD) return;

        RaiseMoveStarted();

        List<Vector3> path = _gridService.GetPathPoints(start, endPoint);
        if (path == null) return;

        monoBehaviour.StartCoroutine(Move(path));
    }

    /// <summary>
    /// Makes the player move to each point.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator Move(List<Vector3> path)
    {
        for (int i = 0; i < path.Count; i++) // Exclude first tile which is occupied by player.
        {
            // Face movement direction when moving to target points.
            FaceDirection(GetFaceDirection(path[i]));

            // Move to each point until reaching the end point.
            while (Vector3.Distance(_transform.position, path[i]) > DISTANCE_THROSHOLD)
            {
                MoveTowards(path[i]);

                yield return null;
            }
        }

        RaiseMoveEnd();
    }

    protected override void RaiseMoveStarted() => GameEvents.RaiseOnPlayerMoveStarted();
    protected override void RaiseMoveEnd() => GameEvents.RaiseOnPlayerMoveEnd(_transform.position);
}