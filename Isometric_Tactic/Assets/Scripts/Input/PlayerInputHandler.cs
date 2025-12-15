using UnityEngine;
using System;

/// <summary>
/// Handles player mouse hover and mouse cick on Tile.
/// </summary>
public class PlayerInputHandler : IPlayerInputHandler
{
    private readonly LayerMask _tileMask; // Ignore other layers except one that is used by Tile.

    private const float MAX_RAY_DISTANCE = 100f;
    private Camera _mainCamera;

    public bool IsInputDisabled { get; set; } = false;

    public event Action<Tile> OnSelectTile;

    public PlayerInputHandler(LayerMask tileMask)
    {
        _tileMask = tileMask;
        _mainCamera = Camera.main;
    }

    /// <summary>
    /// Detects which grid tile the mouse is pointing and clicked.
    /// Must be called in every frame to raise event.
    /// </summary>
    public void HandleInput()
    {
        if (IsInputDisabled) return;

        // Create a Ray from MousePosition in screen space starting from camera and travels straight in the 3D world where mouse pointing.
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        // Check if the ray hits any grid tile
        bool isHitTile = Physics.Raycast(ray, out RaycastHit hit, MAX_RAY_DISTANCE, _tileMask);

        if (isHitTile)
        {
            // Use TryGetComponent instead of GetComponent because TryGetComponent doesn't allocate
            if (hit.collider.TryGetComponent(out Tile tile))
            {
                GameEvents.RaiseOnPointerEnter(tile.ToString());

                if (Input.GetMouseButtonDown(0))
                    OnSelectTile?.Invoke(tile);
            }
            else
            {
                GameEvents.RaiseOnPointerExit();
            }
        }
        else
        {
            GameEvents.RaiseOnPointerExit();
        }
    }
}