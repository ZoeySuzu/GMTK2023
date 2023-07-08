using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TestDungeonController : MonoBehaviour
{

    [SerializeField] GameObject Wall;

    [SerializeField] int width = 10;
    [SerializeField] int height = 5;
    [SerializeField] double wallChance = 20;

    private DungeonLayout currentDungeon;
    private DungeonWalker currentWalker;

    // Start is called before the first frame update
    void Start()
    {
        setupDungeon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setupDungeon()
    {
        int x = width;
        int y = height;

        currentDungeon = new DungeonLayout(x, y);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                DungeonRoom currentRoom = new DungeonRoom(i, j, 0);
                currentRoom.entryNorth = Random.Range(0, 100) >= wallChance ? false : true;
                currentRoom.entryEast = Random.Range(0, 100) >= wallChance ? false : true;
                currentRoom.entrySouth = Random.Range(0, 100) >= wallChance ? false : true;
                currentRoom.entryWest = Random.Range(0, 100) >= wallChance ? false : true;
                currentDungeon.setRoom(i, j, currentRoom);
            }
        }
        for (int i = 0; i < x; i++)
        {
            currentDungeon.getRoom(i, 0).entrySouth = true;
            currentDungeon.getRoom(i, y - 1).entryNorth = true;
        }
        for (int j = 0; j < y; j++)
        {
            currentDungeon.getRoom(0, j).entryWest = true;
            currentDungeon.getRoom(x - 1, j).entryEast = true;
        }

        placeRooms();
    }

    private void placeRooms()
    {
        for (int i = 0; i < currentDungeon.gridWidth; i++)
        {
            for (int j = 0; j < currentDungeon.gridHeight; j++)
            {
                //Frame
                placeBlock((i * 4), (j * 4));
                placeBlock((i * 4) + 1, (j * 4));
                placeBlock((i * 4) + 3, (j * 4));
                placeBlock((i * 4), (j * 4) + 1);
                placeBlock((i * 4), (j * 4) + 3);

                //Doors
                if ((i < currentDungeon.gridWidth - 1) && (currentDungeon.getRoom(i, j).entryEast || currentDungeon.getRoom(i + 1, j).entryWest))
                {
                    placeBlock((i * 4) + 4, (j * 4) + 2);
                }
                if (j < currentDungeon.gridHeight - 1 && (currentDungeon.getRoom(i, j).entryNorth || currentDungeon.getRoom(i, j + 1).entrySouth))
                {
                    placeBlock((i * 4) + 2, (j * 4) + 4);
                }
            }
        }
        for (int i = 0; i < currentDungeon.gridWidth; i++)
        {
            //Frame
            placeBlock((i * 4), (currentDungeon.gridHeight * 4));
            placeBlock((i * 4) + 1, (currentDungeon.gridHeight * 4));
            placeBlock((i * 4) + 3, (currentDungeon.gridHeight * 4));

            //Outer Doors
            placeBlock((i * 4) + 2, 0);
            placeBlock((i * 4) + 2, (currentDungeon.gridHeight * 4));
        }
        for (int j = 0; j < currentDungeon.gridHeight; j++)
        {
            //Frame
            placeBlock((currentDungeon.gridWidth * 4), (j * 4));
            placeBlock((currentDungeon.gridWidth * 4), (j * 4) + 1);
            placeBlock((currentDungeon.gridWidth * 4), (j * 4) + 3);

            //Outer Doors
            placeBlock(0, (j * 4) + 2);
            placeBlock((currentDungeon.gridWidth * 4), (j * 4) + 2);
        }
        placeBlock((currentDungeon.gridWidth * 4), (currentDungeon.gridHeight * 4));
    }

    private void placeBlock(int x, int y)
    {
        Instantiate(Wall, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0), transform);
    }
}
