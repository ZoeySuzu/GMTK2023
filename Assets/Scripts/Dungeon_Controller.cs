using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Controller : MonoBehaviour
{
    public static Dungeon_Controller Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [SerializeField] public GameObject PlayerObject;

    [SerializeField] GameObject Tile;

    [SerializeField] Dungeon_TileData EmptyRoom;
    [SerializeField] Dungeon_TileData EmptyTile;
    [SerializeField] GameObject BossRoom;

    [SerializeField] GameObject StartIcon;

    DungeonLayout currentDungeon;
    DungeonWalker currentWalker;

    [SerializeField] public Dungeon_Tile[] alltiles;
    public Dungeon_Tile[,] tiles = new Dungeon_Tile[6,5];

    private (int x, int y) startingPosition = (3, 4);
    private bool isValidStartPosition = true;

    private void Start()
    {
        for(int i = 0; i < alltiles.Length; i++)
        {
            int x = i % 6;
            int y = Mathf.FloorToInt(i / 6);
            tiles[x, y] = alltiles[i];
            tiles[x, y].UpdateTilePosition(x, y);
        }

        SetTile(3, 4, EmptyTile);
        SetTile(3, 4, EmptyRoom);
    }

    private void SetTile(int x, int y, Dungeon_TileData data)
    {
        if (tiles[x,y].tileData.TileType == TileType.Locked)
        {
            if (x > 0) { if (tiles[x - 1, y].tileData.TileType == TileType.Locked) tiles[x - 1, y].UpdateTile(EmptyTile); }
            if (x < 5) { if (tiles[x + 1, y].tileData.TileType == TileType.Locked) tiles[x + 1, y].UpdateTile(EmptyTile); }
            if (y > 0) { if (tiles[x, y - 1].tileData.TileType == TileType.Locked) tiles[x, y - 1].UpdateTile(EmptyTile); }
            if (y < 4) { if (tiles[x, y + 1].tileData.TileType == TileType.Locked) tiles[x, y + 1].UpdateTile(EmptyTile); }
        }
        tiles[x, y].UpdateTile(data);
        UpdateTileExits(x, y, data);
    }

    public void UpdateTileExits(int x, int y, Dungeon_TileData data)
    {
        if ((int)tiles[x, y].tileData.TileType > 0)
        {
            if (data.TileExitDirections.HasFlag(Direction.North) && y > 0)
            {
                if (tiles[x, y - 1].tileData.TileType == TileType.Locked) tiles[x, y - 1].UpdateTile(EmptyTile);
                if (tiles[x, y - 1].tileData.TileExitDirections.HasFlag(Direction.South))
                {
                    tiles[x, y].TileRealExits |= Direction.North;
                    tiles[x, y - 1].TileRealExits |= Direction.South;
                }
                else
                {
                    tiles[x, y].TileRealExits &= ~Direction.North;
                    tiles[x, y - 1].TileRealExits &= ~Direction.South;
                }
            }
            else
            {
                tiles[x, y].TileRealExits &= ~Direction.North;
                if (y > 0)
                    tiles[x, y - 1].TileRealExits &= ~Direction.South;
            }
            if (data.TileExitDirections.HasFlag(Direction.South))
            {
                if (y < 4){
                    if (tiles[x, y + 1].tileData.TileType == TileType.Locked) tiles[x, y + 1].UpdateTile(EmptyTile);
                    if (tiles[x, y+1].tileData.TileExitDirections.HasFlag(Direction.North))
                    {
                        tiles[x, y].TileRealExits |= Direction.South;
                        tiles[x, y + 1].TileRealExits |= Direction.North;
                    }
                    else
                    {
                        tiles[x, y].TileRealExits &= ~Direction.South;
                        tiles[x, y + 1].TileRealExits &= ~Direction.North;
                    }
                }
                else if (x == 3 && y == 4) //Boss Room
                {
                    tiles[x, y].TileRealExits |= Direction.South;
                }
            }
            else
            {
                tiles[x, y].TileRealExits &= ~Direction.South;
                if (y < 4)
                    tiles[x, y + 1].TileRealExits &= ~Direction.North;
            }

            if (data.TileExitDirections.HasFlag(Direction.West) && x > 0)
            {
                if (tiles[x - 1, y].tileData.TileType == TileType.Locked) tiles[x - 1, y].UpdateTile(EmptyTile);
                if (tiles[x - 1, y].tileData.TileExitDirections.HasFlag(Direction.East))
                {
                    tiles[x, y].TileRealExits |= Direction.West;
                    tiles[x - 1, y].TileRealExits |= Direction.East;
                }
                else
                {
                    tiles[x, y].TileRealExits &= ~Direction.West;
                    tiles[x - 1, y].TileRealExits &= ~Direction.East;
                }
            }
            else
            {
                tiles[x, y].TileRealExits &= ~Direction.West;
                if (x > 0)
                    tiles[x - 1, y].TileRealExits &= ~Direction.East;
            }
            if (data.TileExitDirections.HasFlag(Direction.East) && x < 5)
            {
                if (tiles[x + 1, y].tileData.TileType == TileType.Locked) tiles[x + 1, y].UpdateTile(EmptyTile);
                if (tiles[x + 1, y].tileData.TileExitDirections.HasFlag(Direction.West))
                {
                    tiles[x, y].TileRealExits |= Direction.East;
                    tiles[x + 1, y].TileRealExits |= Direction.West;
                }
                else
                {
                    tiles[x, y].TileRealExits &= ~Direction.East;
                    tiles[x + 1, y].TileRealExits &= ~Direction.West;
                }
            }
            else
            {
                tiles[x, y].TileRealExits &= ~Direction.East;
                if(x < 5)
                    tiles[x + 1, y].TileRealExits &= ~Direction.West;
            }
        }
    }

    public bool SetStartPosition((int x, int y) position)
    {
        LoadDungeonPathSolver();
        startingPosition = position;
        StartIcon.transform.parent = tiles[position.x, position.y].transform;
        StartIcon.transform.localPosition = Vector3.zero; 
        isValidStartPosition = currentDungeon.setStartingRoomLocation(position);
        return isValidStartPosition;
    }

    private void LoadDungeonPathSolver()
    {
        currentDungeon = new DungeonLayout(6, 6);
        currentDungeon.bossRoomLocation = (3, 5);

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1)+1; j++)
            {
                if (j == 5)
                {
                    DungeonRoom currentRoom = new DungeonRoom();
                    currentDungeon.setRoom(i, j, currentRoom);
                }
                else
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
        }
        DungeonRoom bossRoom = new DungeonRoom();
        bossRoom.door = (true, false, false, false);
        currentDungeon.setRoom(3, 5, bossRoom);
    }

    public void StartNewDungeon()
    {
        if (!IsRaidCurrentlyHapening && SetStartPosition(startingPosition))
        {
            currentWalker = new DungeonWalker(currentDungeon, currentDungeon.getStartingRoomLocation(), 0);
            Player.transform.position = tiles[currentDungeon.getStartingRoomLocation().x, currentDungeon.getStartingRoomLocation().y].gameObject.transform.position + Vector3.back;
            IsRaidCurrentlyHapening = true;
            currentTimer = 0;
        }
    }

    [SerializeField] GameObject Player;
    [SerializeField] float stepTimeDelay = 1f;
    private float currentTimer = 0;
    public bool IsRaidCurrentlyHapening = false;
    void Update()
    {

        if (IsRaidCurrentlyHapening)
        {
            if (currentTimer > stepTimeDelay)
            {
                (int x, int y) newPosition = currentWalker.moveStep();
                if (newPosition == (3, 5))
                {
                    Debug.Log("Boss");
                    Player.transform.position = BossRoom.transform.position + Vector3.back;
                    IsRaidCurrentlyHapening = false;
                    GameManager.Instance.DayEnd();
                }
                else
                {
                    Player.transform.position = tiles[newPosition.x, newPosition.y].gameObject.transform.position + Vector3.back;
                    currentTimer = 0;
                }
            }
            currentTimer += Time.deltaTime;
        }
    }
}
