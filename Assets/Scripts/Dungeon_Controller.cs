using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Controller : MonoBehaviour
{
    [SerializeField] GameObject Tile;

    [SerializeField] Dungeon_TileData EmptyRoom;
    [SerializeField] Dungeon_TileData EmptyTile;

    public Dungeon_Tile[,] tiles = new Dungeon_Tile[6, 5];  
    
    private void Start()
    {
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                tiles[i,j] = Instantiate(Tile, transform).GetComponent<Dungeon_Tile>();
                tiles[i, j].GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 96, j * -96, 0);
            }
        }

        SetTile(3, 4, EmptyTile);
        SetTile(3, 4, EmptyRoom);
    }

    private void SetTile(int x, int y, Dungeon_TileData data)
    {
        if (tiles[x,y].tileData.TileType == TileType.Empty)
        {
            if (x > 0) { if (tiles[x - 1, y].tileData.TileType == TileType.Locked) tiles[x - 1, y].UpdateTile(EmptyTile); }
            if (x < 5) { if (tiles[x + 1, y].tileData.TileType == TileType.Locked) tiles[x + 1, y].UpdateTile(EmptyTile); }
            if (y > 0) { if (tiles[x, y - 1].tileData.TileType == TileType.Locked) tiles[x, y - 1].UpdateTile(EmptyTile); }
            if (y < 4) { if (tiles[x, y + 1].tileData.TileType == TileType.Locked) tiles[x, y + 1].UpdateTile(EmptyTile); }
        }
        tiles[x, y].UpdateTile(data);
    }

}
