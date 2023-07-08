using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class TestDungeonController : MonoBehaviour
{

    [SerializeField] GameObject Wall;

    [SerializeField] GameObject Player;

    [SerializeField] int width = 10;
    [SerializeField] int height = 5;
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
        currentDungeon.startingRoomLocation = (x/2,y -1);

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                DungeonRoom currentRoom = new DungeonRoom(0);
                currentRoom.door = (rand(wallChance), rand(wallChance), rand(wallChance), rand(wallChance)); 
                currentDungeon.setRoom(i, j, currentRoom);
            }
        }

        placeRooms();

        currentWalker = new DungeonWalker(currentDungeon, currentDungeon.startingRoomLocation, 0);
        Player.transform.position = new Vector3((currentDungeon.startingRoomLocation.x * 4) + 2, (currentDungeon.startingRoomLocation.y * 4) + 2, 0);
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
                if (!currentDungeon.canTravel(currentDungeon.Room(i, j), DungeonLayout.Direction.South))
                {
                    placeBlock((i * 4) + 2, (j * 4));
                }
                if (!currentDungeon.canTravel(currentDungeon.Room(i, j), DungeonLayout.Direction.West))
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
            if (!currentDungeon.canTravel(currentDungeon.Room(i, currentDungeon.size.y - 1), DungeonLayout.Direction.North))
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
            if (!currentDungeon.canTravel(currentDungeon.Room(currentDungeon.size.x - 1, j), DungeonLayout.Direction.East))
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
