using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile/NewTileData", menuName = "Dungeon/TileData", order = 1)]
public class Dungeon_TileData : ScriptableObject
{
    public string TileName;
    public Sprite TileSprite;
    public bool CanModify = true;
    public TileType TileType = TileType.Room;
    public Direction TileExitDirections;
}
