using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Attached to Player Unit in the world handles playerMovement, animation and manages input 
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _tileMask;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private Animator _animatorController;

    private IUnitMovement _movement;
    private IUnitAnimation _playerAnim;
    private IPlayerInputHandler _input;

    #region Unity MEthods
    private void Awake()
    {
        SetReferences();
        CheckReference();
    }

    private void OnEnable() => SubscribeToEvents();
    private void OnDisable() => UnsubscribeFromEvents();
    private void Update() => _input.HandleInput();
    #endregion

    private void SetReferences()
    {
        _movement = new PlayerMovement(GridManager.Instance, transform, _moveSpeed);
        _playerAnim = new UnitAnimation(_animatorController);
        _input = new PlayerInputHandler(_tileMask);
    }

    private void CheckReference()
    {
        if (_animatorController == null) throw new NullReferenceException("Animator Component is not assigned in PlayerController");
    }

    private void SubscribeToEvents()
    {
        _input.OnSelectTile += MoveTo;

        GameEvents.OnPlayerMoveStarted += HandleMoveStarted;
        GameEvents.OnPlayerMoveEnd += HandleMoveEnd;

        GameEvents.OnEnemyMoveStarted += HandleEnemyMoveStarted;
        GameEvents.OnEnemyMoveEnd += HandleEnemyMoveEnd;
    }

    private void UnsubscribeFromEvents()
    {
        _input.OnSelectTile -= MoveTo;

        GameEvents.OnPlayerMoveStarted -= HandleMoveStarted;
        GameEvents.OnPlayerMoveEnd -= HandleMoveEnd;

        GameEvents.OnEnemyMoveStarted -= HandleEnemyMoveStarted;
        GameEvents.OnEnemyMoveEnd -= HandleEnemyMoveEnd;
    }

    private void HandleMoveStarted(Vector3 startPosition)
    {
        _playerAnim.SetMovementAnimation(true);

        _input.IsInputDisabled = true;
    }

    private void HandleMoveEnd(Vector3 endPoint)
    {
        _playerAnim.SetMovementAnimation(false);

        _input.IsInputDisabled = false;
    }

    private void HandleEnemyMoveStarted(Vector3 startPosition) => _input.IsInputDisabled = true;
    private void HandleEnemyMoveEnd(Vector3 endPosition) => _input.IsInputDisabled = false;

    private void MoveTo(Tile targetTile)
    {
        if (!targetTile.IsObstacle && !targetTile.IsOccupied)
            _movement.MoveTowards(targetTile.transform.position, this);
    }
}