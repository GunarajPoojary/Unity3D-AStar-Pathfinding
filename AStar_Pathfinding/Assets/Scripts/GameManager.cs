using UnityEngine;
using System.Collections.Generic;

// TODO: Should SRP for Player and Enemy to handle spawn
public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private PlayerController _playerUnit;
    [SerializeField] private EnemyController _enemyUnit;

    private void Start() => SetupUnits();

    private void SetupUnits()
    {
        List<Tile> emptyTiles = _gridManager.GetEmptyTiles();

        Tile playerTile = emptyTiles[Random.Range(0, emptyTiles.Count)];
        Tile enemyTile = emptyTiles[Random.Range(0, emptyTiles.Count)];

        while (enemyTile == playerTile)
            enemyTile = emptyTiles[Random.Range(0, emptyTiles.Count)];

        playerTile.IsOccupied = true;
        enemyTile.IsOccupied = true;

        _playerUnit.transform.position = playerTile.transform.position;
        _enemyUnit.transform.position = enemyTile.transform.position;
    }
}