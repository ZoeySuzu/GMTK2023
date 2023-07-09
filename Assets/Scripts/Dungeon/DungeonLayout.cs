using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonLayout
{
	private DungeonRoom[,] rooms;

	public (int x, int y) size { get; set; }
	private (int x, int y) startingRoomLocation;
	public (int x, int y) bossRoomLocation { get; set; }

	private int[,] dungeonWalk;

    public int[,] getDungeonWalk()
    {
        return dungeonWalk;
    }

    public DungeonLayout(int width, int height)
	{
		rooms = new DungeonRoom[width, height];
		size = (width, height);
	}

	public DungeonRoom Room(int x, int y)
	{
		if (rooms[x, y] == null)
		{
			DungeonRoom room = new DungeonRoom(null);
			room.x = x; 
			room.y = y;
			return room;
		}
		return rooms[x,y];
	}

	public void setRoom(int x, int y, DungeonRoom room)
	{
		room.x = x;
		room.y = y;
		rooms[x,y] = room;
	}

	public bool canTravel(DungeonRoom room, Direction direction) {
		switch (direction)
		{
			case Direction.North:
				return room.y - 1 >= 0 && room.door.north && Room(room.x, room.y - 1).door.south;
			case Direction.East:
                return room.x + 1 < size.x && room.door.east && Room(room.x + 1, room.y).door.west;
            case Direction.South:
                return room.y + 1 < size.y && room.door.south && Room(room.x, room.y + 1).door.north;
            case Direction.West:
                return room.x - 1 >= 0 && room.door.west && Room(room.x - 1, room.y).door.east;
			default: 
				return false;
        }
	}

	public (int x, int y) getStartingRoomLocation()
	{
		return startingRoomLocation;
	}

	public bool setStartingRoomLocation((int, int) startRoom)
	{
		startingRoomLocation = startRoom;
		return validateDungeonLayout();
	}

	private bool validateDungeonLayout()
	{
        dungeonWalk = new int[size.x, size.y];

        walkerSnake(bossRoomLocation, new bool[size.x, size.y], 0);

        return dungeonWalk[startingRoomLocation.x, startingRoomLocation.y] != 0;
	}

    private void walkerSnake((int x, int y) currentPosition, bool[,] visited, int currentWalkDistance)
    {
        bool walk = true;
        while (walk)
        {
            visited[currentPosition.x, currentPosition.y] = true;
            currentWalkDistance++;
            if (dungeonWalk[currentPosition.x, currentPosition.y] == 0 || currentWalkDistance < dungeonWalk[currentPosition.x, currentPosition.y])
            {
                dungeonWalk[currentPosition.x, currentPosition.y] = currentWalkDistance;
            }

            List<Direction> entryWays = new List<Direction>();

            if (canTravel(Room(currentPosition.x, currentPosition.y), Direction.North) && !visited[currentPosition.x, currentPosition.y - 1])
            {
                entryWays.Add(Direction.North);
            }
            if (canTravel(Room(currentPosition.x, currentPosition.y), Direction.East) && !visited[currentPosition.x + 1, currentPosition.y])
            {
                entryWays.Add(Direction.East);
            }
            if (canTravel(Room(currentPosition.x, currentPosition.y), Direction.South) && !visited[currentPosition.x, currentPosition.y + 1])
            {
                entryWays.Add(Direction.South);
            }
            if (canTravel(Room(currentPosition.x, currentPosition.y), Direction.West) && !visited[currentPosition.x - 1, currentPosition.y])
            {
                entryWays.Add(Direction.West);
            }

            if (entryWays.Count > 1)
            {
                //Recurse Split
                foreach (Direction way in entryWays) {
                    switch (way)
                    {
                        case Direction.North:
                            walkerSnake((currentPosition.x, currentPosition.y - 1), copyBoolArray(visited), currentWalkDistance);
                            visited[currentPosition.x, currentPosition.y - 1] = true;
                            break;
                        case Direction.East:
                            walkerSnake((currentPosition.x + 1, currentPosition.y), copyBoolArray(visited), currentWalkDistance);
                            visited[currentPosition.x + 1, currentPosition.y] = true;
                            break;
                        case Direction.South:
                            walkerSnake((currentPosition.x, currentPosition.y + 1), copyBoolArray(visited), currentWalkDistance);
                            visited[currentPosition.x, currentPosition.y + 1] = true;
                            break;
                        case Direction.West:
                            walkerSnake((currentPosition.x - 1, currentPosition.y), copyBoolArray(visited), currentWalkDistance);
                            visited[currentPosition.x - 1, currentPosition.y] = true;
                            break;
                    }
                }
            } else {
                if (entryWays.Count == 1)
                {
                    switch (entryWays[0])
                    {
                        case Direction.North:
                            currentPosition = (currentPosition.x, currentPosition.y - 1);
                            break;
                        case Direction.East:
                            currentPosition = (currentPosition.x + 1, currentPosition.y);
                            break;
                        case Direction.South:
                            currentPosition = (currentPosition.x, currentPosition.y + 1);
                            break;
                        case Direction.West:
                            currentPosition = (currentPosition.x - 1, currentPosition.y);
                            break;
                    }
                } else {
                    walk = false;
                }
            }
        }
    }

    private bool[,] copyBoolArray(bool[,] a)
    {
        bool[,] b = new bool[a.GetLength(0), a.GetLength(1)];
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                b[i, j] = a[i, j];
            }
        }
        return b;
    }
}
