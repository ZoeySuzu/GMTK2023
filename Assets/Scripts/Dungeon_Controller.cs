using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private (int x, int y) startingPosition = (3, 3);
    private bool isValidStartPosition = true;

    [SerializeField] Enemy_Data_Holder lilim;
    [SerializeField] Enemy_Data_Holder azura;


    public void Start()
    {
        for(int i = 0; i < alltiles.Length; i++)
        {
            int x = i % 6;
            int y = Mathf.FloorToInt(i / 6);
            tiles[x, y] = alltiles[i];
            tiles[x, y].UpdateTilePosition(x, y);
        }

        SetTile(3, 4, EmptyRoom);
        SetTile(3, 3, EmptyRoom);
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

        float intel = GameManager.Instance.team.NpcList.Sum(x => x.Inteligence) / GameManager.Instance.team.NpcList.Count;
        if (intel > 100) { intel = 100; } else if (intel < 10) { intel = 10; }
        currentDungeon.inteligance = intel;

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1)+1; j++)
            {
                if (j == 5)
                {
                    DungeonRoom currentRoom = new DungeonRoom(null);
                    currentDungeon.setRoom(i, j, currentRoom);
                }
                else
                {
                    Dungeon_Tile roomtile = tiles[i, j];
                    roomtile.enemies.ForEach(x => x.Health = x.MaxHealth);
                    DungeonRoom currentRoom = new DungeonRoom(roomtile);
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
        DungeonRoom bossRoom = new DungeonRoom(null);
        bossRoom.door = (true, false, false, false);
        currentDungeon.setRoom(3, 5, bossRoom);
    }

    public void StartNewDungeon()
    {
        if (!IsRaidCurrentlyHapening && SetStartPosition(startingPosition))
        {
            currentWalker = new DungeonWalker(currentDungeon, currentDungeon.getStartingRoomLocation(), 0);
            Player.transform.position = tiles[currentDungeon.getStartingRoomLocation().x, currentDungeon.getStartingRoomLocation().y].gameObject.transform.position + Vector3.back;
            if (tiles[currentDungeon.getStartingRoomLocation().x, currentDungeon.getStartingRoomLocation().y].enemies.Count > 0) StartBattle(tiles[currentDungeon.getStartingRoomLocation().x, currentDungeon.getStartingRoomLocation().y]);
            IsRaidCurrentlyHapening = true;
            currentTimer = 0;
            GameManager.Instance.DayStart();
            Player.SetActive(true);
        }
    }

    [SerializeField] GameObject Player;
    [SerializeField] float stepTimeDelay = 1f;
    private float currentTimer = 0;
    public bool IsRaidCurrentlyHapening = false;
    public bool InBattle = false;
    public bool InBossBattle = false;
    public bool InAzuraBossBattle = false;
    private BattleController battleController;
    private Dungeon_Tile battleTile;

    public void SetStepSpeed(float value)
    {
        stepTimeDelay = value;
    }

    void Update()
    {

        if (IsRaidCurrentlyHapening )
        {
            if (currentTimer > stepTimeDelay)
            {
                if (InBattle) {
                    currentTimer = 0;
                    if (battleController.Tick()) 
                    { 
                        InBattle = false;
                        if (InBossBattle) {
                            if (lilim.obj.dead)
                            {
                                battleController = new BattleController(new List<Enemy_Object>() { azura.obj }, GameManager.Instance.team);
                                InBossBattle = false;
                                InAzuraBossBattle = true;
                            }
                            else
                            {
                                IsRaidCurrentlyHapening = false;
                                azura.obj.TakeDamage(-200);
                                lilim.obj.TakeDamage(-100);
                                GameManager.Instance.DayEnd();
                            }
                        }
                        else if (InAzuraBossBattle)
                        {
                            if (azura.obj.Health == 0)
                            {
                                GameManager.Instance.GameOver(0);
                            }
                            else
                            {
                                GameManager.Instance.GameOver(2);
                            }
                        }
                        else
                        {
                            foreach (Enemy_Object enemy in battleTile.enemies.ToList())
                            {
                                if (enemy.dead)
                                {
                                    if (enemy.enemyType == EnemyType.Slimeow && Random.Range(0, 4) == 1)
                                    {
                                        enemy.dead = false; enemy.Health = enemy.MaxHealth;
                                        enemy.enemyObject.GetComponent<Animator>().enabled = true;
                                    }
                                    else
                                    {
                                        battleTile.enemies.Remove(enemy);
                                        Destroy(enemy.enemyObject);
                                    }
                                }
                            }
                        }
                    }
                    if (battleController.heroesDead)
                    {
                        IsRaidCurrentlyHapening = false;
                        Player.SetActive(false);
                        azura.obj.TakeDamage(-200);
                        lilim.obj.TakeDamage(-100);
                        GameManager.Instance.DayEnd();
                    }
                    GameManager.Instance.teamUI.UpdateTeam(GameManager.Instance.team);
                }
                else
                {

                    (int x, int y) newPosition = currentWalker.moveStep();
                    if (newPosition == (3, 5))
                    {
                        Player.transform.position = BossRoom.transform.position + Vector3.back;
                                InBossBattle = true;
                        StartBossBattle();
                    }
                    else
                    {
                        Player.transform.position = tiles[newPosition.x, newPosition.y].gameObject.transform.position + Vector3.back;
                        if (tiles[newPosition.x, newPosition.y].enemies.Count > 0) StartBattle(tiles[newPosition.x, newPosition.y]);
                        currentTimer = 0;

                    }
                }
            }
            currentTimer += Time.deltaTime;
        }
    }


    private void StartBattle(Dungeon_Tile tile) {
        battleTile = tile;
        InBattle = true;
        battleController = new BattleController(tile.enemies, GameManager.Instance.team);
    }
    private void StartBossBattle()
    {

        InBattle = true;
        battleController = new BattleController(new List<Enemy_Object>() { lilim.obj }, GameManager.Instance.team);
    }
}
