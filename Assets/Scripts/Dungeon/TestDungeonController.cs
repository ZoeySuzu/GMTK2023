using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class TestDungeonController : MonoBehaviour
{

    [SerializeField] GameObject Wall;

    [SerializeField] GameObject Player;

    [SerializeField] int width = 6;
    [SerializeField] int height = 6;
    [SerializeField] double wallChance = 30;

    [SerializeField] float stepTimeDelay = 1f;

    private float currentTimer = 0;

    private DungeonLayout currentDungeon;
    private DungeonWalker currentWalker;

    // Start is called before the first frame update
    void Start()
    {
        setupTestDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimer > stepTimeDelay)
        {
            (int x, int y) newPosition = currentWalker.moveStep();
            Player.transform.position = new Vector3((newPosition.x * 4) + 2, (newPosition.y * 4) + 2, 0);
            currentTimer = 0;
        }
        currentTimer += Time.deltaTime;
    }

    private bool rand(double chance)
    {
        return Random.Range(0, 100) >= chance ? true : false;
    }

    private void setupTestDungeon()
    {
        int x = width;
        int y = height;

        currentDungeon = new DungeonLayout(x, y);
        currentDungeon.bossRoomLocation = (3, 0);

        for (int i = 0; i < x; i++)
        {
            for (int j = 1; j < y; j++)
            {
                DungeonRoom currentRoom = new DungeonRoom(0);
                currentRoom.door = (rand(wallChance), rand(wallChance), rand(wallChance), rand(wallChance)); 
                currentDungeon.setRoom(i, j, currentRoom);
            }
        }

        DungeonRoom bossRoom = new DungeonRoom(0);
        bossRoom.door = (true, false, false, false);
        currentDungeon.setRoom(currentDungeon.bossRoomLocation.x, currentDungeon.bossRoomLocation.y, bossRoom);

        (bool a, bool b, bool c, bool d) t = currentDungeon.Room(currentDungeon.bossRoomLocation.x, currentDungeon.bossRoomLocation.y + 1).door;
        currentDungeon.Room(currentDungeon.bossRoomLocation.x, currentDungeon.bossRoomLocation.y + 1).door = (t.a, t.b, true, t.d);

        placeRooms();

        Debug.Log(currentDungeon.setStartingRoomLocation((x / 2, y - 1)));

        currentWalker = new DungeonWalker(currentDungeon, currentDungeon.getStartingRoomLocation(), 0.5f);
        Player.transform.position = new Vector3((currentDungeon.getStartingRoomLocation().x * 4) + 2, (currentDungeon.getStartingRoomLocation().y * 4) + 2, 0);
    }

    private void placeRooms()
    {
        for (int i = 0; i < currentDungeon.size.x; i++)
        {
            for (int j = 0; j < currentDungeon.size.y; j++)
            {
                //Frame
                placeBlock((i * 4), (j * 4));
                placeBlock((i * 4) + 1, (j * 4));
                placeBlock((i * 4) + 3, (j * 4));
                placeBlock((i * 4), (j * 4) + 1);
                placeBlock((i * 4), (j * 4) + 3);

                //Doors
                if (!currentDungeon.canTravel(currentDungeon.Room(i, j), Direction.South))
                {
                    placeBlock((i * 4) + 2, (j * 4));
                }
                if (!currentDungeon.canTravel(currentDungeon.Room(i, j), Direction.West))
                {
                    placeBlock((i * 4), (j * 4) + 2);
                }
            }
        }
        for (int i = 0; i < currentDungeon.size.x; i++)
        {
            //Frame
            placeBlock((i * 4), (currentDungeon.size.y * 4));
            placeBlock((i * 4) + 1, (currentDungeon.size.y * 4));
            placeBlock((i * 4) + 3, (currentDungeon.size.y * 4));

            //Outer Doors
            if (!currentDungeon.canTravel(currentDungeon.Room(i, currentDungeon.size.y - 1), Direction.North))
            {
                placeBlock((i * 4) + 2, (currentDungeon.size.y * 4));
            }
        }
        for (int j = 0; j < currentDungeon.size.y; j++)
        {
            //Frame
            placeBlock((currentDungeon.size.x * 4), (j * 4));
            placeBlock((currentDungeon.size.x * 4), (j * 4) + 1);
            placeBlock((currentDungeon.size.x * 4), (j * 4) + 3);

            //Outer Doors
            if (!currentDungeon.canTravel(currentDungeon.Room(currentDungeon.size.x - 1, j), Direction.East))
            {
                placeBlock((currentDungeon.size.x * 4), (j * 4) + 2);
            }
        }
        placeBlock((currentDungeon.size.x * 4), (currentDungeon.size.y * 4));
    }

    private void placeBlock(int x, int y)
    {
        Instantiate(Wall, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0), transform);
    }
}
