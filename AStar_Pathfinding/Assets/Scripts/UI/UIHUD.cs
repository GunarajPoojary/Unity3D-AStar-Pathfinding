using TMPro;
using UnityEngine;

/// <summary>
/// UI object to display Tile info
/// </summary>
public class UIHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _infoText; // To display tile position in the grid

    #region Unity Methods
    private void OnEnable() => SubscribeToEvents();
    private void OnDisable() => UnsubscribeFromEvents();
    #endregion

    private void SubscribeToEvents()
    {
        GameEvents.OnPointerEnter += SetTileInfo;
        GameEvents.OnPointerExit += ClearTileInfo;
    }

    private void UnsubscribeFromEvents()
    {
        GameEvents.OnPointerEnter -= SetTileInfo;
        GameEvents.OnPointerExit -= ClearTileInfo;
    }

    private void SetTileInfo(string tile) => _infoText.text = tile;
    private void ClearTileInfo() => _infoText.text = "Hover on any Tile";
}