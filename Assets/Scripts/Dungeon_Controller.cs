using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Controller : MonoBehaviour
{
    [SerializeField] public GameObject PlayerObject;

    [SerializeField] GameObject Tile;

    [SerializeField] Dungeon_TileData EmptyRoom;
    [SerializeField] Dungeon_TileData EmptyTile;

    DungeonLayout currentDungeon;
    DungeonWalker currentWalker;

    [SerializeField] public Dungeon_Tile[] alltiles;
    public Dungeon_Tile[,] tiles = new Dungeon_Tile[6,5];

    private void Start()
    {
        for(int i = 0; i < alltiles.Length; i++)
        {
            tiles[i % 6, Mathf.FloorToInt(i / 6)] = alltiles[i];
        }

        SetTile(3, 4, EmptyTile);
        SetTile(3, 4, EmptyRoom);
        SetTile(3, 3, EmptyRoom);

        LoadDungeonPathSolver();
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
        UpdateTileExits(x, y, data);
    }

    private void UpdateTileExits(int x, int y, Dungeon_TileData data)
    {
        if ((int)tiles[x, y].tileData.TileType > 1)
        {
            if (data.TileExitDirections.HasFlag(Direction.North) && y > 0 && tiles[x, y].tileData.TileExitDirections.HasFlag(Direction.South))
            {
                tiles[x, y].TileRealExits |= Direction.North;
                tiles[x, y - 1].TileRealExits |= Direction.South;
            }
            if (data.TileExitDirections.HasFlag(Direction.South))
            {
                if (y < 4 && tiles[x, y].tileData.TileExitDirections.HasFlag(Direction.North)){
                    tiles[x, y].TileRealExits |= Direction.South;
                    tiles[x, y + 1].TileRealExits |= Direction.North;
                }
                else if (x == 3 && y == 4) //Boss Room
                {
                    tiles[x, y].TileRealExits |= Direction.South;
                }
            }

            if (data.TileExitDirections.HasFlag(Direction.West) && x > 0 && tiles[x-1, y].tileData.TileExitDirections.HasFlag(Direction.East))
            {
                tiles[x, y].TileRealExits |= Direction.West;
                tiles[x - 1, y].TileRealExits |= Direction.East;
            }
            if (data.TileExitDirections.HasFlag(Direction.East) && x < 5 && tiles[x+1, y].tileData.TileExitDirections.HasFlag(Direction.West))
            {
                tiles[x, y].TileRealExits |= Direction.East;
                tiles[x + 1, y].TileRealExits |= Direction.West;
            }
        }
    }


    private void LoadDungeonPathSolver()
    {
        currentDungeon = new DungeonLayout(6, 5);
        //Resolve Start
        currentDungeon.startingRoomLocation = (3, 4);

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                DungeonRoom currentRoom = new DungeonRoom();
                Dungeon_Tile roomtile = tiles[i, j];
                //Need to calculate exits
                bool northExit = roomtile.TileRealExits.HasFlag(Direction.North);
                bool eastExit = roomtile.TileRealExits.HasFlag(Direction.East);
                bool southExit = roomtile.TileRealExits.HasFlag(Direction.South);
                bool westExit = roomtile.TileRealExits.HasFlag(Direction.West);

                currentRoom.door = (northExit, eastExit, southExit, westExit);
                currentDungeon.setRoom(i, j, currentRoom);
            }
        }

        currentWalker = new DungeonWalker(currentDungeon, currentDungeon.startingRoomLocation, 0);
        Player.transform.position = tiles[currentDungeon.startingRoomLocation.x, currentDungeon.startingRoomLocation.y].gameObject.transform.position + Vector3.back;
    }


    [SerializeField] GameObject Player;
    [SerializeField] float stepTimeDelay = 1f;
    private float currentTimer = 0;
    void Update()
    {
        if (currentTimer > stepTimeDelay)
        {
            (int x, int y) newPosition = currentWalker.moveStep();
            Player.transform.position = tiles[newPosition.x,newPosition.y].gameObject.transform.position + Vector3.back;
            currentTimer = 0;
        }
        currentTimer += Time.deltaTime;
    }
}
