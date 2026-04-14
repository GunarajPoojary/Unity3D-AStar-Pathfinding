using System;

/// <summary>
/// Must be inherited by PlayerInputHandler
/// </summary>
public interface IPlayerInputHandler
{
    bool IsInputDisabled { get; set; }
    event Action<Tile> OnSelectTile;

    void HandleInput();
}