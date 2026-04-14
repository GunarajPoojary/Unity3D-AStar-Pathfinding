using System;
using UnityEngine;

/// <summary>
/// Static events
/// </summary>
public static class GameEvents
{
    #region Events
    public static event Action<string> OnPointerEnter;
    public static event Action<Tile> OnSelectTile;
    public static event Action<Vector3> OnPlayerMoveStarted;
    public static event Action<Vector3> OnPlayerMoveEnd;
    public static event Action<Vector3> OnEnemyMoveEnd;
    public static event Action<Vector3> OnEnemyMoveStarted;
    public static event Action OnPointerExit;
    #endregion

    #region Event Publish Methods
    public static void RaiseOnPointerEnter(string value) => OnPointerEnter?.Invoke(value);
    public static void RaiseOnPointerExit() => OnPointerExit?.Invoke();
    public static void RaiseSelectTileEvent(Tile value) => OnSelectTile?.Invoke(value);
    public static void RaiseOnPlayerMoveStarted(Vector3 value) => OnPlayerMoveStarted?.Invoke(value);
    public static void RaiseOnPlayerMoveEnd(Vector3 value) => OnPlayerMoveEnd?.Invoke(value);
    public static void RaiseOnEnemyMoveStarted(Vector3 value) => OnEnemyMoveStarted?.Invoke(value);
    public static void RaiseOnEnemyMoveEnd(Vector3 value) => OnEnemyMoveEnd?.Invoke(value);
    #endregion
}