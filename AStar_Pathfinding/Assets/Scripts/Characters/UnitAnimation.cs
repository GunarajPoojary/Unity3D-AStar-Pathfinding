using UnityEngine;

/// <summary>
/// Handles unit Animation
/// </summary>
public class UnitAnimation : IUnitAnimation
{
    private Animator _animator;
    private static readonly int IsMovingHash = Animator.StringToHash("isMoving");

    public UnitAnimation(Animator animator)
    {
        _animator = animator;
    }

    public void SetMovementAnimation(bool isMoving) => _animator.SetBool(IsMovingHash, isMoving);
}