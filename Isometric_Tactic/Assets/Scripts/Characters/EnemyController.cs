using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Attached to Enemy Unit in the world handles EnemyMovement and animation 
/// </summary>
public class EnemyController : MonoBehaviour, IAI
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private Animator _animatorController;
    private IUnitMovement _movement;
    private IUnitAnimation _enemyAnim;

    #region Unity MEthods
    private void Awake()
    {
        SetReferences();
        CheckReference();
    }

    private void OnEnable() => SubscribeToEvents();
    private void OnDisable() => UnsubscribeFromEvents();
    #endregion

    private void SubscribeToEvents()
    {
        GameEvents.OnPlayerMoveEnd += TakeTurn;
        GameEvents.OnEnemyMoveStarted += HandleMoveStarted;
        GameEvents.OnEnemyMoveEnd += HandleMoveEnd;
    }

    private void UnsubscribeFromEvents()
    {
        GameEvents.OnPlayerMoveEnd -= TakeTurn;
        GameEvents.OnEnemyMoveStarted -= HandleMoveStarted;
        GameEvents.OnEnemyMoveEnd -= HandleMoveEnd;
    }

    private void HandleMoveStarted(Vector3 startPosition) => _enemyAnim.SetMovementAnimation(true);
    private void HandleMoveEnd(Vector3 endPosition) => _enemyAnim.SetMovementAnimation(false);

    private void SetReferences()
    {
        _enemyAnim = new UnitAnimation(_animatorController);
        _movement = new EnemyMovement(GridManager.Instance, transform, _moveSpeed);
    }

    private void CheckReference()
    {
        if (_animatorController == null) throw new NullReferenceException("Animator Component is not assigned in PlayerController");
    }

    public void TakeTurn(Vector3 playerPosition) => _movement.MoveTowards(playerPosition, this);
}