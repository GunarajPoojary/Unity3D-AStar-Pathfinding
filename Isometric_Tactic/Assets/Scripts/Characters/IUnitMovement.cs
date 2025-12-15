using UnityEngine;

/// <summary>
/// Inherited by abstract UnitMovement class 
/// </summary>
public interface IUnitMovement
{
    void MoveTowards(Vector3 targetPoint, MonoBehaviour monoBehaviour); // Monobehavior to run the coroutine
}