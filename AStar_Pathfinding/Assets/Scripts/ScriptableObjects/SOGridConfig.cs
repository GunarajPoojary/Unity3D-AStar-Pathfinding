using UnityEngine;

/// <summary>
/// A ScriptableObject containing static grid coordinate settings.
/// </summary>
[CreateAssetMenu(fileName = "SOGridConfig", menuName = "SOGridConfig")]
public class SOGridConfig : ScriptableObject
{
    // Modify only in the inspector
    [field: Tooltip("Number of Tiles in grid X Coordinate")]
    [field: SerializeField] public int GridWidth { get; private set; } = 10;

    [field: Tooltip("Number of Tiles in grid Z Coordinate")]
    [field: SerializeField] public int GridHeight { get; private set; } = 10;
}