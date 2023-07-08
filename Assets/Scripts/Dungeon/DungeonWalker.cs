using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonWalker
{

    private DungeonLayout dungeon;
    private int[,] visited;
    private DungeonRoom currentRoom;
    private Direction previousDirection = Direction.North;
    private float inteligance;

    private int[,] stepsAway;

    public DungeonWalker(DungeonLayout currentDungeon, (int x, int y) startingPosition, float inteligance)
    {
        dungeon = currentDungeon;
        currentRoom = dungeon.Room(startingPosition.x, startingPosition.y);
        this.inteligance = inteligance;

        visited = new int[dungeon.size.x, dungeon.size.y];
        stepsAway = new int[dungeon.size.x, dungeon.size.y];
        visited[currentRoom.x, currentRoom.y] += 1;
    }

    public (int x, int y) moveStep()
    {
        int visitedWeight = 1;
        List<(Direction direction, int visited)> entryWays = new List<(Direction direction, int visited)>();
        if (dungeon.canTravel(currentRoom, Direction.North))
        {
            entryWays.Add((Direction.North, visited[currentRoom.x, currentRoom.y + 1]));
        }
        if (dungeon.canTravel(currentRoom, Direction.East))
        {
            entryWays.Add((Direction.East, visited[currentRoom.x + 1, currentRoom.y]));
        }
        if (dungeon.canTravel(currentRoom, Direction.South))
        {
            entryWays.Add((Direction.South, visited[currentRoom.x, currentRoom.y - 1]));
        }
        if (dungeon.canTravel(currentRoom, Direction.West))
        {
            entryWays.Add((Direction.West, visited[currentRoom.x - 1, currentRoom.y]));
        }

        entryWays = entryWays.OrderBy(x => x.visited).ToList();

        if (entryWays.Count == 1)
        {
            visitedWeight = 20;
        }

        bool matching = true;
        List<(Direction direction, int visited)> pathChoices = new List<(Direction direction, int visited)>() { entryWays[0] };
        for (int i = 1; i < entryWays.Count; i++)
        {
            if (matching && entryWays[i].visited == entryWays[i - 1].visited)
            {
                pathChoices.Add(entryWays[i]);
            }
            else
            {
                matching = false;
            }
        }
        //Debug.Log("before: " + pathChoices.Count + " " + previousDirection);
        
        entryWays = new List<(Direction direction, int visited)>();
        if (pathChoices.Count > 1)
        {
            for (int i = 0; i < pathChoices.Count; i++)
            {
                if (pathChoices[i].direction != previousDirection) {
                    entryWays.Add(pathChoices[i]);
                }
            }
            pathChoices = entryWays;
        }

        //Debug.Log("after: " + pathChoices.Count);
        

        //ADD INTELEGANCE HERE
        int choice = Random.Range(0, pathChoices.Count);
        //--

        Debug.Log(pathChoices[choice].visited);

        //Move
        switch (pathChoices[choice].direction)
        {
            case Direction.North:
                currentRoom = dungeon.Room(currentRoom.x, currentRoom.y + 1);
                previousDirection = Direction.South;
                break;
            case Direction.East:
                currentRoom = dungeon.Room(currentRoom.x + 1, currentRoom.y);
                previousDirection = Direction.West;
                break;
            case Direction.South:
                currentRoom = dungeon.Room(currentRoom.x, currentRoom.y - 1);
                previousDirection = Direction.North;
                break;
            case Direction.West:
                currentRoom = dungeon.Room(currentRoom.x - 1, currentRoom.y);
                previousDirection = Direction.East;
                break;
        }
        Debug.Log(visited[currentRoom.x, currentRoom.y]);
        visited[currentRoom.x, currentRoom.y] += visitedWeight;

        return (currentRoom.x, currentRoom.y);
    }
}