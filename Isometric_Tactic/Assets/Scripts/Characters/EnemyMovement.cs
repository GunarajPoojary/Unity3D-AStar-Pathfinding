using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Responsible for movement of the enemy
/// </summary>
public class EnemyMovement : UnitMovement
{
    public EnemyMovement(IGridService gridService, Transform transform, float moveSpeed) : base(
        gridService,
        transform,
        moveSpeed)
    { }

    /// <summary>
    /// Checks whehether the target position is same as enemy position. 
    /// If true do nothing, otherwise get pathPoints to reach the target and starts movement
    /// </summary>
    /// <param name="endPoint">Target tile position in the world</param>
    /// <param name="monoBehaviour">Runs movement coroutine</param>
    public override void MoveTowards(Vector3 enPoint, MonoBehaviour monoBehaviour)
    {
        Vector3 start = _transform.position;

        // check if target tile's position and player's position is same
        if (Mathf.Abs(Vector3.Distance(start, enPoint)) < DISTANCE_THROSHOLD) return;

        RaiseMoveStarted();

        List<Vector3> path = _gridService.GetPathPoints(start, enPoint);
        if (path == null) return;

        monoBehaviour.StartCoroutine(Movement(path));
    }

    /// <summary>
    /// Makes the enemy move to each point till it reaches one of the adjecent tile of player.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator Movement(List<Vector3> pathPoints)
    {
        for (int i = 0; i < pathPoints.Count - 1; i++) // Exclude last tile which is occupied by player
        {
            // Face movement direction when moving to target points.
            FaceDirection(GetFaceDirection(pathPoints[i]));

            // Move to each point until reaching the adjecent tile of player.
            while (Vector3.Distance(_transform.position, pathPoints[i]) > DISTANCE_THROSHOLD)
            {
                MoveTowards(pathPoints[i]);

                yield return null;
            }
        }

        RaiseMoveEnd();
    }

    protected override void RaiseMoveStarted() => GameEvents.RaiseOnEnemyMoveStarted();
    protected override void RaiseMoveEnd() => GameEvents.RaiseOnEnemyMoveEnd();
}